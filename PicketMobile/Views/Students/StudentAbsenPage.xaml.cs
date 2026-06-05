using CommunityToolkit.Mvvm.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using ZXing.Net.Maui;

namespace PicketMobile.Views.Students;

public partial class StudentAbsenPage : ContentPage
{
    private StudentAbsenViewModel vm;

    public StudentAbsenPage()
    {
        InitializeComponent();
        BindingContext =vm = new StudentAbsenViewModel(); ;
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
            vm.LastScan = first.Value;

            var certPath = Path.Combine(AppContext.BaseDirectory, "appabsen_qr_local_ca.cer");

            var cert = new X509Certificate2(certPath, "Password@123");

            var handler = new HttpClientHandler();
            handler.ClientCertificates.Add(cert);

            using var httpClient = new HttpClient(handler);

            var response = await httpClient.GetAsync("https://192.168.10.3:5056");
            var content = await response.Content.ReadAsStringAsync();

            Console.WriteLine(content);

        }
        catch (Exception ex)
        {
          await DisplayAlert("Error", ex.Message, "OK");
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