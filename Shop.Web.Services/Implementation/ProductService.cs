using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Shop.Web.DTOs;
using Shop.Web.Infratructures.Request;
using Shop.Web.Infratructures.Response;
using Shop.Web.Models.Entity;
using Shop.Web.Repositories.Interface;
using Shop.Web.Services.Interface;

namespace Shop.Web.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllAsync()
        {
            var products = await _productRepository.GetAllIncludingCategory().ToListAsync();
            return _mapper.Map<IEnumerable<ProductDTO>>(products);
        }

        public async Task<ProductDTO> GetByIdAsync(Guid id)
        {
            var product = await _productRepository.GetByIdIncludingCategoryAsync(id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task AddAsync(ProductDTO productDto)
        {
            var category = await _categoryRepository.GetByIdAsync(productDto.CategoryId);
            if (category == null)
            {
                throw new Exception("Invalid CategoryId.");
            }

            var product = _mapper.Map<Product>(productDto);
            product.ProductId = Guid.NewGuid();
            product.Category = null;
            await _productRepository.AddAsync(product);
        }

        public async Task UpdateAsync(ProductDTO productDto)
        {
            var product = await _productRepository.GetByIdAsync(productDto.ProductId);
            if (product == null)
            {
                throw new Exception("Product Id not found.");
            }

            var category = await _categoryRepository.GetByIdAsync(productDto.CategoryId);
            if (category == null)
            {
                throw new Exception("Invalid CategoryId.");
            }

            _mapper.Map(productDto, product);
            product.Category = null;
            await _productRepository.UpdateAsync(product);
        }

        public async Task DeleteAsync(Guid id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                throw new Exception("Product Id not found.");
            }

            await _productRepository.DeleteAsync(id);
        }

        public async Task<SearchResponse<ProductDTO>> SearchProductsAsync(List<Filter> filters, SortByInfo sortBy, int pageNumber, int pageSize)
        {
            return await _productRepository.SearchAsync(filters, sortBy, pageNumber, pageSize);
        }
    }
}
