var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.API_Users>("api-users");

builder.AddProject<Projects.API_Movies>("api-movies");

builder.AddProject<Projects.API_Books>("api-books");

builder.AddProject<Projects.API_Auth>("api-auth");

builder.AddProject<Projects.API_Gateway>("api-gateway");

builder.Build().Run();
