using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiFrontend.ViewModels
{
    public partial class GlobalViewModel : ObservableObject
    {
        private bool _isGlobalLoading;
        private string _loadingMessage = "";
        public bool IsGlobalLoading
        {
            get => _isGlobalLoading;
            set => SetProperty(ref _isGlobalLoading, value);
        }

        public string LoadingMessage
        {
            get => _loadingMessage;
            set => SetProperty(ref _loadingMessage, value);
        }
    }
}
