using Endpointer.Authentication.API.Firebase.Services.RefreshTokenRepositories;
using Endpointer.Authentication.API.Models;
using Endpointer.Authentication.API.Services.RefreshTokenRepositories;
using Endpointer.Authentication.API.Services.UserRepositories;
using Firebase.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Endpointer.Authentication.API.Firebase.Extensions
{
    public static class EndpointerAuthenticationOptionsBuilderFirebaseExtensions
    {
        /// <summary>
        /// Add Firebase data source services.
        /// </summary>
        /// <param name="firebaseClient">The client to connect to Firebase.</param>
        /// <returns>The builder to configure options.</returns>
        public static EndpointerAuthenticationOptionsBuilder WithFirebaseDataSource(this EndpointerAuthenticationOptionsBuilder builder, 
            FirebaseClient firebaseClient)
        {
            builder.WithCustomDataSource(services =>
            {
                services.AddSingleton(firebaseClient);
                services.AddSingleton<IUserRepository, FirebaseUserRepository>();
                services.AddSingleton<IRefreshTokenRepository, FirebaseRefreshTokenRepository>();
            });

            return builder;
        }
    }
}
