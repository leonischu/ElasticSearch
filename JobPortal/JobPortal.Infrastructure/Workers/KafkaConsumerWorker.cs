using Confluent.Kafka;
using JobPortal.Application.Events;
using JobPortal.Application.Interfaces;
using JobPortal.Domain.Models;
using JobPortal.Infrastructure.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JobPortal.Infrastructure.Workers
{
    public class KafkaConsumerWorker : BackgroundService  // runs in background forever ,always listening 
    {
        private readonly KafkaSetting _settings;  // Holds kafka server address topic name , id 
        private readonly IServiceScopeFactory _scopeFactory;   // factory that creates small container when needed , safely use scoped services like elastic service 

        public KafkaConsumerWorker(KafkaSetting settings, IServiceScopeFactory scopeFactory)
        {
            _settings = settings;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig   // Defines how worker connects to kafka 
            {
                BootstrapServers = _settings.BootstrapServers, //Kafka server address
                GroupId = _settings.GroupId, // Consumer group  name
                AutoOffsetReset = AutoOffsetReset.Earliest,  // if no saved position exist start reading from beginning
                EnableAutoCommit = false // saying kafka i will manually confirm when message is processed 
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();  // create consumer  , ignore = no key , string = message value is json 
            consumer.Subscribe(_settings.Topic);  // subscribe to topic , i.e job-updated

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var result = consumer.Consume(stoppingToken); // pauses until kafka senda message

                    //  handle null/tombstone safely
                    if (result?.Message?.Value == null)
                    {
                        consumer.Commit(result);
                        continue;
                    }

                    var debeziumEvent = JsonSerializer.Deserialize<DebeziumMessage<Job>>(result.Message.Value);

                    var job = debeziumEvent?.Payload?.After;

                    if (job == null)
                    {
                        // delete case or invalid message
                        consumer.Commit(result);
                        continue;
                    }

                    var payload = debeziumEvent.Payload;

                    //  Create scope here
                    using (var scope = _scopeFactory.CreateScope())   // creates temporary DI container 
                    {
                        var elastic = scope.ServiceProvider
                            .GetRequiredService<IElasticService<Job>>();  // ask DI container give me elastic service for job 

                        await elastic.IndexAsync(job); // sends data to elastic search 
                    }

                    consumer.Commit(result); // tells kafka successfully processed this message
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ERROR:");

                    if (ex.InnerException != null)
                        Console.WriteLine(ex.InnerException.Message);

                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}