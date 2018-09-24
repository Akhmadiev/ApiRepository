namespace MainApi.Interfaces
{
    using ApiAdditional;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRepository
    {
        void Save<T>(T entity) where T : Entity;

        void Save<T>(IEnumerable<T> entities) where T : Entity;

        IEnumerable<Country> GetAll(Expression<Func<Country, bool>> predicate = null);
    }
}
