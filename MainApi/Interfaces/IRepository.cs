namespace MainApi.Interfaces
{
    using ApiAdditional;
    using System.Collections.Generic;
    using System.Linq;

    public interface IRepository
    {
        void Save(Country entity);

        void Save(IEnumerable<Country> entities);

        IQueryable<Country> GetAll();
    }
}
