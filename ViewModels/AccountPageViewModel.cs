using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiFrontend.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiFrontend.ViewModels
{
    public partial class AccountPageViewModel : ObservableObject
    {
        [RelayCommand]
        async Task ToLoginPage()
        {
            await Shell.Current.GoToAsync($"///LoginPage");
        } 
    }
}
