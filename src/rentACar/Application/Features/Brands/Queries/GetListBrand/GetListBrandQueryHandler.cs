using Application.Features.Brands.Models;
using Application.Services.Repositories;
using AutoMapper;
using Core.Persistence.Paging;
using Domain.Entities;
using MediatR;

namespace Application.Features.Brands.Queries.GetListBrand
{
    public class GetListBrandQueryHandler : IRequestHandler<GetListBrandQuery, BrandListModel>
    {
        IBrandRepository _brandRepository;
        IMapper _mapper;

        public GetListBrandQueryHandler(IBrandRepository brandRepository, IMapper mapper)
        {
            _brandRepository = brandRepository;
            _mapper = mapper;
        }

        public async Task<BrandListModel> Handle(GetListBrandQuery request, CancellationToken cancellationToken)
        {
            IPaginate<Brand> paginate  =  await _brandRepository.GetListAsync(index: request.PageRequest.Page, size: request.PageRequest.PageSize);

            BrandListModel mappedBrandListModel = _mapper.Map<BrandListModel>(paginate);

            return mappedBrandListModel;
        }
    }
}

