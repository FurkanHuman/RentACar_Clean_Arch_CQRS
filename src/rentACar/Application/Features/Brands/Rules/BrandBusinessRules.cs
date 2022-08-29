using Application.Services.Repositories;
using Core.CrossCuttingConcerns.Exceptions;
using Core.Persistence.Paging;
using Domain.Entities;

namespace Application.Features.Brands.Rules
{
    public class BrandBusinessRules
    {
        private readonly IBrandRepository _brandRepository;

        public BrandBusinessRules(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task BrandNameCanNotBeDublicatedWhenInserted(string name)
        {
            IPaginate<Brand> resut=await _brandRepository.GetListAsync(b=>b.Name==name);
            if (resut.Items.Any()) throw new BusinessException("Brand Name Exit.");
        }
    }
}
