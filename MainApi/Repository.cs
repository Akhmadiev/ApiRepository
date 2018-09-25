namespace MainApi
{
    using ApiAdditional;
    using MainApi.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class Repository : IRepository
    {
        public ApiContext ApiContext { get => new ApiContext(); }

        public IQueryable<T> GetAll<T>() where T : Entity
        {
            return ApiContext.Set<T>();
        }

        public void Save<T>(IEnumerable<T> entities) where T : Entity
        {
            foreach (var entity in entities)
            {
                ApiContext.Set<T>().Add(entity);
            }

            ApiContext.SaveChanges();
        }

        public void Save<T>(T entity) where T : Entity
        {
            ApiContext.Set<T>().Add(entity);

            ApiContext.SaveChanges();
        }

        public void Update<T>(T entity) where T : Entity
        {
            ApiContext.SaveChanges();
        }
    }
}
