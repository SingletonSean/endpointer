using Endpointer.Accounts.API.Firebase.Services.AccountRepositories;
using Endpointer.Accounts.API.Models;
using Endpointer.Accounts.API.Services.AccountRepositories;
using Firebase.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Endpointer.Accounts.API.Firebase.Extensions
{
    public static class EndpointerAccountsOptionsBuilderFirebaseExtensions
    {
        /// <summary>
        /// Add Firebase data source services.
        /// </summary>
        /// <param name="firebaseClient">The client to connect to Firebase.</param>
        /// <returns>The builder to configure options.</returns>
        public static EndpointerAccountsOptionsBuilder WithFirebaseDataSource(this EndpointerAccountsOptionsBuilder builder, 
            FirebaseClient firebaseClient)
        {
            builder.WithCustomDataSource(services =>
            {
                services.AddSingleton(firebaseClient);
                services.AddSingleton<IAccountRepository, FirebaseAccountRepository>();
            });

            return builder;
        }
    }
}
