namespace ApiWindowsService
{
    using MainApi;
    using Newtonsoft.Json;
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.ServiceProcess;

    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var path = $"{new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName}\\logs";

            try
            {
                var main = new MainApiClass();
                main.Start();

                var data = main.GetAll().ToList();

                var logPath = $"{path}\\api_{DateTime.Now.ToString("dd.MM.yyyy hh.mm.ss")}.txt";

                using (var sw = File.CreateText(path))
                {
                    sw.WriteLine($"Start date: {DateTime.Now}");

                    var json = JsonConvert.SerializeObject(data);
                    sw.WriteLine(json);

                    sw.WriteLine();
                    sw.WriteLine();
                }
            }
            catch(Exception e)
            {
                var errorPath = $"{path}\\api_error_{DateTime.Now.ToString("dd.MM.yyyy hh.mm.ss")}.txt";

                using (var sw = File.CreateText(errorPath))
                {
                    sw.WriteLine(e.StackTrace);
                    sw.WriteLine(e.Message);
                }
            }
        }

        protected override void OnStop()
        {
        }
    }
}
