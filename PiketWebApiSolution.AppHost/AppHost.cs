using Google.Protobuf.WellKnownTypes;

var builder = DistributedApplication.CreateBuilder(args);

int picketApiPort = 5001;

// Tambahkan Docker Compose environment (opsional)
builder.AddDockerComposeEnvironment("env")
  .WithDashboard(dashboard =>
  {
      dashboard.WithHostPort(8181)
                .WithForwardedHeaders(enabled: true);
  });



// ?? PostgreSQL + Database
//var backupPath = Path.GetFullPath(@"D:\smk8\PicketApp\pgadmin_backups");
//Directory.CreateDirectory(backupPath);

var postgres = builder.AddPostgres("db")
    // .WithPgAdmin(pgadmin =>
    // {
    // var backupPathX = "/mnt/d/smk8/PicketApp/pgadmin_backups";
    //     pgadmin.WithVolume($"{backupPathX}:/backups"); // <--- tambahkan volume ini
    // })
    .WithDataVolume("picketdb_volume");

var db = postgres.AddDatabase("piketdb");

// ?? Redis
var redis = builder.AddRedis("cache");


//whatsapp
var whatsapp = builder.AddDockerfile("whatsapp", "../WhatsApp")
    .WithReference(db)
    .WithEnvironment("DATABASE_URL", "postgresql://postgres:${DB_PASSWORD}@db:5432/piketdb")
    .WithVolume("whatsapp_auth", "/usr/src/app/.wwebjs_auth")
    .WithHttpEndpoint(port: 3000, targetPort: 3000)
    .WithExternalHttpEndpoints()
    .PublishAsDockerComposeService((resource, service) =>
    {
        service.Name = "whatsapp";
    });

// ?? ASP.NET Core Web API
var picketapi = builder.AddProject<Projects.PiketWebApi>("piketapi")
    .WithReference(redis)
    .WithReference(db)          
    .WithEnvironment("WHATSAPP_API_URL", whatsapp.GetEndpoint("http"))
    .WithEnvironment("JWT_SECRET", "O0ywQA1xv4h1EXL4cnsQ4ReHEoqwMuejtXB4KxOeTCB4nOpV4yVegegbEgtzsZWfl5wlBWBY5kpUPVsEmkVr3V9sxvOPmT4YR3PvUCiw7s1xMrSoCMVqnG7VlSeO469Z")
    .WithEnvironment("JWT_ISSUER", $"https://localhost:{picketApiPort.ToString()}")
    .WithEnvironment("JWT_AUDIENCE", $"https://localhost:{picketApiPort.ToString()}")
    .WithEnvironment("DEFAULT_PASSWORD", "Password@123")
    .WithEnvironment("PIKETAPI_PORT", picketApiPort.ToString())
    .WaitFor(db)
    .WaitFor(redis)
    .WithEndpoint(port: picketApiPort)
    .WithExternalHttpEndpoints()
    .PublishAsDockerComposeService((resource, service) =>
    {
        service.Name = "picketapi";
    });

//DEV TUNNEL
//var tunnel = builder.AddDevTunnel("mytunnel")
//                    .WithReference(picketapi.GetEndpoint("https"), allowAnonymous: true)
//                    .WaitFor(picketapi)
//                    ;




//var bunApp = builder.AddBunApp("picket-admin-client", "../smkn8picket-client")
//    .WithHttpEndpoint(port: 3000, env: "PORT");

var adminclient = builder.AddBunApp("picket-admin-client", "../smkn8picket-client", "dev")
    .WithReference(picketapi)
    .WaitFor(picketapi)
    //.WithEnvironment("VITE_API_URL", picketapi.GetEndpoint("http"))
    //.WithEnvironment("VITE_LOGO", "/smk.png")
    .WithExternalHttpEndpoints()
    .PublishAsDockerComposeService((resource, service) =>
    {
        service.Name = "picket-admin-client";
    });
;



builder.Build().Run();
