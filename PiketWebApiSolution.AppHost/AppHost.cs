var builder = DistributedApplication.CreateBuilder(args);

// 🔹 Tambahkan Docker Compose environment (opsional)
//builder.AddDockerComposeEnvironment("compose")
//       .WithDashboard(dashboard =>
//       {
//           dashboard.WithHostPort(8181)
//                    .WithForwardedHeaders(enabled: true);
//       });

// 🔹 PostgreSQL + Database
//var backupPath = Path.GetFullPath(@"D:\smk8\PicketApp\pgadmin_backups");
//Directory.CreateDirectory(backupPath);
var backupPathX = "/mnt/d/smk8/PicketApp/pgadmin_backups";

var postgres = builder.AddPostgres("db")
    .WithPgAdmin(pgadmin =>
    {
        pgadmin.WithVolume($"{backupPathX}:/backups"); // <--- tambahkan volume ini
    })
    .WithDataVolume("postgres_data");

var db = postgres.AddDatabase("piketdb");

// 🔹 Redis
var redis = builder.AddRedis("cache");

// 🔹 ASP.NET Core Web API
var picketapi = builder.AddProject<Projects.PiketWebApi>("piketapi")
    .WithReference(redis)
    .WithReference(db)
    .WaitFor(db);


//DEV TUNNEL
//var tunnel = builder.AddDevTunnel("mytunnel")
//                    .WithReference(picketapi.GetEndpoint("https"), allowAnonymous: true)
//                    .WaitFor(picketapi)
//                    ;



builder.AddNpmApp("adminclient", "../smkn8picket-client")
    .WithReference(picketapi)
    .WaitFor(picketapi)
    .WithEnvironment("VITE_API_URL", picketapi.GetEndpoint("https"))
    .WithHttpEndpoint(name: "vue-client", port: 5173, isProxied: false);


// 🔹 Bangun dan jalankan aplikasi Aspire

builder.Build().Run();
