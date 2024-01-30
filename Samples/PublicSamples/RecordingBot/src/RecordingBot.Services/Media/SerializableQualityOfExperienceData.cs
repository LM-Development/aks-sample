// ***********************************************************************
// Assembly         : RecordingBot.Services
// Author           : JasonTheDeveloper
// Created          : 09-07-2020
//
// Last Modified By : dannygar
// Last Modified On : 09-07-2020
// ***********************************************************************
// <copyright file="SerializableQualityOfExperienceData.cs" company="Microsoft">
//     Copyright ©  2020
// </copyright>
// <summary></summary>
// ***********************************************************************
using Microsoft.Skype.Bots.Media;

namespace RecordingBot.Services.Media
{
    /// <summary>
    /// Class SerializableAudioQualityOfExperienceData.
    /// </summary>
    public class SerializableAudioQualityOfExperienceData
    {
        public string Id { get; }
        public long AverageInBoundNetworkJitter { get; }
        public long MaximumInBoundNetworkJitter { get; }
        public long TotalMediaDuration { get; }

        public SerializableAudioQualityOfExperienceData(string id, AudioQualityOfExperienceData aQoE)
        {
            Id = id;
            AverageInBoundNetworkJitter = aQoE.AudioMetrics.AverageInboundNetworkJitter.Ticks;
            MaximumInBoundNetworkJitter = aQoE.AudioMetrics.MaximumInboundNetworkJitter.Ticks;
            TotalMediaDuration = aQoE.TotalMediaDuration.Ticks;
        }
    }
}
