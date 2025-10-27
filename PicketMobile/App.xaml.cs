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
            string token = Preferences.Get(key: "token", null);
            if (string.IsNullOrEmpty(token))
            {
                return new Window(new LoginPage());
            }
            else
            {
                IAccountService service = ServiceHelper.GetService<IAccountService>()!;
                if (service.UserInRole("Student"))
                    return new Window(new StudentShell());
                else
                    return new Window(new AppShell());


            }
        }
    }
}