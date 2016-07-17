using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UnderCurrent
{
	public class App : Application
	{

		static ActivityIndicator spinner = new ActivityIndicator
        {
            VerticalOptions = LayoutOptions.Center,
            HorizontalOptions = LayoutOptions.Center
        };

		static Label state = new Label
		{
			VerticalOptions = LayoutOptions.Center,
			HorizontalOptions = LayoutOptions.Center,
			FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
		};

        static Tile[] tiles;

        static Button authenticateButton, getStartedButton;

		static bool authenticated;

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

			authenticateButton = new Button { Text = "Authenticate me!" };

			getStartedButton = new Button { Text = "Let's get started!", IsEnabled = false };

			layout.Children.Add(welcomeText);
			layout.Children.Add(authenticateButton);
			layout.Children.Add(state);
			layout.Children.Add(getStartedButton);
			layout.Children.Add(spinner);

			authenticateButton.Clicked += authenticateButtonClicked;
			getStartedButton.Clicked += getStartedButtonClicked;
			

			return new ContentPage
			{
				Content = layout

			};
		}

		protected async static void getStartedButtonClicked(object sender, EventArgs e)
		{
            spinner.IsVisible = true;
            spinner.IsRunning = true;

            if (authenticated)
			{
                try
                {

                    var tileService = new TileService();
                    tiles = await tileService.GetTilesAsync();

                    if (tiles != null)
                    {

                        foreach (Tile tile in tiles)
                        {

                            foreach (TileDefinition tileDefinition in tile.editableFields)
                            {
                                foreach (Collection collections in tileDefinition.collections)
                                {
                                    foreach (Field field in collections.editableFields)
                                    {

                                        var tileButton = new Button
                                        {
                                            HorizontalOptions = LayoutOptions.Center,
                                            VerticalOptions = LayoutOptions.Center,
                                            Text = tile.name
                                        };

                                        layout.Children.Add(
                                            new StackLayout()
                                            {
                                                HorizontalOptions = LayoutOptions.Center,
                                                Orientation = StackOrientation.Vertical,
                                                Children = {
                                                    new Label
                                                    {
                                                        Text = "x: " + tile.xCoord + " y: " + tile.yCoord + " z: " + tile.zCoord,
                                                        HorizontalOptions = LayoutOptions.Center,
                                                        VerticalOptions = LayoutOptions.Center,
                                                        VerticalTextAlignment = TextAlignment.End,
                                                        FontSize = Device.GetNamedSize(NamedSize.Micro, typeof(Label)),
                                                    },
                                                    tileButton
                                                }
                                            });

                                        tileButton.Clicked += tileButtonClicked;

                                    }
                                }
                            }
                        }

                    }
                    getStartedButton.IsEnabled = false;

                }
                catch (TimeoutException)
                {
                    await App.Current.MainPage.DisplayAlert("Info", "Could not get information, please try again later", "OK");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    spinner.IsRunning = false;
                    spinner.IsRunning = false;
                }
            }
			else {
				state.Text = "Please authenticate first";
			}

		}

		private async static void tileButtonClicked(object sender, EventArgs e)
		{
			var tilePage = new NavigationPage(new TilePage(tiles));
            await App.Current.MainPage.Navigation.PushModalAsync(tilePage);
        }

		protected async static void authenticateButtonClicked(object sender, EventArgs e)
		{
            spinner.IsVisible = true;
            spinner.IsRunning = true;
            try
            {
                var tileService = new TileService();
                authenticated = await tileService.Authenticate();
                if (authenticated)
                {
                    state.Text = "You are authenticated :)";
                    authenticateButton.IsEnabled = false;
                    getStartedButton.IsEnabled = true;
                }
                else
                {
                    state.Text = "You are not authenticated :(";
                }
                
            } catch (TimeoutException)
            {
                await App.Current.MainPage.DisplayAlert("Info", "Could not authenticate, please try again later", "OK");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                spinner.IsVisible = false;
                spinner.IsRunning = false;
            }
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

