using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Validation
{
    public class DogValidator: IValidator<DogModel>
    {
        protected IUnitOfWork unitOfWork;
        public DogValidator(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void SetUnitOfWork(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<ValidationResult> Validate(DogModel model)
        {
            var result = new ValidationResult()
            {
                IsValid = true,
                Messages = new List<string>()
            };
            var dogs = await unitOfWork.DogRepository.GetAllAsync();
            if (string.IsNullOrEmpty(model.Name)) 
            {
                result.IsValid = false;
                result.Messages.Add("Name is required!");
            }
            else if (dogs.Any(x => x.Name.Equals(model.Name)))
            {
                result.IsValid = false;
                result.Messages.Add("Dog with this name already exists!");
            }

            if (string.IsNullOrEmpty(model.Color)) 
            {
                result.IsValid = false;
                result.Messages.Add("Color is required!");
            }
            if(model.TailLength < 1) 
            {
                result.IsValid = false;
                result.Messages.Add("Tail length must be greater than 0!");
            }
            if(model.Weight < 1) 
            {
                result.IsValid = false;
                result.Messages.Add("Weight must be greater than 0!");
            }
            return result;
        }
    }
}
