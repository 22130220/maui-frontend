using MauiFrontend.ViewModels;
using MauiFrontend.Views;

namespace MauiFrontend
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                await Shell.Current.GoToAsync(nameof(LoginPage));
            });
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }
    }
}
