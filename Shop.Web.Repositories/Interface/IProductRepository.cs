using Shop.Web.DTOs;
using Shop.Web.Infratructures.Request;
using Shop.Web.Infratructures.Response;
using Shop.Web.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Web.Repositories.Interface
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IQueryable<Product>> SortByAsync(List<SortByInfo> sorts, IQueryable<Product> query);
        Task<IQueryable<Product>> FilterAsync(IQueryable<Product> query, List<Filter> filters);
        Task<SearchResponse<ProductDTO>> SearchAsync(List<Filter> filters, SortByInfo sortBy, int pageNumber, int pageSize);

        IQueryable<Product> GetAllIncludingCategory();
        Task<Product> GetByIdIncludingCategoryAsync(Guid id);
    }
}
