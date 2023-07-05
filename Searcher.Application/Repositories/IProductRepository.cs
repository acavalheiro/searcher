// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IProductRepository.cs" company="The Virtual Forge">
// Copyright (c) 2023 All Rights Reserved
// </copyright>
// <summary>
// 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Searcher.Application.Repositories;

using Seacher.Domain.Entities;

public interface IProductRepository : IRepository<Product, Guid>
{
    
}