namespace Searcher.Scrappers.Implementations
{
    using System.Data.Common;
    using System.Text.Json;
    using System.Web;

    using AngleSharp;
    using AngleSharp.Html.Dom;

    using PuppeteerSharp;

    using Product = Seacher.Domain.Models.Product;

    public class ScrapperElCorteIngles : IScrapper
    {
        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {

            var productsData = new List<Product>();
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false
            });
            var page = await browser.NewPageAsync();
            var keyword = HttpUtility.UrlEncode(searchTerm);
            await page.GoToAsync($"https://www.elcorteingles.pt/supermercado/pesquisar/?term={keyword}&search=text");

            var content = await page.GetContentAsync();

            var context = BrowsingContext.New(Configuration.Default);
            var document = await context.OpenAsync(req => req.Content(content));

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


            await browser.CloseAsync();
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
