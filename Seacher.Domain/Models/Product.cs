// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Product.cs" company="The Virtual Forge">
// Copyright (c) 2023 All Rights Reserved
// </copyright>
// <summary>
// 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Seacher.Domain.Models
{
    using System.Globalization;

    public class Product
    {
        public string? Id { get; set; }
        public string? Name { get; set; }

        public string Description { get; set; }

        public decimal? Value { get; set; }

        public string Brand { get; set; }

        public string EAN { get; set; }

        public string Url { get; set; }

        public string? Size { get; set; }
    }
}