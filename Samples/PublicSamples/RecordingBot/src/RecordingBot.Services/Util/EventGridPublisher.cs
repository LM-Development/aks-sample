using Azure;
using Azure.Messaging.EventGrid;
using RecordingBot.Model.Constants;
using RecordingBot.Model.Models;
using RecordingBot.Services.Contract;
using RecordingBot.Services.ServiceSetup;
using System;

namespace RecordingBot.Services.Util
{
    public class EventGridPublisher : IEventPublisher
    {
        private readonly string _topicName = "recordingbotevents";
        private readonly string _regionName = string.Empty;
        private readonly string _topicKey = string.Empty;

        public EventGridPublisher(AzureSettings settings)
        {
            _topicName = settings.TopicName ?? "recordingbotevents";
            _topicKey = settings.TopicKey;
            _regionName = settings.RegionName;
        }

        public void Publish(string subject, string message, string topicName)
        {
            topicName ??= _topicName;

            var topicEndpoint = string.Format(BotConstants.topicEndpoint, topicName, _regionName);

            if (!string.IsNullOrEmpty(_topicKey))
            {
                var client = new EventGridPublisherClient(new Uri(topicEndpoint), new AzureKeyCredential(_topicKey));

                var eventGrid = new EventGridEvent(subject, "RecordingBot.BotEventData", "2.0", new BotEventData { Message = message })
                {
                    EventTime = DateTime.Now
                };

                client.SendEvent(eventGrid);

                if (subject.StartsWith("CallTerminated"))
                {
                    Console.WriteLine($"Publish to {topicName} subject {subject} message {message}");
                }
            }
            else
            {
                Console.WriteLine($"Skipped publishing {subject} events to Event Grid topic {topicName} - No topic key specified");
            }
        }
    }
}
