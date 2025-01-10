using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace UWPMessenger.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MessageAppContext>
    {
        public MessageAppContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MessageAppContext>();
            optionsBuilder.UseSqlite("Data Source=UWPMessenger.db");

            return new MessageAppContext(optionsBuilder.Options);
        }
    }
}
