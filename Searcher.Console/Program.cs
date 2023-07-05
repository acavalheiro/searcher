using System.Diagnostics.CodeAnalysis;
using System.Transactions;

using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using PuppeteerSharp;

using Seacher.Domain.Common.Interfaces;
using Seacher.Domain.Entities;

using Searcher.Application.Repositories;
using Searcher.Persistence.Contexts;
using Searcher.Persistence.Repositories;
using Searcher.Scrappers;
using Searcher.Scrappers.Implementations;

// See https://aka.ms/new-console-template for more information
Console.WriteLine("Search Product:");
var keyword = Console.ReadLine();


using IScrapper continente = new ScrapperContinente();
using IScrapper auchan = new ScrapperAuchan();
using IScrapper elcorteIngles = new ScrapperElCorteIngles();

using IDbContext dbContext = new ApplicationDbContext();

IProductScrapperRepository repo = new ProductScrapperRepository(dbContext);


//var dataAuchan =  auchan.SearchProductsAsync(keyword);
//var dataContinente =  continente.SearchProductsAsync(keyword);
//var dataElcorteingles =  elcorteIngles.SearchProductsAsync(keyword);

var dataContinente = await continente.ScrapByCategory("");

var products = dataContinente.ToList().Select(
    x => new ProductScrapped()
             {
                 Name = x.Name,
                 Brand = x.Brand,
                 Url = x.Url,
                 Value = x.Value,
                 Size = x.Size,
                 Description = x.Description,
                 InternalId = x.Id,
                 StoreId = 1
             });

await repo.AddRangeAsync(products);
await dbContext.SaveChangesAsync(new CancellationToken());


//Task.WaitAll(dataAuchan, dataContinente, dataElcorteingles);

foreach (var product in dataContinente)
{
    Console.WriteLine($"Store : Continente |    Name : {product.Name} ! Brand : {product.Brand} | Price : {product.Value}");
}

//foreach (var product in dataAuchan.Result)
//{
//    Console.WriteLine($"Store : Auchan |        Name : {product.Name} ! Brand : {product.Brand} | Price : {product.Value}");
//}

//foreach (var product in dataElcorteingles.Result)
//{
//    Console.WriteLine($"Store : ECI |        Name : {product.Name} ! Brand : {product.Brand} | Price : {product.Value}");
//}



//using var browserFetcher = new BrowserFetcher();
//await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
//var browser = await Puppeteer.LaunchAsync(new LaunchOptions
//                                              {
//                                                  Headless = true
//                                              });
//var page = await browser.NewPageAsync();
//await page.GoToAsync($"https://www.continente.pt/pesquisa/?q={keyword}");

//var content = await page.GetContentAsync();

//var context = BrowsingContext.New(Configuration.Default);
//var document = await context.OpenAsync(req => req.Content(content));

//var products = document.QuerySelectorAll("*")
//    .Where(e => e is {LocalName: "div", ClassName: "product"})
//    .ToList();

//products.ForEach(
//    product =>
//        {
//            if (product.QuerySelectorAll("*").FirstOrDefault(s => s is {LocalName : "a", ClassName : not null} and IHtmlAnchorElement && s.ClassName.Contains("pwc-tile--description")) is IHtmlAnchorElement itemName)
//            {
//                Console.WriteLine($"Product Name {itemName.Text}");
//            }

//            if (product.QuerySelectorAll("*").FirstOrDefault(s => s is { LocalName: "p", ClassName: not null } and IHtmlParagraphElement && s.ClassName.Contains("col-tile--brand")) is IHtmlParagraphElement itemBrand)
//            {
//                Console.WriteLine($"Product Brand: {itemBrand.TextContent}");
//            }

//            if (product.QuerySelectorAll("*").FirstOrDefault(s => s is { LocalName: "p", ClassName: not null } and IHtmlParagraphElement && s.ClassName.Contains("col-tile--quantity")) is IHtmlParagraphElement itemType)
//            {
//                Console.WriteLine($"Product Brand: {itemType.TextContent}");
//            }

//            if (product.QuerySelectorAll("*").FirstOrDefault(s => s is {LocalName: "span", ClassName: "value", } and IHtmlSpanElement ) is IHtmlSpanElement itemPrice)
//            {
//                var value = itemPrice.Attributes["content"]?.Value;
//                Console.WriteLine($"Product Price {value}");
//            }

//        });

Console.ReadLine();