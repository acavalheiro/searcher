// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IScrapper.cs" company="The Virtual Forge">
// Copyright (c) 2023 All Rights Reserved
// </copyright>
// <summary>
// 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Seacher.Domain.Models;

namespace Searcher.Scrappers
{
    public interface IScrapper : IDisposable , IAsyncDisposable
    {
        /// <summary>
        /// The search products.
        /// </summary>
        /// <param name="searchTerm">
        /// The search term.
        /// </param>
        /// <returns>
        /// The <see cref="IEnumerable"/>.
        /// </returns>
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
    }
}