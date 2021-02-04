﻿using Endpointer.Authentication.API.Contexts;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Endpointer.Authentication.API.Services.UserRepositories;
using Endpointer.Core.API.Models;
using Firebase.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Endpointer.Authentication.API.Models
{
    public class EndpointerAuthenticationOptionsBuilder
    {
        private Action<IServiceCollection> _addDataSourceServices;

        public EndpointerAuthenticationOptionsBuilder()
        {
            _addDataSourceServices = services =>
            {
                services.AddSingleton<IUserRepository, InMemoryUserRepository>();
                services.AddSingleton<IRefreshTokenRepository, InMemoryRefreshTokenRepository>();
            };
        }

        /// <summary>
        /// Add database services to Endpointer with a DefaultAuthenticationDbContext.
        /// </summary>
        /// <param name="dbOptions">The options to configure the DbContext.</param>
        /// <returns>The builder to configure options.</returns>
        public EndpointerAuthenticationOptionsBuilder WithEntityFrameworkDataSource(Action<DbContextOptionsBuilder> dbOptions = null)
        {
            return WithEntityFrameworkDataSource<DefaultAuthenticationDbContext>(dbOptions);
        }

        /// <summary>
        /// Add Entity Framework database services to Endpointer.
        /// </summary>
        /// <typeparam name="TDbContext">The type of DbContext for the database.</typeparam>
        /// <param name="dbOptions">The options to configure the DbContext.</param>
        /// <returns>The builder to configure options.</returns>
        public EndpointerAuthenticationOptionsBuilder WithEntityFrameworkDataSource<TDbContext>(Action<DbContextOptionsBuilder> dbOptions = null)
            where TDbContext : DbContext, IAuthenticationDbContext<User>
        {
            dbOptions = dbOptions ?? (o => { });

            _addDataSourceServices = services =>
            {
                services.AddDbContext<TDbContext>(dbOptions);
                services.AddScoped<IUserRepository, DatabaseUserRepository<TDbContext>>();
                services.AddScoped<IRefreshTokenRepository, DatabaseRefreshTokenRepository<TDbContext>>();
            };

            return this;
        }

        /// <summary>
        /// Add Firebase data source services.
        /// </summary>
        /// <param name="firebaseClient">The client to connect to Firebase.</param>
        /// <returns>The builder to configure options.</returns>
        public EndpointerAuthenticationOptionsBuilder WithFirebaseDataSource(FirebaseClient firebaseClient)
        {
            _addDataSourceServices = services =>
            {
                services.AddSingleton(firebaseClient);
                services.AddSingleton<IUserRepository, FirebaseUserRepository>();
                services.AddSingleton<IRefreshTokenRepository, FirebaseRefreshTokenRepository>();
            };

            return this;
        }

        /// <summary>
        /// Add data source services using a custom data source.
        /// </summary>
        /// <param name="dataSourceConfiguration">The configuration of the custom data source services.</param>
        /// <returns>The builder to configure options.</returns>
        public EndpointerAuthenticationOptionsBuilder WithCustomDataSource(CustomDataSourceConfiguration dataSourceConfiguration)
        {
            _addDataSourceServices = services =>
            {
                dataSourceConfiguration.AddUserRepository(services);
                dataSourceConfiguration.AddRefreshTokenRepository(services);
            };

            return this;
        }

        /// <summary>
        /// Build the Endpointer options.
        /// </summary>
        /// <returns>The built options.</returns>
        public EndpointerAuthenticationOptions Build()
        {
            return new EndpointerAuthenticationOptions()
            {
                AddDataSourceServices = _addDataSourceServices,
            };
        }
    }
}
