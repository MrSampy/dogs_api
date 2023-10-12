using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Models;
using Data.Interfaces;

namespace Business.Interfaces
{
    public interface IValidator<TModel> where TModel : class
    {        
        public Task<ValidationResult> Validate(TModel model);
        public void SetUnitOfWork(IUnitOfWork unitOfWork);
    }
}
