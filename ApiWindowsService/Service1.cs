namespace ApiWindowsService
{
    using MainApi;
    using Newtonsoft.Json;
    using System;
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

                var logPath = $"{path}\\api.txt";

                using (var sw = new StreamWriter(logPath, true))
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
                var logErrPath = $"{path}\\api_error.txt";

                using (var sw = new StreamWriter(logErrPath, true))
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
