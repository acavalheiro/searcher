// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IRepository.cs" company="The Virtual Forge">
// Copyright (c) 2023 All Rights Reserved
// </copyright>
// <summary>
// 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using Seacher.Domain.Common.Interfaces;
using System.Linq.Expressions;
using System.Security.Principal;

namespace Searcher.Application.Repositories;

public interface IRepository<T, TPK> 
    where T : class, IEntity<TPK>
    where TPK : struct
{
    T? GetById(TPK id);

    Task<T?> GetByIdAsync(TPK id);

    IEnumerable<T> GetAll();
    IEnumerable<T> Find(Expression<Func<T, bool>> expression);
    void Add(T entity);
    void AddRange(IEnumerable<T> entities);
    Task AddRangeAsync(IEnumerable<T> entities);
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
}