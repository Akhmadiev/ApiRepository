namespace Api3
{
    using ApiAdditional;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ApiClass : IPlugin
    {
        public string Name => "Api3";

        public async Task<List<ApiAdditional.Country>> Do()
        {
            var data = await ApiAdditionalClass.GetData<Country>("https://api.myjson.com/bins/pta44");

            var result = new List<ApiAdditional.Country>(data.Count);

            foreach (var row in data)
            {
                result.Add(new ApiAdditional.Country
                {
                    CapitalCity = row.CapitalName,
                    ContinentType = (ContinentType)row.ContinentType,
                    Name = row.CountryName,
                    StartDate = DateTime.Now
                });
            }

            return result;
        }
    }
}
