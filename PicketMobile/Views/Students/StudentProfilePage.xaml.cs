using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PicketMobile.Services;
using SharedModel.Responses;
using System.Text.Json;

namespace PicketMobile.Views.Students;

public partial class StudentProfilePage : ContentPage
{
	public StudentProfilePage()
	{
		InitializeComponent();
        BindingContext = new StudentProfilePageViewModel();
    }
}



internal partial class StudentProfilePageViewModel : ObservableObject
{
    [ObservableProperty]
    private StudentResponse profile;


    [ObservableProperty]
    private string initial;

    [ObservableProperty]
    private string nomorRegistrasi;


    [ObservableProperty]
    private string photo;

    public AsyncRelayCommand LogoutCommand { get; set; }
    public AsyncRelayCommand KartuPelajarCommand { get; set; }
    public StudentProfilePageViewModel()
    {
        LogoutCommand = new AsyncRelayCommand(LogoutAction);
        KartuPelajarCommand = new AsyncRelayCommand(KartuPelajarAction);
        string? profileString = Preferences.Get("profile", null);
        if (!string.IsNullOrEmpty(profileString))
        {
            Profile = JsonSerializer.Deserialize<StudentResponse>(profileString, Helper.JsonOption)!;
            Initial = Helper.GetInitial(Profile == null ? "" : Profile.Name)!;
            NomorRegistrasi = $"{Profile.NIS}/{Profile.NISN}";
            var baseUrl = Preferences.Get("url", string.Empty);
            Photo = $"{baseUrl}/photos/student/{Profile.Photo}";
        }
    }

    private async Task KartuPelajarAction()
    {
       await Shell.Current.Navigation.PushModalAsync(new StudentCardIdPage());
    }

    private async Task LogoutAction()
    {
        IAccountService service = ServiceHelper.GetService<IAccountService>()!;
        await service.Logout();
    }
}