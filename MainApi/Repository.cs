namespace MainApi
{
    using ApiAdditional;
    using MainApi.Interfaces;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Reflection;

    public class Repository : IRepository
    {
        public IQueryable<Country> GetAll()
        {
            var context = new ApiContext();
            return context.Countries;
        }

        public IEnumerator<Entity> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public void Save(Country entity)
        {
            using (var context = new ApiContext())
            {
                context.Countries.Add(entity);

                context.SaveChanges();
            }
        }

        public void Save(IEnumerable<Country> entities)
        {
            using (var context = new ApiContext())
            {
                foreach (var entity in entities)
                {
                    context.Countries.Add(entity);
                }

                context.SaveChanges();
            }
        }
    }
}
