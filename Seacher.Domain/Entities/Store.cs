// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Store.cs" company="The Virtual Forge">
// Copyright (c) 2023 All Rights Reserved
// </copyright>
// <summary>
// 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Seacher.Domain.Common.Interfaces;

namespace Seacher.Domain.Entities;

public class Store : BaseEntity<int>, IEntity<int>
{
    public string Name { get; set; }
}