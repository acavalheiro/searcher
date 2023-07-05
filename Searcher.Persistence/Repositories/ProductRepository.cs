// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductRepository.cs" company="The Virtual Forge">
// Copyright (c) 2023 All Rights Reserved
// </copyright>
// <summary>
// 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Searcher.Persistence.Repositories;

using Seacher.Domain.Common.Interfaces;
using Seacher.Domain.Entities;using Searcher.Application.Repositories;

public class ProductRepository : Repository<Product,Guid> , IProductRepository
{
    public ProductRepository(IDbContext dbContext)
        : base(dbContext)
    {
    }
}