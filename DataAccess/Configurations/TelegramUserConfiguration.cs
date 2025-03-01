using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class TelegramUserConfiguration : IEntityTypeConfiguration<TelegramUser>
    {
        public void Configure(EntityTypeBuilder<TelegramUser> builder)
        {
            builder.HasKey(u => u.Id);

        }
    }
}
