using Shop.Web.DTOs;
using Shop.Web.Infratructures.Request;
using Shop.Web.Infratructures.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Services.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetAllAsync();
        Task<ProductDTO> GetByIdAsync(Guid id);
        Task AddAsync(ProductDTO productDto);
        Task UpdateAsync(ProductDTO productDto);
        Task DeleteAsync(Guid id);

        Task<SearchResponse<ProductDTO>> SearchProductsAsync(List<Filter> filters, SortByInfo sortBy, int pageNumber, int pageSize);
    }
}
