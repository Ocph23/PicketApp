using CommunityToolkit.Mvvm.ComponentModel;
using PicketMobile.Models;
using SharedModel.Responses;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using ZXing.Net.Maui;

namespace PicketMobile.Views.Students;

public partial class StudentAbsenPage : ContentPage
{
    private StudentAbsenViewModel vm;

    public StudentAbsenPage()
    {
        InitializeComponent();
        BindingContext = vm = new StudentAbsenViewModel(); ;
        cameraBarcodeReaderView.Options = new BarcodeReaderOptions
        {
            Formats = BarcodeFormats.TwoDimensional,
            AutoRotate = true,
            Multiple = true
        };
    }


    private async void cameraBarcodeReaderView_BarcodesDetected(object sender, ZXing.Net.Maui.BarcodeDetectionEventArgs e)
    {
        try
        {
            vm.IsDetecting = false;
            var first = e.Results?.FirstOrDefault();
            if (first is null || first.Value == vm.LastScan)
            {
                vm.IsDetecting = true;
                return;
            }
            Vibration.Default.Vibrate();

            QRCodeModel qrcode = JsonSerializer.Deserialize<QRCodeModel>(first.Value, Helper.JsonOption);

            using var stream = await FileSystem.OpenAppPackageFileAsync("appabsen_qr_local_ca.cer");

            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);

            var cert = new X509Certificate2(ms.ToArray(), "Password@123");

            var handler = new HttpClientHandler() {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            handler.ClientCertificates.Add(cert);

            using var httpClient = new HttpClient(handler) {  BaseAddress = new Uri("https://192.168.1.9:5056") };


            string? profileString = Preferences.Get("profile", null);
            if (!string.IsNullOrEmpty(profileString))
            {
                var profile = JsonSerializer.Deserialize<StudentResponse>(profileString, Helper.JsonOption)!;
                var request = new AbsenRequestModel
                {
                    SessionId = qrcode.SessionId,
                    NomorKartu =profile.NISN ?? profile.NIS,
                    DeviceId = DeviceInfo.Name
                };

                var response = await httpClient.PostAsync("/api/qr-attendance", new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json"));
                var responseContent = await response.Content.ReadAsStringAsync();
                var qRAbsenResponse = JsonSerializer.Deserialize<QRAbsenResponse>(responseContent, Helper.JsonOption);
                if(response.IsSuccessStatusCode)
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    DisplayAlert("Absen Berhasil", qRAbsenResponse?.Message ?? "Berhasil melakukan absen", "OK"));
                }
                else
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    DisplayAlert("Absen Gagal", qRAbsenResponse?.Message ?? "Gagal melakukan absen", "OK"));
                }
            }
        }
        catch (Exception ex)
        {
            await MainThread.InvokeOnMainThreadAsync(() =>
            DisplayAlert("Error", ex.Message, "OK"));
        }

    }
    private async Task BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {
        var cert = new X509Certificate2("appabsen-qr-local-ca.cer", "Password@123");

        var handler = new HttpClientHandler();
        handler.ClientCertificates.Add(cert);

        using var httpClient = new HttpClient(handler);

        var response = await httpClient.GetAsync("https://192.168.10.3:5056");
        var content = await response.Content.ReadAsStringAsync();

        Console.WriteLine(content);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        vm.LastScan = string.Empty;
        vm.IsDetecting = true;
    }


    public partial class StudentAbsenViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isDetecting = true;

        [ObservableProperty]
        private string lastScan;
    }
}