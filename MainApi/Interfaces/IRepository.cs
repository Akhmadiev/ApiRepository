namespace MainApi.Interfaces
{
    using ApiAdditional;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public interface IRepository
    {
        ApiContext ApiContext { get; }

        void Save<T>(T entity) where T : Entity;

        void Save<T>(IEnumerable<T> entities) where T : Entity;

        void Update<T>(T entity) where T : Entity;

        IQueryable<T> GetAll<T>() where T : Entity;

        T Get<T>(Guid id) where T : Entity;
    }
}
