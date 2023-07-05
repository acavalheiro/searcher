// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDbContext.cs" company="The Virtual Forge">
// Copyright (c) 2023 All Rights Reserved
// </copyright>
// <summary>
// 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Seacher.Domain.Common.Interfaces;

using Microsoft.EntityFrameworkCore;

using Seacher.Domain.Entities;

public interface IDbContext : IDisposable
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}