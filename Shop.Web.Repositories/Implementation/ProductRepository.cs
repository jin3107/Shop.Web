using Microsoft.EntityFrameworkCore;
using Shop.Web.DTOs;
using Shop.Web.Infratructures.Request;
using Shop.Web.Infratructures.Response;
using Shop.Web.Models.Data;
using Shop.Web.Models.Entity;
using Shop.Web.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Repositories.Implementation
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        #region Search Request
        public async Task<SearchResponse<ProductDTO>> SearchAsync(List<Filter> filters, SortByInfo sortBy, int pageNumber, int pageSize)
        {
            // Thay đổi kiểu trả về của GetAll() để bao gồm Include
            var productQuery = GetAllIncludingCategory();

            productQuery = await FilterAsync(productQuery, filters);
            productQuery = await SortByAsync(new List<SortByInfo> { sortBy }, productQuery);

            var totalRows = await productQuery.CountAsync();

            var pagedQuery = productQuery.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var products = await pagedQuery.ToListAsync();

            var result = products.Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                CategoryName = product.Category!.Name,
                Quantity = product.Quantity,
                Stock = product.Stock,
                CategoryId = product.CategoryId
            }).ToList();

            var totalPages = (long)Math.Ceiling((double)totalRows / pageSize);

            var response = new SearchResponse<ProductDTO>
            {
                Data = result,
                CurrentPage = pageNumber,
                TotalPages = totalPages,
                RowsPerPage = pageSize,
                TotalRows = totalRows
            };

            return response;
        }

        public IQueryable<Product> GetAllIncludingCategory()
        {
            return GetAll().Include(p => p.Category);
        }

        public async Task<Product> GetByIdIncludingCategoryAsync(Guid id)
        {
            return await GetAllIncludingCategory().FirstOrDefaultAsync(p => p.Id == id);
        }
        #endregion

        #region Filter
        public Task<IQueryable<Product>> FilterAsync(IQueryable<Product> query, List<Filter> filters)
        {
            if (filters != null && filters.Any())
            {
                foreach (var filter in filters)
                {
                    if (string.IsNullOrEmpty(filter.FieldName) || string.IsNullOrEmpty(filter.Value) || string.IsNullOrEmpty(filter.Operation))
                        continue;

                    switch (filter.FieldName.ToLower())
                    {
                        case "name":
                            switch (filter.Operation.ToLower())
                            {
                                case "equals":
                                    query = query.Where(x => x.Name!.Equals(filter.Value, StringComparison.OrdinalIgnoreCase));
                                    break;
                                case "contains":
                                    query = query.Where(x => x.Name!.Contains(filter.Value));
                                    break;
                            }
                            break;

                        case "Price":
                            if (decimal.TryParse(filter.Value, out decimal priceValue))
                            {
                                switch (filter.Operation.ToLower())
                                {
                                    case "equals":
                                        query = query.Where(x => x.Price == priceValue);
                                        break;
                                    case "greaterthan":
                                        query = query.Where(x => x.Price > priceValue);
                                        break;
                                    case "lessthan":
                                        query = query.Where(x => x.Price < priceValue);
                                        break;
                                }
                            }
                            break;

                        case "Categoryname":
                            switch (filter.Operation.ToLower())
                            {
                                case "equals":
                                    query = query.Where(x => x.Category!.Name!.Equals(filter.Value, StringComparison.OrdinalIgnoreCase));
                                    break;
                                case "contains":
                                    query = query.Where(x => x.Category!.Name!.Contains(filter.Value));
                                    break;
                            }
                            break;
                    }
                }
            }
            return Task.FromResult(query);
        }
        #endregion

        #region Sort
        public Task<IQueryable<Product>> SortByAsync(List<SortByInfo> sorts, IQueryable<Product> query)
        {
            if (sorts != null && sorts.Any())
            {
                foreach (var item in sorts)
                {
                    if (string.IsNullOrEmpty(item.FieldName))
                        continue;

                    if (item.Accending.HasValue)
                    {
                        if (item.Accending.Value)
                        {
                            query = query.OrderBy(x => EF.Property<object>(x, item.FieldName));
                        }
                        else
                        {
                            query = query.OrderByDescending(x => EF.Property<object>(x, item.FieldName));
                        }
                    }
                }
            }
            return Task.FromResult(query);
        }
        #endregion
    }
}
