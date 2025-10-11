var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("db")
    //.WithPgAdmin()
    .WithDataVolume("postgres_data");
var kursusdb = postgres.AddDatabase("piketdb");

var redis = builder.AddRedis("Redis");

//var apiService = builder.AddProject<Projects.AspireApp_ApiService>("apiservice")
//    .WithHttpHealthCheck("/health");

//builder.AddProject<Projects.AspireApp_Web>("webfrontend")
//    .WithExternalHttpEndpoints()
//    .WithHttpHealthCheck("/health")
//    .WithReference(apiService)
//    .WaitFor(apiService);





var picketapi = builder.AddProject<Projects.PiketWebApi>("piketapi")
    .WithReference(redis)
    .WaitFor(postgres)
     .WithHttpEndpoint(name: "rest", port: 5001, isProxied: false)
    //.WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints();
//.WithHttpHealthCheck("/health");



builder.AddNpmApp("vue", "../../smkn8picket-client")
    .WithReference(picketapi)
    .WaitFor(picketapi)
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();


builder.Build().Run();
