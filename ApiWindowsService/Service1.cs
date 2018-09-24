namespace ApiWindowsService
{
    using MainApi;
    using MainApi.Interfaces;
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
            var main = new MainApiClass();
            main.Start();
        }

        protected override void OnStop()
        {
        }
    }
}
