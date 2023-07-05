// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IAuditableEntity.cs" company="The Virtual Forge">
// Copyright (c) 2023 All Rights Reserved
// </copyright>
// <summary>
// 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Security.Principal;

namespace Seacher.Domain.Common.Interfaces;

public interface IAuditableEntity<T> : IEntity<T> where T : struct
{
    T? CreatedBy { get; set; }
    DateTime? CreatedDate { get; set; }
    T? UpdatedBy { get; set; }
    DateTime? UpdatedDate { get; set; }
}