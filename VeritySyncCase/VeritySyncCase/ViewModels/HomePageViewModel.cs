using System.Collections.ObjectModel;
using VeritySyncCase.Models;
using VeritySyncCase.Service;
using Microsoft.Maui.Controls;

namespace VeritySyncCase.ViewModels
{
    public partial class HomePageViewModel : BaseViewModel
    {
        private int _countPlugged;
        public int CountPlugged
        {
            get { return _countPlugged; }
            set { SetProperty(ref _countPlugged, value); }
        }
        private int _countSelected;
        public int CountSelected
        {
            get { return _countSelected; }
            set { SetProperty(ref _countSelected, value); }
        }
        public HomePageViewModel(INavigationService navigationService) : base(navigationService)
        {
          
        }
    }
}
