using System;
using Xamarin.Forms;

namespace UnderCurrent
{
	public class App : Application
	{
		static ActivityIndicator indicator = new ActivityIndicator { Color = new Color(.5) };

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
			var getStartedButton = new Button { Text = "Click to get started" };

			layout.Children.Add(welcomeText);
			layout.Children.Add(getStartedButton);
			layout.Children.Add(indicator);

			getStartedButton.Clicked += getStartedButtonClicked;


			return new ContentPage
			{

				Content = layout

			};
		}

		protected static Tile[] getTiles()
		{
			var tileService = new TileService();
			return tileService.GetTilesAsync().Result;

		}

		protected static void getStartedButtonClicked(object sender, EventArgs e)
		{
			indicator.IsVisible = true;
			indicator.IsRunning = true;
			indicator.Focus();

			//indicator.SetBinding(ActivityIndicator.IsRunningProperty, "IsLoading");

			Tile[] tiles = getTiles();

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

			indicator.IsRunning = false;
			indicator.Unfocus();
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

