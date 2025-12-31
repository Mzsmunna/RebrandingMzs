using Microsoft.EntityFrameworkCore;
using Mzstruct.DB.EFCore.Context;

namespace Mzstruct.DB.Providers.PostgreSQL.Context
{
    public class PostgreEFContext : EFContext
    {
        public PostgreEFContext(DbContextOptions options) : base(options) { }
    }
}
