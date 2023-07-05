// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScrapperAuchan.cs" company="The Virtual Forge">
// Copyright (c) 2023 All Rights Reserved
// </copyright>
// <summary>
// 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Searcher.Scrappers.Implementations
{
    using System.Text.Json;
    using System.Web;

    using AngleSharp;
    using AngleSharp.Html.Dom;

    using PuppeteerSharp;

    using Product = Seacher.Domain.Models.Product;

    public class ScrapperAuchan : BaseScrapper, IScrapper
    {
        private readonly string searchUrl = "https://www.auchan.pt/pt/pesquisa?q={0}&search-button=&lang=pt_PT";

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            var productsData = new List<Product>();
            var keyword = HttpUtility.UrlEncode(searchTerm);
            var url = string.Format(searchUrl, keyword);

            var document = await this.GetDocument(url);

            var products = document.QuerySelectorAll("*")
                .Where(e => e is { LocalName: "div", ClassName: "product" })
                .ToList();

            products.ForEach(
               async product =>
                    {

                        var elementData = product.QuerySelectorAll("*").FirstOrDefault(s => s is {LocalName: "div", ClassName: not null} and IHtmlDivElement && s.ClassName.Contains("product-tile"));

                        if (elementData is null)
                            return;


                        var elementDataJson = elementData.Attributes["data-gtm"]?.Value;


                        if (elementDataJson is null)
                            return;

                       

                        var data =  JsonSerializer.Deserialize<ProductData>(elementDataJson, new JsonSerializerOptions(JsonSerializerDefaults.Web));

                        if (data is null) return;

                        
                        var productData = new Product() { Brand = data.Brand, Name = data.Name, Description = data.Category, Value = data.Price};


                        productsData.Add(productData);

                    });

            return productsData;
            
        }

        public Task<IEnumerable<Product>> ScrapByCategory(string category)
        {
            throw new NotImplementedException();
        }

        private class ProductData
        {

            public string? Name { get; set; }

            public string? Brand { get; set; }

            public string Category { get; set; }

            public decimal? Price { get; set; }
        //    public string Brand { get; set; }
        //    private string Name { get; set; }
        }
    }
}