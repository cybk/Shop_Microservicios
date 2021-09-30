using Account.Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Account.Api.Context
{
    public class AccountDbContext : IdentityDbContext<MyUser>
    {
        public AccountDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
