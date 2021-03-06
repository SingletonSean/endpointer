using Endpointer.Authentication.API.Extensions;
using Endpointer.Authentication.API.Models;
using Endpointer.Accounts.API.Extensions;
using Endpointer.Demos.Web.Contexts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using Firebase.Database;
using Google.Apis.Auth.OAuth2;
using Endpointer.Authentication.API.Firebase.Extensions;
using Endpointer.Accounts.API.Firebase.Extensions;
using System.Net.Mail;

namespace Endpointer.Demos.Web
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            AuthenticationConfiguration authenticationConfiguration = new AuthenticationConfiguration();
            _configuration.Bind("Authentication", authenticationConfiguration);

            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfiguration.AccessTokenSecret)),
                ValidIssuer = authenticationConfiguration.Issuer,
                ValidAudience = authenticationConfiguration.Audience,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };

            string connectionString = _configuration.GetConnectionString("sqlite");

            EmailVerificationConfiguration emailVerificationConfiguration = new EmailVerificationConfiguration()
            {
                EmailFromAddress = "admin@endpointer.com",
                EmailFromName = "Endpointer",
                TokenAudience = "localhost:5001",
                TokenIssuer = "localhost:5001",
                TokenExpirationMinutes = 60,
                TokenSecret = authenticationConfiguration.AccessTokenSecret,
                VerifyBaseUrl = "https://localhost:5001/verify",
                ConfigureFluentEmailServices = (builder) => builder.AddSmtpSender(() => new SmtpClient("localhost")
                {
                    EnableSsl = false,
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    PickupDirectoryLocation = @"C:/emails"
                })
            };

            // Entity Framework
            //services.AddEndpointerAuthentication(authenticationConfiguration,
            //    validationParameters,
            //    o => o
            //        .EnableEmailVerification(emailVerificationConfiguration)
            //        .RequireVerifiedEmail()
            //        .WithEntityFrameworkDataSource<CustomDbContext>(
            //            c => c.UseSqlite(connectionString)));

            //services.AddEndpointerAccounts(validationParameters,
            //    o => o.WithEntityFrameworkDataSource<CustomDbContext>(
            //        c => c.UseSqlite(connectionString)));

            // Firebase
            services.AddEndpointerAuthentication(authenticationConfiguration,
                validationParameters,
                o => o
                    .WithFirebaseDataSource(CreateFirebaseClient())
                    .EnableEmailVerification(emailVerificationConfiguration)
                    .RequireVerifiedEmail());

            services.AddEndpointerAccounts(validationParameters, o => o.WithFirebaseDataSource(CreateFirebaseClient()));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private FirebaseClient CreateFirebaseClient()
        {
            return new FirebaseClient(_configuration.GetValue<string>("FirebaseURL"),
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = async () =>
                    {
                        GoogleCredential credential = GetGoogleCredential();

                        return await credential.CreateScoped(
                                "https://www.googleapis.com/auth/userinfo.email",
                                "https://www.googleapis.com/auth/firebase.database")
                            .UnderlyingCredential.GetAccessTokenForRequestAsync();
                    },
                    AsAccessToken = true
                });
        }

        private GoogleCredential GetGoogleCredential()
        {
            if (IsDevelopment())
            {
                return GoogleCredential.FromFile("./firebase-credential.json");
            }
            else
            {
                return GoogleCredential.FromJson(Environment.GetEnvironmentVariable("FIREBASE_CONFIG"));
            }
        }

        private static bool IsDevelopment()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
        }
    }
}
