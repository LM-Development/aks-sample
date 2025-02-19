namespace RecordingBot.Services.ServiceSetup
{
    public class WavSettings
    {
        public int? SampleRate { get; set; }
        public int? Quality { get; set; }

        public WavSettings(int sampleRate, int quality)
        {
            SampleRate = sampleRate;
            Quality = quality;
        }
    }
}
