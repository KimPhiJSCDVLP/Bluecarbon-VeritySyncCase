using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeritySyncCase.View;

namespace VeritySyncCase.CustomControl
{
    public class BottomBarControl : ContentView
    {
        public BottomBarControl()
        {
            var stackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Colors.LightGray
            };

            var homePageButton = new Button
            {
                Text = "HomePage",
                BackgroundColor = Colors.Transparent,
                TextColor = Colors.Black,
                CornerRadius = 0,
            };
            homePageButton.Clicked += async (sender, args) =>
            {
                await Navigation.PushAsync(new WelcomePage());
            };

            var backupFilePageButton = new Button
            {
                Text = "BackupFilePage",
                BackgroundColor = Colors.Transparent,
                TextColor = Colors.Black ,
                CornerRadius = 0,
            };
            backupFilePageButton.Clicked += async (sender, args) =>
            {
                await Navigation.PushAsync(new BackupFilePage());
            };

            stackLayout.Children.Add(homePageButton);
            stackLayout.Children.Add(backupFilePageButton);

            Content = stackLayout;
        }
    }
}
