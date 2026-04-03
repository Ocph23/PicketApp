using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PicketMobile.Services;
using The49.Maui.BottomSheet;
using ZXing.Net.Maui.Controls;

namespace PicketMobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.Services.AddSingleton<IAccountService, AccountService>();
            builder.Services.AddSingleton<IPicketService, PicketService>();
            builder.Services.AddSingleton<IScheduleService, ScheduleService>();
            builder.Services.AddSingleton<IStudentService, StudentService>();
            builder.Services.AddHttpClient<IPicketService, PicketService>(client =>
            {
                client.BaseAddress = new Uri(Preferences.Get("url", "http://localhost:5001"));
                client.DefaultRequestHeaders.Add("Content-Type", "application/json; charset=utf-8");
                var token = Preferences.Get("token", null);
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
            });
            builder.UseMauiApp<App>()
                .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
                .UseMauiCommunityToolkit()
                 .UseBottomSheet()
                .UseBarcodeReader()
            ;
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}