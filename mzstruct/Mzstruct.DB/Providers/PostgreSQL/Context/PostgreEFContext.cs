using Microsoft.EntityFrameworkCore;
using Mzstruct.Base.Enums;
using Mzstruct.DB.ORM.EFCore.Context;
using Mzstruct.DB.ORM.EFCore.Helpers;

namespace Mzstruct.DB.Providers.PostgreSQL.Context
{
    public class PostgreEFContext : EFContext
    {
        public PostgreEFContext(DbContextOptions options) : base(options) { }
    }
}
