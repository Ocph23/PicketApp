var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("db")
    //.WithPgAdmin()
    .WithDataVolume("postgres_data");

var kursusdb = postgres.AddDatabase("piketdb");

var redis = builder.AddRedis("Redis");

var picketapi = builder.AddProject<Projects.PiketWebApi>("piketapi")
    .WithReference(redis)
    .WaitFor(postgres);

builder.AddNpmApp("vue", "../../smkn8picket-client")
    .WithReference(picketapi)
    .WaitFor(picketapi)
     .WithEnvironment("VITE_API_URL", picketapi.GetEndpoint("https"))
     .WithHttpEndpoint(name: "vue-client", port: 5173, isProxied: false)
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();


builder.Build().Run();
