// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ApplicationDbContext.cs" company="The Virtual Forge">
// Copyright (c) 2023 All Rights Reserved
// </copyright>
// <summary>
// 
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Searcher.Persistence.Contexts;

using Microsoft.EntityFrameworkCore;

using Seacher.Domain.Common.Interfaces;
using Seacher.Domain.Entities;

public sealed class ApplicationDbContext : DbContext , IDbContext
{
    //public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    //    : base(options)
    //{
    //    this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    //}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=.\SQLEXPRESS;Database=Searcher;Integrated Security=SSPI;MultipleActiveResultSets=true;TrustServerCertificate=true");
    }

    public DbSet<Store> Stores => Set<Store>();

    public DbSet<ProductScrapped> ProductScrappeds => Set<ProductScrapped>();

    public new DbSet<TEntity> Set<TEntity>()
        where TEntity : class
    {
        return base.Set<TEntity>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Store>().HasKey(x => x.Id);
        modelBuilder.Entity<Store>().Property(x => x.Name).IsRequired().HasMaxLength(255);
        modelBuilder.Entity<Store>().HasData(new Store() {Id = 1, Name = "Continente"}, new Store() {Id = 2, Name = "Auchan"}, new Store() {Id = 3, Name = "El Corte Ingles"});

        modelBuilder.Entity<ProductScrapped>().HasKey(x => x.Id);
        modelBuilder.Entity<ProductScrapped>().ToTable("ProductScrappeds").Property(p => p.Id)
            .HasColumnType("bigint");

        modelBuilder.Entity<ProductScrapped>().Property(x => x.RunCode).IsRequired().HasMaxLength(40);

        base.OnModelCreating(modelBuilder);
    }
}