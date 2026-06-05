using PicketMobile.Services;
using PicketMobile.Views;
using PicketMobile.Views.Students;

namespace PicketMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            try
            {
                string token = Preferences.Get("token", null);

                if (string.IsNullOrEmpty(token))
                {
                    return new Window(new LoginPage());
                }

                IAccountService? service = ServiceHelper.GetService<IAccountService>();

                if (service == null)
                {
                    return new Window(new LoginPage());
                }

                if (service.UserInRole("Student"))
                {
                    return new Window(new StudentShell());
                }

                return new Window(new AppShell());
            }
            catch (Exception ex)
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current!.Windows[0].Page.DisplayAlert(
                        "Error",
                        ex.ToString(),
                        "OK");
                });

                return new Window(new ContentPage
                {
                    Content = new Label
                    {
                        Text = ex.ToString()
                    }
                });
            }
        }
    }
}