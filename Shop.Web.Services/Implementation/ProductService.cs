﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shop.Web.DTOs;
using Shop.Web.Infratructures.Request;
using Shop.Web.Infratructures.Response;
using Shop.Web.Models.Entity;
using Shop.Web.Repositories.Implementation;
using Shop.Web.Repositories.Interface;
using Shop.Web.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = await _productRepository.GetAll().ToListAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task AddAsync(ProductDTO productDto)
        {
            var product = _mapper.Map<Product>(productDto);
            product.Id = Guid.NewGuid();
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateAsync(ProductDTO productDto)
        {
            var product = await _productRepository.GetByIdAsync(productDto.Id);
            if (product == null)
            {
                throw new Exception($"Product Id does not found");
            }

            _mapper.Map(product, productDto);
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new Exception($"Product Id does not found");
            }

            await _productRepository.DeleteAsync(id);
        }

        public async Task<SearchResponse<ProductDTO>> SearchProductsAsync(List<Filter> filters, SortByInfo sortBy, int pageNumber, int pageSize)
        {
            return await _productRepository.SearchAsync(filters, sortBy, pageNumber, pageSize);
        }
    }
}
