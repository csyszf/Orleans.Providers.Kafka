﻿using Microsoft.Extensions.DependencyInjection;
using Orleans.Configuration;
using Orleans.Hosting;
using Orleans.Providers.Kafka.Streams;
using Orleans.Streams;
using System;
using System.Collections.Generic;
using System.Text;

namespace Orleans.Providers.Kafka.Streams
{
    public class SiloKafkaStreamConfigurator : SiloRecoverableStreamConfigurator
    {
        public SiloKafkaStreamConfigurator(string name, ISiloHostBuilder builder)
            : base(name, builder, KafkaAdapterFactory.Create)
        {
            this.siloBuilder.ConfigureApplicationParts(parts => parts.AddFrameworkPart(typeof(KafkaAdapterFactory).Assembly))
                .ConfigureServices(services => services.ConfigureNamedOptionForLogging<KafkaOptions>(name)
                    .ConfigureNamedOptionForLogging<KafkaReceiverOptions>(name)
                    .ConfigureNamedOptionForLogging<KafkaStreamCachePressureOptions>(name)
                    .AddTransient<IConfigurationValidator>(sp => new KafkaOptionsValidator(sp.GetOptionsByName<KafkaOptions>(name), name))
                    .AddTransient<IConfigurationValidator>(sp => new KafkaRecieverOptionsValidator(sp.GetOptionsByName<KafkaReceiverOptions>(name), name))
                );
        }


        public SiloKafkaStreamConfigurator ConfigureKafka(Action<OptionsBuilder<KafkaOptions>> configureOptions)
        {
            this.Configure<KafkaOptions>(configureOptions);
            return this;
        }

        public SiloKafkaStreamConfigurator ConfigureReceiver(Action<OptionsBuilder<KafkaReceiverOptions>> configureOptions)
        {
            this.Configure<KafkaReceiverOptions>(configureOptions);
            return this;
        }

        public SiloKafkaStreamConfigurator ConfigureCachePressuring(Action<OptionsBuilder<KafkaStreamCachePressureOptions>> configureOptions)
        {
            this.Configure<KafkaStreamCachePressureOptions>(configureOptions);
            return this;
        }
    }

    //public class ClusterClientEventHubStreamConfigurator : ClusterClientPersistentStreamConfigurator
    //{
    //    public ClusterClientEventHubStreamConfigurator(string name, IClientBuilder builder)
    //       : base(name, builder, EventHubAdapterFactory.Create)
    //    {
    //        this.clientBuilder.ConfigureApplicationParts(parts => parts.AddFrameworkPart(typeof(EventHubAdapterFactory).Assembly))
    //            .ConfigureServices(services => services.ConfigureNamedOptionForLogging<EventHubOptions>(name)
    //            .AddTransient<IConfigurationValidator>(sp => new KafkaRecieverOptionsValidator(sp.GetOptionsByName<EventHubOptions>(name), name)));
    //    }

    //    public ClusterClientEventHubStreamConfigurator ConfigureEventHub(Action<OptionsBuilder<EventHubOptions>> configureOptions)
    //    {
    //        this.Configure<EventHubOptions>(configureOptions);
    //        return this;
    //    }
    //}
}