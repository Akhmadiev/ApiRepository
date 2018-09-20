namespace Api1
{
    using ApiAdditional;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class ApiClass : IPlugin
    {
        public string Name => "Api1";

        public async Task<List<T>> Do<T>() where T : class
        {
            Thread.Sleep(10000);
            return await ApiAdditionalClass.GetData<T>("https://api.myjson.com/bins/1ahk3g");
        }
    }
}