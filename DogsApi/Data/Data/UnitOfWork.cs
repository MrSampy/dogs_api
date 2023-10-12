using Data.Entities;
using Data.Interfaces;
using Data.Repositories;

namespace Data.Data
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly DogAPIDBContext dbContext;

        private DogRepository _dogRepository;
        public IRepository<Dog> DogRepository => _dogRepository ??= new DogRepository(dbContext);
        public UnitOfWork(DogAPIDBContext dbContext)
        {
            this.dbContext = dbContext;
        }

    }
}
