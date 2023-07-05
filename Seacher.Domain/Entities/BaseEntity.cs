// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseEntity.cs" company="The Virtual Forge">
// Copyright (c) 2023 All Rights Reserved
// </copyright>
// <summary>
// 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Seacher.Domain.Entities;

using Seacher.Domain.Common.Interfaces;

public abstract class BaseEntity<T> : BaseEntity, IEntity<T> where T : struct
{
    public T Id { get; set; }
}

public abstract class BaseEntity : IEntity
{
    
}