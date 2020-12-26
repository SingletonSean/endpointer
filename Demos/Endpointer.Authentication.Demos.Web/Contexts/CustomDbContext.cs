﻿using Endpointer.Authentication.API.Contexts;
using Endpointer.Authentication.API.Models;
using Endpointer.Core.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Endpointer.Authentication.Demos.Web.Contexts
{
    public class CustomDbContext : DbContext, IAuthenticationDbContext<User>
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
