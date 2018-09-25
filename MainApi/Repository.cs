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
            using (var context = new ApiContext())
            {
                foreach (var entity in entities)
                {
                    context.Set<T>().Add(entity);
                }

                context.SaveChanges();
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

        public void Update<T>(T entity) where T : Entity
        {
            using (var context = new ApiContext())
            {
                var updateEntity = context.Set<T>().Where(x => x.Id == entity.Id).First();

                var outProperties = updateEntity.GetType().GetProperties();

                var inProperties = entity.GetType().GetProperties();

                foreach (var outProperty in outProperties)
                {
                    var inProperty = inProperties.First(x => x.Name == outProperty.Name);

                    var outValue = outProperty.GetValue(updateEntity).ToString();
                    var inValue = inProperty.GetValue(entity).ToString();

                    if (outValue != inValue)
                    {
                        outProperty.SetValue(updateEntity, inProperty.GetValue(entity));
                    }
                }

                context.SaveChanges();
            }
        }

        public T Get<T>(int id) where T : Entity
        {
            return ApiContext.Set<T>().Where(x => x.Id == id).FirstOrDefault();
        }
    }
}
