namespace Api2
{
    using ApiAdditional;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ApiClass : IPlugin
    {
        public string Name => "Api2";

        public async Task<List<ApiAdditional.Country>> Do()
        {
            var data = await ApiAdditionalClass.GetData<Country>("https://api.myjson.com/bins/1ambe4");

            var result = new List<ApiAdditional.Country>(data.Count);

            foreach (var row in data)
            {
                result.Add(new ApiAdditional.Country
                {
                    CapitalCity = row.CapitalCity,
                    ContinentType = (ContinentType)row.ContinentType,
                    Name = row.Name,
                    StartDate = DateTime.Now
                });
            }

            return result;
        }
    }
}
