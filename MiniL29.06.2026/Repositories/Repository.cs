using Microsoft.EntityFrameworkCore;
using MiniL29._06._2026.Data;
using MiniL29._06._2026.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniL29._06._2026.Entities.Base;

namespace MiniL29._06._2026.Repositories
{
    internal class Repository<T> where T : BaseEntity, new()
    {
        private readonly AllDbSystem _context;
        private readonly DbSet<T> _table;
        public Repository(AllDbSystem context)
        {
            _context = context;
            _table = _context.Set<T>();
        }
        public void Add(T entity)
        { 
            _table.Add(entity);
        }
        public List<T> GetAll()
        {
            return _table.AsNoTracking().ToList();
        }
        public T? GetById(int Id, bool isTracking = false) 
        {
            if (isTracking) 
            {
                return _table.FirstOrDefault(e => e.Id == Id);
            }
            return _table.AsNoTracking().FirstOrDefault(e => e.Id == Id);
        }
        public void Update(T entity) 
        {
            _table.Update(entity);
        }
        public void Delete(T entity) 
        {
            _table.Remove(entity);
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}

