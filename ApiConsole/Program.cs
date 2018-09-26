namespace ApiConsole
{
    using ApiAdditional;
    using Castle.MicroKernel.Resolvers.SpecializedResolvers;
    using Castle.Windsor;
    using MainApi;
    using MainApi.Entities;
    using MainApi.Interfaces;
    using MainApi.Reports;
    using Newtonsoft.Json;
    using Quartz;
    using Quartz.Impl;
    using System;
    using System.Linq;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            var container = new WindsorContainer();
            container.Kernel.Resolver.AddSubResolver(new CollectionResolver(container.Kernel));

            var main = new MainApiClass();
            main.Start(container);

            var report = container.Resolve<MainApi.Reports.Report>();
            report.Start();

            while (true)
            {
                Console.WriteLine("Generate report - 1");
                Console.WriteLine("Get report - 2");
                var input = Console.ReadLine();

                if (input == "1")
                {
                    Console.WriteLine("Choose report:");
                    var names = typeof(ReportIds).GetFields().Select(x => (string)x.GetValue(null)).ToList();
                    foreach (var name in names)
                    {
                        Console.WriteLine(name);
                    }

                    input = Console.ReadLine();

                    var reportId = names.First(x => x.Contains(input));
                    var id = report.Generate(reportId);

                    Console.WriteLine($"Your ID: '{id}'. The report will be ready in 2 minutes");
                }
                else if (input == "2")
                {
                    Console.WriteLine("Write ID:");
                    var id = Convert.ToInt32(Console.ReadLine());

                    var result = report.GetReport(id);

                    Console.WriteLine();
                    Console.WriteLine(result);
                }

                Console.WriteLine();
                Console.WriteLine();
            }
        }
    }
}