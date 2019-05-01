using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;

        public int PageSize = 4;

        public ViewResult List(string category, int productPage = 1)
        {
            ProductsListViewModel productsListViewModel = new ProductsListViewModel
            {
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null
                        ? repository.Products.Count()
                        : repository.Products.Count(e => e.Category == category)
                },

                CurrentCategory = category
            };

            if (category == null)
                productsListViewModel.Products = repository.Products
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize);
            else
                productsListViewModel.Products = repository.Products
                    .Where(p => p.Category == category)
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize);


            return View(productsListViewModel);
        }

        public ProductController(IProductRepository repo) => repository = repo;


        //public ViewResult List() => View(repository.Products);
    }
}