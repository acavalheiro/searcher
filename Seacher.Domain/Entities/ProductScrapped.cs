// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductScrapped.cs" company="The Virtual Forge">
// Copyright (c) 2023 All Rights Reserved
// </copyright>
// <summary>
// 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Seacher.Domain.Common.Interfaces;

namespace Seacher.Domain.Entities;

public class ProductScrapped : BaseEntity<int>, IEntity<int>
{
    public ProductScrapped() => this.CreatedDate = DateTime.Now;


    public int StoreId { get; set; }

    public string? InternalId { get; set; }

    
    public string? Name { get; set; }

    public string? Description { get; set; }

    public decimal? Value { get; set; }

    public string? Brand { get; set; }

    public string? EAN { get; set; }

    public string? Url { get; set; }

    public string? Size { get; set; }

    public DateTime CreatedDate { get; set; }

    public string RunCode { get; set; }
}