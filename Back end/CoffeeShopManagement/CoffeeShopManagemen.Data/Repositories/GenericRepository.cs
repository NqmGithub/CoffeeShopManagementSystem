using CoffeeShopManagement.Data.RepositoryContracts;
using CoffeeShopManagement.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly CoffeeShopDbContext _db;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(CoffeeShopDbContext db)
        {
            _db = db;
            _dbSet = db.Set<T>();
        }
        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void AddRangeAsync(List<T> list)
        {
            _dbSet.AddRangeAsync(list);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public virtual void Delete(Guid id)
        {
            var entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
        }

        public IQueryable<T> Get(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _dbSet;
            if (filter != null)
            {
                query.Where(filter);
            }
            if (string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            if (orderBy != null)
            {
                orderBy(query);
            }
            return query;
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public T? GetById(Guid id)
        {
            return _dbSet.Find(id);
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetPagination(int pageNumber, int pageSize)
        {
            return await _dbSet.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToListAsync();
        }

        public IQueryable<T> GetQuery()
        {
            return _dbSet;
        }

        public IQueryable<T> GetQuery(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }
    }
}
