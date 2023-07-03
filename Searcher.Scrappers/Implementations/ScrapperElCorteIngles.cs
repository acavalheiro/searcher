namespace Searcher.Scrappers.Implementations
{
    using System.Data.Common;
    using System.Text.Json;
    using System.Web;

    using AngleSharp;
    using AngleSharp.Html.Dom;

    using PuppeteerSharp;

    using Product = Seacher.Domain.Models.Product;

    public class ScrapperElCorteIngles : BaseScrapper , IScrapper
    {

        private const string searchUrl = "https://www.elcorteingles.pt/supermercado/pesquisar/?term={0}&search=text";

        public ScrapperElCorteIngles()
            : base(false)
        {

        }
        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {

            var productsData = new List<Product>();
            
            var keyword = HttpUtility.UrlEncode(searchTerm);
            var url = string.Format(searchUrl, keyword);

            var document = await this.GetDocument(url);
            
            var products = document.QuerySelectorAll("*")
                .Where(e => e is { LocalName: "div", ClassName: not null } and IHtmlDivElement && e.ClassName.Contains("grid-item") && e.ClassName.Contains("js-product"))
                .ToList();

            products.ForEach(
                product =>
                    {
                        var elementDataJson = product.Attributes["data-json"]?.Value;

                        if (elementDataJson is null)
                            return;

                        var data = JsonSerializer.Deserialize<ProductData>(elementDataJson, new JsonSerializerOptions(JsonSerializerDefaults.Web));

                        if (data is null) return;

                        //var elementDataName = product.QuerySelectorAll("*")
                        //    .FirstOrDefault(s => s is {LocalName: "a", ClassName: not null} and IHtmlAnchorElement && s.ClassName.Contains("pwc-tile--description")) as IHtmlAnchorElement;
                        //if (elementDataName is null)
                        //    return;


                        //var elementData = product.QuerySelectorAll("*").FirstOrDefault(s => s is { LocalName: "div", ClassName: not null } and IHtmlDivElement && s.ClassName.Contains("product-tile"));

                        //if (elementData is null)
                        //    return;


                       






                        var productData = new Product() { Brand = data.Brand, Name = data.Name, Description = data.Category.FirstOrDefault(), Value = data.Price?.Final };


                        productsData.Add(productData);

                    });


            
            return productsData.Where(x => x.Name is not null).ToList();
        }

        private record ProductData
        {

            public string? Name { get; set; }

            public string? Brand { get; set; }

            public string[] Category { get; set; }

            public Price? Price { get; set; }
            //    public string Brand { get; set; }
            //    private string Name { get; set; }
        }

        private record Price(string Currency, decimal? Final)
        {

        }
    }




}
