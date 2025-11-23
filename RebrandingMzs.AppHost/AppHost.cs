var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Tasker_RestAPI>("tasker-restapi");

builder.Build().Run();
