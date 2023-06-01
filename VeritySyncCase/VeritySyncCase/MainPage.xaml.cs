
#if WINDOWS
using VeritySyncCase.Models;
using System.Management;
using Windows.Devices.Enumeration;
using Windows.Management.Deployment;
using VeritySyncCase.Utils;
#endif

namespace VeritySyncCase;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
}
