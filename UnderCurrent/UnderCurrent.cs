using System;
using Xamarin.Forms;

namespace UnderCurrent
{
    public class App : Application
    {
        static ActivityIndicator indicator = new ActivityIndicator {};

        static Label error = new Label() { Text = "" };

        static bool authenticated = new bool();

        static StackLayout layout = new StackLayout
        {
            // Accomodate iphone status bar
            Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
        };

        public static Page getMainPage()
		{
			var welcomeText = new Label
			{
				Text = "Welcome to Under Current!",
				VerticalOptions = LayoutOptions.Center,
				HorizontalOptions = LayoutOptions.Center
			};

            var authenticateButton = new Button { Text = "Authenticate me!" };

            var getStartedButton = new Button { Text = "Let's get started!"};

			layout.Children.Add(welcomeText);
			layout.Children.Add(getStartedButton);
            layout.Children.Add(authenticateButton);
            layout.Children.Add(indicator);
            layout.Children.Add(error);

            getStartedButton.Clicked += getStartedButtonClicked;
            
            authenticateButton.Clicked += authenticateButtonClicked;



            return new ContentPage
			{

				Content = layout

			};
		}

		protected static Tile[] getTiles()
		{
            try
            {
                var tileService = new TileService();

                Tile[] tiles = tileService.GetTilesAsync().Result;
                return tiles;
            }
            catch (Exception e)
            {
                error.Text = e.ToString();
                return null;
            }

        }

        protected static void getStartedButtonClicked(object sender, EventArgs e)
		{
			indicator.IsVisible = true;
			indicator.IsRunning = true;

			Tile[] tiles = getTiles();

            if (tiles != null)
            {

                foreach (Tile tile in tiles)
                {

                    foreach (TileDefinition tileDefinition in tile.editableFields)
                    {
                        foreach (Collection collections in tileDefinition.collections)
                        {
                            foreach (Field field in collections.editableFields)
                                layout.Children.Add(
                                    new Button
                                    {
                                        Text = "Name: " + tile.name +
                                                       " x: " + tile.xCoord +
                                                    " y: " + tile.yCoord +
                                                    " z: " + tile.zCoord
                                    });
                        }
                    }
                }
            }

            indicator.IsRunning = false;
            indicator.Unfocus();
            
		}

        protected static void authenticateButtonClicked(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            indicator.IsRunning = true;
            var tileService = new TileService();
            authenticated = tileService.Authenticate().Result;
            indicator.IsVisible = false;
            indicator.IsRunning = false;
        }

        protected override void OnStart()
		{
			MainPage = getMainPage();

		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

