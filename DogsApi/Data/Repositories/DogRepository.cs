using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class DogRepository : IRepository<Dog>
    {
        private readonly DogAPIDBContext Context;

        public DogRepository(DogAPIDBContext context)
        {
            Context = context;
        }

        public async Task AddAsync(Dog entity)
        {
            await Context.Dogs.AddAsync(entity);

            Context.SaveChanges();
        }

        public async Task<IEnumerable<Dog>> GetAllAsync()
        {
            return await Context.Dogs.ToListAsync();
        }
    }
}
