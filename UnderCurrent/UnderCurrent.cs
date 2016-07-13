using System;
using Xamarin.Forms;

namespace UnderCurrent
{
	public class App : Application
	{
		static ActivityIndicator indicator = new ActivityIndicator { };

		static Label state = new Label
		{
			VerticalOptions = LayoutOptions.Center,
			HorizontalOptions = LayoutOptions.Center,
			FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
		};

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
			layout.Children.Add(indicator);

			authenticateButton.Clicked += authenticateButtonClicked;
			getStartedButton.Clicked += getStartedButtonClicked;

			return new ContentPage
			{
				Content = layout

			};
		}

		protected async static void getStartedButtonClicked(object sender, EventArgs e)
		{
			indicator.IsVisible = true;
			indicator.IsRunning = true;

			if (authenticated)
			{

				var tileService = new TileService();
				var tiles = await tileService.GetTilesAsync();

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

												new Button
												{
													Text = tile.name,
													HorizontalOptions = LayoutOptions.Center,
													VerticalOptions = LayoutOptions.Center,

												}

											}


										});
								}
							}
						}
					}
				}
				getStartedButton.IsEnabled = false;
			}
			else {
				state.Text = "Please authenticate first";
			}

			indicator.IsRunning = false;
			indicator.IsRunning = false;

		}

		protected async static void authenticateButtonClicked(object sender, EventArgs e)
		{
			indicator.IsVisible = true;
			indicator.IsRunning = true;
			var tileService = new TileService();
			authenticated = await tileService.Authenticate();
			if (authenticated)
			{
				state.Text = "You are authenticated :)";
				authenticateButton.IsEnabled = false;
				getStartedButton.IsEnabled = true;
			}
			else {
				state.Text = "You are not authenticated :(";
			}
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

