// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Repository.cs" company="The Virtual Forge">
// Copyright (c) 2023 All Rights Reserved
// </copyright>
// <summary>
// 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Searcher.Persistence.Repositories;

using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Seacher.Domain.Common.Interfaces;

using Searcher.Application.Repositories;

public abstract class Repository<T, TPK> : IRepository<T, TPK>
    where T : class, IEntity<TPK> where TPK : struct
{
    private readonly IDbContext dbContext;
    private readonly DbSet<T> dbSet;

    protected Repository(IDbContext dbContext)
    {
        this.dbContext = dbContext;
        dbSet = dbContext.Set<T>();
    }

    public T? GetById(TPK id)
    {
        return dbSet.Find(id);
    }

    public async Task<T?> GetByIdAsync(TPK id)
    {
        return await dbSet.FindAsync(id);
    }

    public IEnumerable<T> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> Find(Expression<Func<T, bool>> expression)
    {
        throw new NotImplementedException();
    }

    public void Add(T entity)
    {
        throw new NotImplementedException();
    }

    public void AddRange(IEnumerable<T> entities)
    {
        this.dbSet.AddRange(entities);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await this.dbSet.AddRangeAsync(entities);
    }

    public void Remove(T entity)
    {
        throw new NotImplementedException();
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        throw new NotImplementedException();
    }
}