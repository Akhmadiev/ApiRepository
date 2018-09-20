namespace Api2
{
    using ApiAdditional;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ApiClass : IPlugin
    {
        public string Name => "Api2";

        public async Task<List<T>> Do<T>() where T : class
        {
            return await ApiAdditionalClass.GetData<T>("https://api.myjson.com/bins/1ambe4");
        }
    }
}
