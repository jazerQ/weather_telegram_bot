using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess { 
    public class TelegramServiceFactory : IDesignTimeDbContextFactory<TelegramDbContext>
    {
        public TelegramDbContext CreateDbContext(string[] args)
        {
            var provider = new DbContextOptionsBuilder<TelegramDbContext>();
            provider.UseNpgsql(appsettingJsonReader.GetConnectionString());
            return new TelegramDbContext(provider.Options);
        }
    }
}
