using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Configuration
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "cac3123-fasf1234-fasdf-das",
                    UserId = "12312-fadfasd-dasd"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "cac3123-fasda12g4-fssdf-das",
                    UserId = "12312-fadfasd-dasd"
                });
        }
    }
}