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
        public IEnumerable<Country> GetAll(Expression<Func<Country, bool>> predicate = null)
        {
            using (var context = new ApiContext())
            {
                return context.Countries
                    .Where(predicate)
                    .ToList();
            }
        }

        public void Save<T>(IEnumerable<T> entities) where T : Entity
        {
            using (var context = new ApiContext())
            {
                foreach (var entity in entities)
                {
                    context.Set<T>().Add(entity);

                    context.SaveChanges();
                }
            }
        }

        public void Save<T>(T entity) where T : Entity
        {
            using (var context = new ApiContext())
            {
                context.Set<T>().Add(entity);

                context.SaveChanges();
            }
        }
    }
}
