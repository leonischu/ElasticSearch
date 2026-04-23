using Confluent.Kafka;
using JobPortal.Application.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobPortal.Infrastructure.Messaging
{
    public  class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<Null, string> _producer;   //kafka client object , sends  messages and stores connection to kafka server 
        private readonly KafkaSetting _settings;

        public KafkaProducer(KafkaSetting settings)
        {
            _settings = settings;

            var config = new ProducerConfig
            {
                BootstrapServers = settings.BootstrapServers,  // i am connecting to local kafka server 
                Acks = Acks.All,
                MessageSendMaxRetries = 3
            };

            _producer = new ProducerBuilder<Null, string>(config).Build();  // creates actual kafka connection 
        }
        public async Task ProduceAsync<T>(T message)   // can send any type of message since it is of generic type. 
        {
            var json = JsonSerializer.Serialize(message);  //Converts object json into string 
            await _producer.ProduceAsync(_settings.Topic, new Message<Null, string> { Value = json });  // Sends message to kafka topic , topic = job updated and value =  json message 
        }
    }
}
