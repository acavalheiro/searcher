namespace Searcher.Scrappers.Implementations
{
    using System.Text.Json;
    using System.Web;

    using AngleSharp;
    using AngleSharp.Html.Dom;

    using PuppeteerSharp;

    using Product = Seacher.Domain.Models.Product;

    public class ScrapperContinente : IScrapper
    {
        public async Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm)
        {

            var productsData = new List<Product>();
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            });
            var page = await browser.NewPageAsync();
            var keyword = HttpUtility.UrlEncode(searchTerm);
            await page.GoToAsync($"https://www.continente.pt/pesquisa/?q={keyword}");

            var content = await page.GetContentAsync();

            var context = BrowsingContext.New(Configuration.Default);
            var document = await context.OpenAsync(req => req.Content(content));

            var products = document.QuerySelectorAll("*")
                .Where(e => e is { LocalName: "div", ClassName: "product" })
                .ToList();

            products.ForEach(
                product =>
                    {



                        var elementDataName = product.QuerySelectorAll("*")
                            .FirstOrDefault(s => s is {LocalName: "a", ClassName: not null} and IHtmlAnchorElement && s.ClassName.Contains("pwc-tile--description")) as IHtmlAnchorElement;
                        if (elementDataName is null)
                            return;


                        var elementData = product.QuerySelectorAll("*").FirstOrDefault(s => s is { LocalName: "div", ClassName: not null } and IHtmlDivElement && s.ClassName.Contains("product-tile"));

                        if (elementData is null)
                            return;


                        var elementDataJson = elementData.Attributes["data-product-tile-impression"]?.Value;


                        if (elementDataJson is null)
                            return;



                        var data = JsonSerializer.Deserialize<ProductData>(elementDataJson, new JsonSerializerOptions(JsonSerializerDefaults.Web));

                        if (data is null) return;


                        var productData = new Product() { Brand = data.Brand, Name = elementDataName.TextContent, Description = data.Category, Value = data.Price };


                        productsData.Add(productData);

                    });



            return productsData.Where(x => x.Name is not null).ToList();
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
