// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ScrapperContinente.cs" company="The Virtual Forge">
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
    using AngleSharp.Dom;
    using AngleSharp.Html.Dom;

    using PuppeteerSharp;

    using Product = Seacher.Domain.Models.Product;

    public class ScrapperContinente : BaseScrapper, IScrapper
    {
        private const string searchUrl = "https://www.continente.pt/pesquisa/?q={0}";

        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {
            var productsData = new List<Product>();
            var keyword = HttpUtility.UrlEncode(searchTerm);
            var url = string.Format(searchUrl, keyword);

            var document = await this.GetDocument(url);

            var products = document.QuerySelectorAll("*").Where(e => e is {LocalName: "div", ClassName: "product"}).ToList();

            products.ForEach(
                productItem =>
                    {
                        var product = this.GetProduct(productItem);

                        if (product is not null)
                            productsData.Add(product);
                    });

            return productsData.Where(x => x.Name is not null).ToList();
        }

        public async Task<IEnumerable<Product>> ScrapByCategory(string category)
        {

            var urlBase = $"https://www.continente.pt/laticinios-e-ovos/?start=0&srule=FOOD-Laticinios&pmin=0.01";
            var productsData = new List<Product>();

            var document = await this.GetDocument(urlBase);

            var footer = document.QuerySelectorAll("*").FirstOrDefault(s => s is { LocalName: "div", ClassName: not null } and IHtmlDivElement && s.ClassName.Contains("grid-footer")) as IHtmlDivElement;

            if (footer is null)
                return productsData;

            var elementDataJson = footer.Attributes["data-total-count"]?.Value;

            if (elementDataJson is null)
                return productsData ;

            var totalProducts = int.Parse(elementDataJson);

            for (int i = 0; i < totalProducts; i+=100)
            {
               // var url = $"https://www.continente.pt/mercearia/?start=0&srule=FOOD_Mercearia&pmin=0.01&start={i}&sz=100";
                var url = $"{urlBase}&start={i}&sz=100";
                document = await this.GetDocument(url);

                var products = document.QuerySelectorAll("*").Where(e => e is { LocalName: "div", ClassName: "product" }).ToList();

                products.ForEach(
                    productItem =>
                        {

                            var product = this.GetProduct(productItem);

                            if (product is not null)
                                productsData.Add(product);
                        });
            }



            return productsData ;

        }

        private Product? GetProduct(IElement product)
        {
            var elementDataName = product.QuerySelectorAll("*")
                                      .FirstOrDefault(
                                          s => s is { LocalName: "a", ClassName: not null } and IHtmlAnchorElement && s.ClassName.Contains("pwc-tile--description")) as IHtmlAnchorElement;
            if (elementDataName is null)
                return null;

            var elementData = product.QuerySelectorAll("*").FirstOrDefault(s => s is { LocalName: "div", ClassName: not null } and IHtmlDivElement && s.ClassName.Contains("product-tile"));

            if (elementData is null)
                return null;

            var elementDataJson = elementData.Attributes["data-product-tile-impression"]?.Value;

            if (elementDataJson is null)
                return null;

            var data = JsonSerializer.Deserialize<ProductData>(elementDataJson, new JsonSerializerOptions(JsonSerializerDefaults.Web));

            if (data is null) return null;

            var elementDataSize = product.QuerySelectorAll("*")
                                      .FirstOrDefault(
                                          s => s is { LocalName: "p", ClassName: not null } and IHtmlParagraphElement && s.ClassName.Contains("pwc-tile--quantity")) as IHtmlParagraphElement;
            


            return new Product() { Brand = data.Brand, Name = elementDataName.TextContent, Description = data.Category, Value = data.Price, Id = data.Name, Url = elementDataName.Href, Size = elementDataSize?.TextContent};
        }

        private class ProductData
        {
            public string? Id { get; set; }

            public string? Brand { get; set; }

            public string Category { get; set; }

            public string? Name { get; set; }

            public decimal? Price { get; set; }

            // public string Brand { get; set; }
            // private string Name { get; set; }
        }
    }
}