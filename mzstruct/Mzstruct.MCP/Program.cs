using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mzstruct.Base.Entities;
using Mzstruct.DB.Providers.MongoDB.Contracts.IRepos;
using Mzstruct.DB.Providers.MongoDB.Repos;
using Mzstruct.MCP.Tools;

namespace Mzstruct.MCP
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //Console.WriteLine("Hello, MCP!");
            var builder = Host.CreateApplicationBuilder(args);

            builder.Logging.AddConsole(options =>
            {
                options.LogToStandardErrorThreshold = LogLevel.Trace;
            });

            builder.Services.AddSingleton<IBaseUserRepository<BaseUser>, BaseUserRepository<BaseUser>>();
            builder.Services.AddMcpServer().WithStdioServerTransport()
                .WithTools<UserTools>();
                //.WithToolsFromAssembly();

            await builder.Build().RunAsync();
        }
    }
}
