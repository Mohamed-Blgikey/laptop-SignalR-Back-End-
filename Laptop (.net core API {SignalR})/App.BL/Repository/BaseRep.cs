using App.BL.Interface;
using App.DAL.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.BL.Repository
{
    public class BaseRep<T> : IBaseRep<T> where T : class
    {
        private readonly AppDbContext context;

        public BaseRep(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<T> Add(T item)
        {
            await context.Set<T>().AddRangeAsync(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<T> Edit(T item)
        {
            context.Set<T>().Update(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<T> Delete(T item)
        {
            context.Set<T>().Remove(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await context.Set<T>().FindAsync(id);
        }

    }
}
