using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Data
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly DogAPIDBContext dbContext;

        private DogRepository dogRepository;
        public IRepository<Dog> DogRepository => dogRepository ??= new DogRepository(dbContext);

        public UnitOfWork(DogAPIDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

    }
}
