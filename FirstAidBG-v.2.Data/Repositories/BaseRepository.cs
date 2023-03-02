using FirstAidBG_v._2.Data.Entities;
using FirstAidBG_v._2.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FirstAidBG_v._2.Data.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext context;

        public BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public virtual async Task<ICollection<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public virtual async Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await context.Set<T>().Where(filter).ToListAsync();
        }

        public virtual async ValueTask<T> GetByIdAsync(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

        public virtual async Task CreateAsync(T entity)
        {
            context.Set<T>().Add(entity);
            await context.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(T entity)
        {
            var dbEntity = await GetByIdAsync(entity.Id);

            if (dbEntity == null)
            {
                throw new ArgumentException($"No such {typeof(T)} with id: {entity.Id}");
            }

            context.Entry(dbEntity).CurrentValues.SetValues(entity);

            await context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);

            if (entity == null)
            {
                throw new ArgumentException($"No such {typeof(T)} with id: {id}");
            }

            context.Remove(entity);

            await context.SaveChangesAsync();
        }
    }
}
