using Microsoft.EntityFrameworkCore;
using Mzstruct.DB.EFCore.Context;

namespace Mzstruct.DB.Providers.PostgreSQL.Context
{
    public class PostgreEFContext : AppEFContext<PostgreEFContext>
    {
        public PostgreEFContext(DbContextOptions<PostgreEFContext> options) : base(options) { }
    }
}
