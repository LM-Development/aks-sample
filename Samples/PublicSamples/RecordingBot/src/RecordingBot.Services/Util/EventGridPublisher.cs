// ***********************************************************************
// Assembly         : RecordingBot.Services
// Author           : JasonTheDeveloper
// Created          : 09-07-2020
//
// Last Modified By : dannygar
// Last Modified On : 09-07-2020
// ***********************************************************************
// <copyright file="EventGridPublisher.cs" company="Microsoft">
//     Copyright ©  2020
// </copyright>
// <summary></summary>
// ***********************************************************************
using Azure;
using Azure.Messaging.EventGrid;
using RecordingBot.Model.Constants;
using RecordingBot.Model.Models;
using RecordingBot.Services.Contract;
using RecordingBot.Services.ServiceSetup;
using System;

namespace RecordingBot.Services.Util
{
    /// <summary>
    /// Class EventGridPublisher.
    /// Implements the <see cref="RecordingBot.Services.Contract.IEventPublisher" />
    /// </summary>
    /// <seealso cref="RecordingBot.Services.Contract.IEventPublisher" />
    public class EventGridPublisher : IEventPublisher
    {
        private readonly string topicName;
        private readonly string regionName;
        private readonly string topicKey;

        public EventGridPublisher(AzureSettings settings)
        {
            topicName = settings.TopicName ?? "recordingbotevents";
            topicKey = settings.TopicKey;
            regionName = settings.RegionName;
        }

        public void Publish(string subject, string message, string topicName)
        {
            topicName ??= this.topicName;

            var topicEndpoint = string.Format(BotConstants.topicEndpoint, topicName, regionName);

            if (!string.IsNullOrEmpty(topicKey))
            {
                var client = new EventGridPublisherClient(new Uri(topicEndpoint), new AzureKeyCredential(topicKey));

                var eventGrid = new EventGridEvent(subject, "RecordingBot.BotEventData", "2.0", new BotEventData { Message = message })
                {
                    EventTime = DateTime.Now
                };

                client.SendEvent(eventGrid);

                if (subject.StartsWith("CallTerminated"))
                {
                    Console.WriteLine($"Publish to {topicName} subject {subject} message {message}");
                }
                else
                {
                    Console.WriteLine($"Publish to {topicName} subject {subject}");
                }
            }
            else
            {
                Console.WriteLine($"Skipped publishing {subject} events to Event Grid topic {topicName} - No topic key specified");
            }
        }
    }
}
