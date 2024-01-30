// ***********************************************************************
// Assembly         : RecordingBot.Services
// Author           : JasonTheDeveloper
// Created          : 09-07-2020
//
// Last Modified By : dannygar
// Last Modified On : 09-03-2020
// ***********************************************************************
// <copyright file="BotService.cs" company="Microsoft">
//     Copyright ©  2020
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.Graph.Communications.Calls;
using Microsoft.Graph.Communications.Calls.Media;
using Microsoft.Graph.Communications.Client;
using Microsoft.Graph.Communications.Common;
using Microsoft.Graph.Communications.Common.Telemetry;
using Microsoft.Graph.Communications.Resources;
using Microsoft.Graph.Contracts;
using Microsoft.Graph.Models;
using Microsoft.Skype.Bots.Media;
using RecordingBot.Model.Models;
using RecordingBot.Services.Authentication;
using RecordingBot.Services.Contract;
using RecordingBot.Services.ServiceSetup;
using RecordingBot.Services.Util;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace RecordingBot.Services.Bot
{
    /// <summary>
    /// Class BotService.
    /// Implements the <see cref="System.IDisposable" />
    /// Implements the <see cref="RecordingBot.Services.Contract.IBotService" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="RecordingBot.Services.Contract.IBotService" />
    public class BotService : IDisposable, IBotService
    {
        /// <summary>
        /// The logger
        /// </summary>
        private readonly IGraphLogger _logger;
        /// <summary>
        /// The event publisher
        /// </summary>
        private readonly IEventPublisher _eventPublisher;
        /// <summary>
        /// The settings
        /// </summary>
        private readonly AzureSettings _settings;
        public ConcurrentDictionary<string, CallHandler> CallHandlers { get; } = new ConcurrentDictionary<string, CallHandler>();

        /// <summary>
        /// Gets the entry point for stateful bot.
        /// </summary>
        /// <value>The client.</value>
        public ICommunicationsClient Client
        {
            get
            {
                if (_client == null)
                {
                    InitializeClient();
                }
                return _client;
            }
        }
        private ICommunicationsClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="BotService" /> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="eventPublisher">The event publisher.</param>
        /// <param name="settings">The settings.</param>
        public BotService(IGraphLogger logger, IEventPublisher eventPublisher, IAzureSettings settings)
        {
            _logger = logger;
            _eventPublisher = eventPublisher;
            _settings = (AzureSettings)settings;
        }

        public void Dispose()
        {
            _client?.Dispose();
            _client = null;
        }

        public void Initialize()
        {
            InitializeClient();
            RegisterCallEventHandlers();
        }

        /// <summary>
        /// End a particular call.
        /// </summary>
        /// <param name="callLegId">The call leg id.</param>
        /// <returns>The <see cref="Task" />.</returns>
        public async Task EndCallByCallLegIdAsync(string callLegId)
        {
            try
            {
                await GetHandlerOrThrow(callLegId).Call.DeleteAsync().ConfigureAwait(false);
            }
            catch (Exception)
            {
                // Manually remove the call from SDK state.
                // This will trigger the ICallCollection.OnUpdated event with the removed resource.
                _client.Calls().TryForceRemove(callLegId, out ICall _);
            }
        }

        /// <summary>
        /// Joins the call asynchronously.
        /// </summary>
        /// <param name="joinCallBody">The join call body.</param>
        /// <returns>The <see cref="ICall" /> that was requested to join.</returns>
        public async Task<ICall> JoinCallAsync(JoinCallBody joinCallBody)
        {
            // A tracking id for logging purposes. Helps identify this call in logs.
            var scenarioId = Guid.NewGuid();
            var (chatInfo, meetingInfo) = JoinInfo.ParseJoinURL(joinCallBody.JoinURL);
            var tenantId = (meetingInfo as OrganizerMeetingInfo).Organizer.GetPrimaryIdentity().GetTenantId();
            var mediaSession = CreateLocalMediaSession();

            var joinParams = new JoinMeetingParameters(chatInfo, meetingInfo, mediaSession)
            {
                TenantId = tenantId,
            };

            if (!string.IsNullOrWhiteSpace(joinCallBody.DisplayName))
            {
                // Teams client does not allow changing of ones own display name.
                // If display name is specified, we join as anonymous (guest) user
                // with the specified display name.  This will put bot into lobby
                // unless lobby bypass is disabled.
                joinParams.GuestIdentity = new Identity
                {
                    Id = Guid.NewGuid().ToString(),
                    DisplayName = joinCallBody.DisplayName,
                };
            }

            var statefulCall = await _client.Calls().AddAsync(joinParams, scenarioId).ConfigureAwait(false);
            statefulCall.GraphLogger.Info($"Call creation complete: {statefulCall.Id}");
            return statefulCall;
        }

        private void InitializeClient()
        {
            var name = GetType().Assembly.GetName().Name;
            var builder = new CommunicationsClientBuilder(name, _settings.AadAppId, _logger);
            var authProvider = new AuthenticationProvider(name, _settings.AadAppId, _settings.AadAppSecret, _logger);

            builder.SetAuthenticationProvider(authProvider);
            builder.SetNotificationUrl(_settings.CallControlBaseUrl);
            builder.SetMediaPlatformSettings(_settings.MediaPlatformSettings);
            builder.SetServiceBaseUrl(_settings.PlaceCallEndpointUrl);

            _client = builder.Build();
        }

        /// <summary>
        /// Creates the local media session.
        /// </summary>
        /// <param name="mediaSessionId">The media session identifier.
        /// This should be a unique value for each call.</param>
        /// <returns>The <see cref="ILocalMediaSession" />.</returns>
        private ILocalMediaSession CreateLocalMediaSession(Guid mediaSessionId = default)
        {
            try
            {
                // create media session object, this is needed to establish call connections
                return _client.CreateMediaSession(
                    new AudioSocketSettings
                    {
                        StreamDirections = StreamDirection.Recvonly,
                        // Note! Currently, the only audio format supported when receiving unmixed audio is Pcm16K
                        SupportedAudioFormat = AudioFormat.Pcm16K,
                        ReceiveUnmixedMeetingAudio = true //get the extra buffers for the speakers
                    },
                    new VideoSocketSettings
                    {
                        StreamDirections = StreamDirection.Inactive
                    },
                    mediaSessionId: mediaSessionId);
            }
            catch (Exception e)
            {
                _logger.Log(System.Diagnostics.TraceLevel.Error, e.Message);
                throw;
            }
        }

        private void RegisterCallEventHandlers()
        {
            _client.Calls().OnIncoming += CallsOnIncoming;
            _client.Calls().OnUpdated += CallsOnUpdated;
        }

        /// <summary>
        /// Incoming call handler.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="CollectionEventArgs{TResource}" /> instance containing the event data.</param>
        private void CallsOnIncoming(ICallCollection sender, CollectionEventArgs<ICall> args)
        {
            args.AddedResources.ForEach(call =>
            {
                var mediaSession = Guid.TryParse(call.Id, out Guid callId) ? CreateLocalMediaSession(callId) : CreateLocalMediaSession();
                call?.AnswerAsync(mediaSession).ForgetAndLogExceptionAsync(call.GraphLogger, $"Answering call {call.Id} with scenario {call.ScenarioId}.");
            });
        }

        /// <summary>
        /// Updated call handler.
        /// </summary>
        /// <param name="sender">The <see cref="ICallCollection" /> sender.</param>
        /// <param name="args">The <see cref="CollectionEventArgs{ICall}" /> instance containing the event data.</param>
        private void CallsOnUpdated(ICallCollection sender, CollectionEventArgs<ICall> args)
        {
            foreach (var call in args.AddedResources)
            {
                CallHandlers[call.Id] = new CallHandler(call, _settings, _eventPublisher);
            }

            foreach (var call in args.RemovedResources)
            {
                if (CallHandlers.TryRemove(call.Id, out CallHandler handler))
                {
                    handler.Dispose();
                }
            }
        }

        /// <summary>
        /// The get handler or throw.
        /// </summary>
        /// <param name="callLegId">The call leg id.</param>
        /// <returns>The <see cref="CallHandler" />.</returns>
        /// <exception cref="ArgumentException">call ({callLegId}) not found</exception>
        private CallHandler GetHandlerOrThrow(string callLegId)
        {
            if (!CallHandlers.TryGetValue(callLegId, out CallHandler handler))
            {
                throw new ArgumentException($"call ({callLegId}) not found");
            }

            return handler;
        }
    }
}
