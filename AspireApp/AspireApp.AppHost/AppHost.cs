using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// 🔹 Tambahkan Docker Compose environment (opsional)
builder.AddDockerComposeEnvironment("compose")
       .WithDashboard(dashboard =>
       {
           dashboard.WithHostPort(8181)
                    .WithForwardedHeaders(enabled: true);
       });

// 🔹 PostgreSQL + Database
var postgres = builder.AddPostgres("db")
    //.WithPgAdmin()
    .WithDataVolume("postgres_data");

var db= postgres.AddDatabase("piketdb");

// 🔹 Redis
var redis = builder.AddRedis("Redis");

// 🔹 ASP.NET Core Web API
var picketapi = builder.AddProject<Projects.PiketWebApi>("piketapi")
    .WithReference(redis) 
    .WithReference(db) 
    //.WithEnvironment("ASPNETCORE_PORT", "5001")
    .WaitFor(db);


builder.AddNpmApp("adminclient", "../../smkn8picket-client")
    .WithReference(picketapi)
    .WaitFor(picketapi)
    .WithEnvironment("VITE_API_URL", picketapi.GetEndpoint("https"))
    .WithHttpEndpoint(name: "vue-client", port: 5173, isProxied: false);


// 🔹 Bangun dan jalankan aplikasi Aspire

builder.Build().Run();
