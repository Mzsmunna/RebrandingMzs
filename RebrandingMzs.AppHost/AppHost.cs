using RebrandingMzs.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Tasker_RestAPI>("tasker-restapi")
    .WithSwaggerUI()
    .WithScalar();

builder.Build().Run();
