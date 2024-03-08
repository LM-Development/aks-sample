using Microsoft.Skype.Bots.Media;

namespace RecordingBot.Services.Media
{
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
