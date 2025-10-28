using CommunityToolkit.Maui.Core;
using CommunityToolkit.Mvvm.Messaging;
using PicketMobile.Models;
using PicketMobile.Services;
using SharedModel;
using ZXing.Net.Maui;
using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Mvvm.ComponentModel;

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
        vm.IsDetecting = false;
        var first = e.Results?.FirstOrDefault();
        if (first is null || first.Value == vm.LastScan)
        {
            vm.IsDetecting = true;
            return;
        }
        Vibration.Default.Vibrate();
        vm.LastScan = first.Value;

    }
    private void BarcodesDetected(object sender, BarcodeDetectionEventArgs e)
    {

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