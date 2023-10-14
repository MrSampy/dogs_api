using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class DogService : IDogService
    {
        public IUnitOfWork UnitOfWork;
        public IMapper Mapper;
        public ICacheService CacheService;
        public IValidator<DogModel> Validator;
        public DogService(IUnitOfWork unitOfWork, IMapper createMapperProfile, ICacheService cacheService, IValidator<DogModel> validator)
        {
            UnitOfWork = unitOfWork;
            Mapper = createMapperProfile;
            CacheService = cacheService;
            Validator = validator;
        }
        public async Task AddAsync(DogModel model)
        {
            Validator.SetUnitOfWork(UnitOfWork);
            var validationResult = await Validator.Validate(model);
            if (!validationResult.IsValid) 
            {
                throw new ValidationException("Validation failed: " + string.Join("\n", validationResult.Messages));
            }
            CacheService.Reset();
            await UnitOfWork.DogRepository.AddAsync(Mapper.Map<Data.Entities.Dog>(model));
        }

        public async Task<IEnumerable<DogModel>> GetAllAsync(FilterModel filterModel)
        {
            var cacheKey = "GetAllAsync" + filterModel.ToString();
            var cachedResult = CacheService.Get<IEnumerable<DogModel>>(cacheKey);
            if (cachedResult != null)
            {
                return cachedResult;
            }
            var result = Mapper.Map<IEnumerable<DogModel>>(await UnitOfWork.DogRepository.GetAllAsync());
            if (filterModel.PageNumber.HasValue && filterModel.PageNumber > 0 && filterModel.PageSize.HasValue && filterModel.PageSize > 0) 
            {
                result = ApplyPagination(result, filterModel.PageNumber.Value, filterModel.PageSize.Value);
            }
            if (!string.IsNullOrEmpty(filterModel.Attribute) && !string.IsNullOrEmpty(filterModel.Order))
            {
                result = Sort(result, filterModel.Attribute.ToLower(), filterModel.Order.ToLower());
            }
            CacheService.Set(cacheKey, result, TimeSpan.FromMinutes(10));
            return result;

        }

        private IEnumerable<DogModel> ApplyPagination(IEnumerable<DogModel> models, int pageNumber, int pageSize)
        {
            return models.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }

        private IEnumerable<DogModel> Sort(IEnumerable<DogModel> models, string attribute, string order)
        {
            var attributeToProperty = new Dictionary<string, Func<DogModel, object>>
            {
                ["name"] = x => x.Name,
                ["weight"] = x => x.Weight,
                ["color"] = x => x.Color,
                ["tail_length"] = x => x.TailLength,
            };

            if (attributeToProperty.TryGetValue(attribute, out var propertySelector))
            {
                if (order == "asc") 
                {
                    return models.OrderBy(propertySelector);
                }
                else if (order == "desc")
                {
                    return models.OrderByDescending(propertySelector);
                }
            }

            return models;
        }   

    }
}
