// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseScrapper.cs" company="The Virtual Forge">
// Copyright (c) 2023 All Rights Reserved
// </copyright>
// <summary>
// 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Searcher.Scrappers.Implementations
{
    using AngleSharp;
    using AngleSharp.Dom;

    using PuppeteerSharp;
    using System.Web;

    public abstract class BaseScrapper : IDisposable , IAsyncDisposable
    {
        private bool headLess { get; set; } = true;

        
        private readonly BrowserFetcher browserFetcher;

        private IBrowser? browser = null;


        protected BaseScrapper()
        {
            this.browserFetcher = new BrowserFetcher();
            Task.Run(async () => await this.browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision));
        }
        protected BaseScrapper(bool headLess)
        {
            this.browserFetcher = new BrowserFetcher();
            Task.Run(async () => await this.browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision));
            this.headLess = headLess;
        }

        private async Task<IBrowser> SetupBrowser()
        {
            this.browser ??= await Puppeteer.LaunchAsync(new LaunchOptions {Headless = this.headLess});

            return this.browser;
        }

        public async Task<IDocument> GetDocument(string url)
        {
            await this.SetupBrowser();


            var page = await this.browser.NewPageAsync();
            
            await page.GoToAsync(url);

            var content = await page.GetContentAsync();

            var context = BrowsingContext.New(Configuration.Default);
            return await context.OpenAsync(req => req.Content(content));
        }

        public void Dispose()
        {
            this.browser.Dispose();
            this.browserFetcher.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (this.browser is not null)
                await this.browser.CloseAsync();


           
        }
    }
}