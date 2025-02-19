namespace RecordingBot.Services.Contract
{
    public interface IEventPublisher
    {
        void Publish(string subject, string Message, string TopicName = "");
    }
}
