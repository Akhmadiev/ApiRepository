namespace MainApi
{
    using Quartz;
    using Quartz.Impl;
    using System.Linq;
    using Castle.Windsor;
    using System;
    using ApiAdditional;
    using System.IO;
    using System.Reflection;
    using Castle.MicroKernel.Registration;
    using MainApi.Interfaces;

    public class MainApiClass
    {
        public void Start(IWindsorContainer container)
        {
            RegisterPlugins(container);
            RegisterReports(container);
            RegisterOthers(container);
        }

        private void RegisterPlugins(IWindsorContainer container)
        {
            var files = Directory.GetFiles($"{new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName}\\dll");

            var pluginType = typeof(IPlugin);

            foreach (var file in files)
            {
                var assembly = Assembly.LoadFile(file);
                var types = assembly.GetTypes();

                foreach (var type in types.Where(x => !x.IsInterface && !x.IsAbstract))
                {
                    if (type.GetInterface(pluginType.FullName) != null)
                    {
                        var instance = (IPlugin)Activator.CreateInstance(type);
                        container.Register(Component.For<IPlugin>().Instance(instance));
                    }
                }
            }
        }

        private void RegisterOthers(IWindsorContainer container)
        {
            container.Register(Component.For<IRepository>().ImplementedBy<Repository>());
            container.Register(Component.For<ILogger>().ImplementedBy<Logger>());
        }

        private void RegisterReports(IWindsorContainer container)
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            var types = assembly.GetTypes();

            var reportType = typeof(IGenerateReport);

            foreach (var type in types)
            {
                if (type.GetInterface(reportType.FullName) != null)
                {
                    var instance = (IGenerateReport)Activator.CreateInstance(type);
                    container.Register(Component.For<IGenerateReport>().Instance(instance).Named(instance.ReportId));
                }
            }
        }
    }
}