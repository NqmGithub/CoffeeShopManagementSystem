﻿using CoffeeShopManagement.Business.Helpers;
using CoffeeShopManagement.Data.UnitOfWork;
using CoffeeShopManagement.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeShopManagement.Business.DTO
{
    public class ProductDTO
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; } = null!;

        public string CategoryName { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }
        public string? Thumbnail { get; set; }
        public string Status { get; set; }

        public int? numberReviews { get; set; }
        public double? Rate { get; set; }
    }

    public static class ProductExtensions
    {
        public static ProductDTO ToProductDTO(this Product product, IUnitOfWork unitOfWork)
        {
            var categoryName = unitOfWork.ProductRepository
                .GetQuery()
                .Include(x => x.Categoty)
                .Where(x => product.CategotyId == x.Categoty.Id)
                .Select(x => x.Categoty.CategoryName)
                .FirstOrDefault();
            return new ProductDTO()
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CategoryName = categoryName,
                Price = product.Price,
                Quantity = product.Quantity,
                Thumbnail = product.Thumbnail,
                Description = product.Description,
                Status = ProductHelper.ConvertToStatusString (product.Status),
            };
        }
    }
}
