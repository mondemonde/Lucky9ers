using System.Reflection;
using Lucky9.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Lucky9.Domain.Entities;
using Lucky9.Infrastructure.Identity;
using Lucky9.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore.Design;
using static IdentityModel.ClaimComparer;
using Microsoft.Extensions.Configuration;
using Lucky9.Infrastructure.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Domain.Models;
using IdentityServer4.EntityFramework.Options;

namespace Lucky9.Infrastructure.Persistence;





public class AppDbContext : DbContext, IDataContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        IMediator mediator,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor)
        : base(options )
    {
        _mediator = mediator;
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }


    public AppDbContext(
        DbContextOptions<AppDbContext> options
        )
        : base(options)
    {
    }
    public string DbPath { get => "sqlite.db"; }
   readonly string DefaultConnection = "Data Source=ITGLAP0018\\SQL2019;Database=valocityDB;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true";

    //readonly string DefaultConnection = @"Server=(localdb)\\mssqllocaldb;Database=Project3Db;Trusted_Connection=True;MultipleActiveResultSets=true";

    //_Step#1.3 db connections
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)

         => optionsBuilder.UseSqlite($"Data Source={DbPath}");
         //=> optionsBuilder.UseSqlServer(DefaultConnection);


       // public DbSet<ApplicationUser> AspNetUsers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Bet> Bets { get; set; }
       public  DbSet<User> Users { get; set; }

    // public DbSet<IPlayer> AspNetUsers => DbSet < ApplicationUser > AspNetUsers;

    public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Player>()
            //    .ToTable("Players").HasKey(i => i.Id);

            modelBuilder.Entity<Bet>()
             .ToTable("Bets").HasKey(i => i.Id);


            modelBuilder.Entity<Game>()
           .ToTable("Games").HasKey(i => i.Id)
           ;



            base.OnModelCreating(modelBuilder);
        }
    }

    public class YourDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public string DbPath { get => "sqlite.db"; }
        readonly string DefaultConnection = "Data Source=ITGLAP0018\\SQL2019;Database=valocityDB;Integrated Security=True;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true";

    DbContextOptions<AppDbContext> _options {  get; set; }
      IOptions<OperationalStoreOptions> _operationalStoreOptions {  get; set; }
   

    //public YourDbContextFactory (DbContextOptions<DataContext> options,
    //    IOptions<OperationalStoreOptions> operationalStoreOptions)
    //{
    //    _operationalStoreOptions = operationalStoreOptions;
    //    _options = options;
    //}

    public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        //_Step#1.2 db connections
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
       //  optionsBuilder.UseSqlServer(DefaultConnection);


_operationalStoreOptions = new OperationalStoreOptionsMigrations();

            return new AppDbContext(optionsBuilder.Options);
        }


    public class OperationalStoreOptionsMigrations :
   IOptions<OperationalStoreOptions>
    {
        public OperationalStoreOptions Value => new OperationalStoreOptions()
        {
            DeviceFlowCodes = new TableConfiguration("DeviceCodes"),
            EnableTokenCleanup = false,
            PersistedGrants = new TableConfiguration("PersistedGrants"),
            TokenCleanupBatchSize = 100,
            TokenCleanupInterval = 3600,
        };
    }

}






