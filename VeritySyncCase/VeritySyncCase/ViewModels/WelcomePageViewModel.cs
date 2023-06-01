using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using VeritySyncCase.Service;
using VeritySyncCase.View;

namespace VeritySyncCase.ViewModels
{
    public partial class WelcomePageViewModel : BaseViewModel
    {
        public WelcomePageViewModel(INavigationService navigationService) : base(navigationService)
        {
        }
    }
}
