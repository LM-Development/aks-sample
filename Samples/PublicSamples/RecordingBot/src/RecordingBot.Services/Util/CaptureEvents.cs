using Microsoft.Graph.Communications.Calls;
using Microsoft.Graph.Communications.Resources;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using RecordingBot.Model.Extension;
using RecordingBot.Model.Models;
using RecordingBot.Services.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordingBot.Services.Util
{
    public class CaptureEvents : BufferBase<object>
    {
        private readonly string _path;
        private readonly JsonSerializer _serializer;

        public CaptureEvents(string path)
        {
            _path = path;
            _serializer = new JsonSerializer();
        }

        private async Task SaveFile<T>(T data, string fileName)
        {
            Directory.CreateDirectory(_path);

            var fullName = Path.Combine(_path, fileName);

            using (var stream = File.CreateText(fullName))
            using (var writer = new JsonTextWriter(stream))
            {
                writer.Formatting = Formatting.Indented;
                _serializer.Serialize(writer, data);
                await writer.FlushAsync();
            }
        }

        private async Task SaveBsonFile(object data, string fileName)
        {
            Directory.CreateDirectory(_path);

            var fullName = Path.Combine(_path, fileName);

            using (var file = File.Create(fullName))
            using (var writer = new BsonDataWriter(file))
            {
                _serializer.Serialize(writer, data);
                await writer.FlushAsync();
            }
        }

        private async Task SaveQualityOfExperienceData(SerializableAudioQualityOfExperienceData data)
        {
            await SaveFile(data, $"{data.Id}-AudioQoE.json");
        }

        private async Task SaveAudioMediaBuffer(SerializableAudioMediaBuffer data)
        {
            await SaveBsonFile(data, data.Timestamp.ToString());
        }

        private async Task SaveParticipantEvent(CollectionEventArgs<IParticipant> data)
        {
            var participantData = new ParticipantData
            {
                AddedResources = new List<IParticipant>(data.AddedResources.Select(p => new ParticipantExtension(p))),
                RemovedResources = new List<IParticipant>(data.RemovedResources.Select(p => new ParticipantExtension(p)))
            };

            await SaveFile(participantData, $"{DateTime.UtcNow.Ticks}-participant.json");
        }

        private async Task SaveRequests(string data)
        {
            Directory.CreateDirectory(_path);

            var fullName = Path.Combine(_path, $"{DateTime.UtcNow.Ticks}.json");

            await File.AppendAllTextAsync(fullName, data, Encoding.Unicode);
        }

        protected override async Task Process(object data)
        {
            switch (data)
            {
                case string d:
                    await SaveRequests(d);
                    break;
                case CollectionEventArgs<IParticipant> d:
                    await SaveParticipantEvent(d);
                    break;
                case SerializableAudioMediaBuffer d:
                    await SaveAudioMediaBuffer(d);
                    break;
                case SerializableAudioQualityOfExperienceData q:
                    await SaveQualityOfExperienceData(q);
                    break;
            }
        }

        public async Task Finalize()
        {
            while (Buffer.Count > 0)
            {
                await Task.Delay(200);
            }

            await End();
        }
    }
}
