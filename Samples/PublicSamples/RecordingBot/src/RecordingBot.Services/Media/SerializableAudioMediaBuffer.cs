// ***********************************************************************
// Assembly         : RecordingBot.Services
// Author           : JasonTheDeveloper
// Created          : 09-07-2020
//
// Last Modified By : dannygar
// Last Modified On : 08-17-2020
// ***********************************************************************
// <copyright file="SerializableAudioMediaBuffer.cs" company="Microsoft">
//     Copyright ©  2020
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.Graph.Communications.Calls;
using Microsoft.Graph.Models;
using Microsoft.Skype.Bots.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace RecordingBot.Services.Media
{
    /// <summary>
    /// Class SerializableAudioMediaBuffer.
    /// Implements the <see cref="System.IDisposable" />
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class SerializableAudioMediaBuffer : IDisposable
    {
        public uint[] ActiveSpeakers { get; set; }
        public long Length { get; set; }
        public bool IsSilence { get; set; }
        public long Timestamp { get; set; }
        public byte[] Buffer { get; set; }
        public SerializableUnmixedAudioBuffer[] SerializableUnmixedAudioBuffers { get; set; }

        private readonly List<IParticipant> participants;

        public SerializableAudioMediaBuffer()
        {
            participants = [];
        }

        public SerializableAudioMediaBuffer(AudioMediaBuffer buffer, List<IParticipant> participants)
        {
            this.participants = participants;

            Length = buffer.Length;
            ActiveSpeakers = buffer.ActiveSpeakers;
            IsSilence = buffer.IsSilence;
            Timestamp = buffer.Timestamp;

            if (Length > 0)
            {
                Buffer = new byte[Length];
                Marshal.Copy(buffer.Data, Buffer, 0, (int)Length);
            }

            if (buffer.UnmixedAudioBuffers != null)
            {
                SerializableUnmixedAudioBuffers = buffer.UnmixedAudioBuffers
                    .Where(w => w.Length > 0)
                    .Select(s => new SerializableUnmixedAudioBuffer(s, GetParticipantFromMSI(s.ActiveSpeakerId)))
                    .ToArray();
            }
        }

        private IParticipant GetParticipantFromMSI(uint msi)
        {
            return participants.FirstOrDefault(w => !w.Resource.IsInLobby.HasValue && w.Resource.MediaStreams.Any(a => a.SourceId == msi.ToString()));
        }

        public void Dispose()
        {
            SerializableUnmixedAudioBuffers = null;
            Buffer = null;
        }

        public class SerializableUnmixedAudioBuffer
        {
            public uint ActiveSpeakerId { get; set; }
            public long Length { get; set; }
            public long OriginalSenderTimestamp { get; set; }
            public string DisplayName { get; set; }
            public string AdId { get; set; }
            public IDictionary<string, object> AdditionalData { get; set; }
            public byte[] Buffer { get; set; }

            public SerializableUnmixedAudioBuffer()
            {

            }

            public SerializableUnmixedAudioBuffer(UnmixedAudioBuffer buffer, IParticipant participant)
            {
                ActiveSpeakerId = buffer.ActiveSpeakerId;
                Length = buffer.Length;
                OriginalSenderTimestamp = buffer.OriginalSenderTimestamp;

                var identity = AddParticipant(participant);

                if (identity != null)
                {
                    DisplayName = identity.DisplayName;
                    AdId = identity.Id;
                }
                else
                {
                    var user = participant?.Resource?.Info?.Identity?.User;
                    if (user != null)
                    {
                        DisplayName = user.DisplayName;
                        AdId = user.Id;
                        AdditionalData = user.AdditionalData;
                    }
                }

                Buffer = new byte[Length];
                Marshal.Copy(buffer.Data, Buffer, 0, (int)Length);
            }

            private static Identity AddParticipant(IParticipant participant)
            {
                if (participant?.Resource?.Info?.Identity?.AdditionalData != null)
                {
                    return participant.Resource.Info.Identity.AdditionalData
                        .Where(w => w.Key != "applicationInstance" && w.Value is Identity)
                        .Select(s => s.Value as Identity)
                        .FirstOrDefault();
                }
                return null;
            }
        }
    }
}
