namespace MainApi
{
    using Castle.Windsor;
    using ApiAdditional;
    using System.IO;
    using System.Reflection;
    using Castle.MicroKernel.Registration;
    using MainApi.Interfaces;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;

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

                container.Register(Classes.From(types).BasedOn<IGenerateReport>().WithServiceBase());

                //foreach (var type in types.Where(x => !x.IsInterface && !x.IsAbstract))
                //{
                //    if (type.GetInterface(pluginType.FullName) != null)
                //    {
                //        var instance = (IPlugin)Activator.CreateInstance(type);
                //        container.Register(Component.For<IPlugin>().Instance(instance));
                //    }
                //}
            }
        }

        private void RegisterOthers(IWindsorContainer container)
        {
            container.Register(Component.For<IRepository>().ImplementedBy<Repository>());
            container.Register(Component.For<ILogger>().ImplementedBy<Logger>());
            container.Register(Component.For<Reports.Report>().ImplementedBy<Reports.Report>());
        }

        private void RegisterReports(IWindsorContainer container)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            container.Register(Classes.From(types).BasedOn<IGenerateReport>().WithServiceBase());
        }
    }
}