// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEntity.cs" company="The Virtual Forge">
// Copyright (c) 2023 All Rights Reserved
// </copyright>
// <summary>
// 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Seacher.Domain.Common.Interfaces;

public interface IEntity<T> where T : struct
{
    public T Id { get; set; }
}
public interface IEntity
{}