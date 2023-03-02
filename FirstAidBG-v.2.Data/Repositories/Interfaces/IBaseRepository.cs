using FirstAidBG_v._2.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FirstAidBG_v._2.Data.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task CreateAsync(T entity);
        Task DeleteAsync(int id);
        Task<ICollection<T>> GetAllAsync();
        Task<ICollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        ValueTask<T> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
    }
}
