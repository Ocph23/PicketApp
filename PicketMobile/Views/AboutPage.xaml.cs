namespace PicketMobile.Views;

public partial class AboutPage : ContentPage
{
	public AboutPage()
	{
		InitializeComponent();
		creator.Text = $"Ocph23 -@{DateTime.Now.Year}";
	}
}