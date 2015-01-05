﻿using StructureMap;
using StructureMap.Pipeline;
using Stylet;
using System;

namespace Bootstrappers
{
    public class StructureMapBootstrapper<TRootViewModel> : BootstrapperBase<TRootViewModel> where TRootViewModel : class
    {
        private IContainer container;

        protected override void ConfigureBootstrapper()
        {
            this.Configure();

            this.container = new Container(config =>
            {
                this.DefaultConfigureIoC(config);
                this.ConfigureIoC(config);
            });
        }

        /// <summary>
        /// Override to configure anything that needs configuring
        /// </summary>
        protected virtual void Configure() { }

        /// <summary>
        /// Carries out default configuration of the IoC container. Override if you don't want to do this
        /// </summary>
        protected virtual void DefaultConfigureIoC(ConfigurationExpression config)
        {
            config.For<IViewManagerConfig>().Add(this);
            config.For<IViewManager>().Add<ViewManager>().LifecycleIs<SingletonLifecycle>();
            config.For<IWindowManager>().Add<WindowManager>().LifecycleIs<SingletonLifecycle>();
            config.For<IEventAggregator>().Add<EventAggregator>().LifecycleIs<SingletonLifecycle>();
            config.For<IMessageBoxViewModel>().Add<MessageBoxViewModel>().LifecycleIs<UniquePerRequestLifecycle>();
            foreach (var assembly in this.Assemblies)
            {
                config.Scan(x => x.Assembly(assembly));
            }
        }

        /// <summary>
        /// Override to add your own types to the IoC container.
        /// </summary>
        protected virtual void ConfigureIoC(ConfigurationExpression config) { }

        public override object GetInstance(Type type)
        {
            return this.container.GetInstance(type);
        }
    }
}
