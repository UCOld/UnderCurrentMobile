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
			Text = "",
			IsVisible = false,
			VerticalOptions = LayoutOptions.Center,
			HorizontalOptions = LayoutOptions.Center,
			FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
		};

		static bool authenticated;
		static Button authenticateButton;

		static StackLayout layout = new StackLayout
		{
			// Accomodate iphone status bar
			Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5),
			Spacing = 20
		};

		static Entry usernameEntry = new Entry
		{
			Placeholder = "Player Name",
			IsVisible = true,
			FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Entry)),
			VerticalOptions = LayoutOptions.EndAndExpand,
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			HorizontalTextAlignment = TextAlignment.Center,

		};

		static Entry passwordEntry = new Entry
		{
			IsPassword = true,
			Placeholder = "Secret Key",
			IsVisible = true,
			FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Entry)),
			VerticalOptions = LayoutOptions.EndAndExpand,
			HorizontalOptions = LayoutOptions.CenterAndExpand,
			HorizontalTextAlignment = TextAlignment.Center,
		};

		static StackLayout buttonLayout = new StackLayout
		{
			VerticalOptions = LayoutOptions.EndAndExpand
		};

		static StackLayout editLayout = new StackLayout
		{
			VerticalOptions = LayoutOptions.CenterAndExpand,
			Children = { usernameEntry, passwordEntry }
		};



		public static Page getMainPage()
		{

			if (!authenticated)
			{

				//Current.MainPage.IsBusy = true;

				var logo = new Image
				{
					Source = "logo.png",
					VerticalOptions = LayoutOptions.CenterAndExpand
				};

				var welcomeText1 = new Label
				{
					Text = "\n\n\nWelcome to",
					FontFamily = ("Roboto"),
					FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
					FontAttributes = FontAttributes.Italic,
					HorizontalOptions = LayoutOptions.Center
				};

				var welcomeText2 = new Label
				{
					Text = "Under Current\n",
					TextColor = Color.White,
					FontFamily = ("Roboto"),
					FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
					FontAttributes = FontAttributes.Bold,
					HorizontalOptions = LayoutOptions.Center
				};

				var textLayout = new StackLayout
				{
					Children = { welcomeText1, welcomeText2, logo }
				};

				authenticateButton = new Button
				{
					Text = "Connect",
					FontFamily = ("Roboto Thin"),
					FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
					VerticalOptions = LayoutOptions.EndAndExpand,
					HorizontalOptions = LayoutOptions.Center
				};
				buttonLayout.Children.Add(authenticateButton);

				layout.Children.Add(textLayout);
				layout.Children.Add(editLayout);
				layout.Children.Add(buttonLayout);
				layout.Children.Add(state);
				layout.Children.Add(new Label { Text = "" });

				authenticateButton.Clicked += authenticateButtonClicked;

				return new ContentPage
				{
					Content = new ScrollView
					{
						Content = layout
					}

				};
			}
			else {
				return Current.MainPage;
			}

		}

		async static Task<Block[]> getBlocks()
		{
			var getBlockService = new CoreService();
			System.Diagnostics.Debug.WriteLine("Get Block Service created");
			return await getBlockService.GetBlocksAsync();

		}

		protected async static void authenticateButtonClicked(object sender, EventArgs e)
		{
			buttonLayout.Children.Remove(authenticateButton);
			buttonLayout.Children.Add(spinner);
			spinner.IsVisible = true;
			spinner.IsRunning = true;
			if (!authenticated)
			{

				try
				{

					var authenticateService = new CoreService();
					authenticated = await authenticateService.Authenticate(usernameEntry.Text, passwordEntry.Text);

					if (authenticated)
					{

						state.Text = "You are logged in";
						//var blocks = await getBlocks();
						usernameEntry.IsEnabled = false;
						passwordEntry.IsEnabled = false;
						authenticateButton.Text = "Go to my blocks";
						var blocksPage = new NavigationPage(new BlocksPage());
						//var blocksPage = new NavigationPage(new BlocksPage(blocks));
						await Current.MainPage.Navigation.PushModalAsync(blocksPage);

					}
					else {
						System.Diagnostics.Debug.WriteLine("Could not authenticate");
						throw new UnauthorizedAccessException("Could not authenticate");
					}
				}
				catch (TimeoutException)
				{
					await Current.MainPage.DisplayAlert("Info", "Connection timed out, please try again later", "OK");

				}
				catch (System.Net.WebException)
				{
					await Current.MainPage.DisplayAlert("Info", "Could not reach server, please try again later", "OK");
				}
				catch (UnauthorizedAccessException)
				{
					await Current.MainPage.DisplayAlert("Info", "Could not connect, make sure your player name and secret key is correct and try again", "OK");
				}
				catch (Exception ex)
				{
					throw ex;
				}

			}
			else {
				usernameEntry.IsEnabled = false;
				passwordEntry.IsEnabled = false;
				authenticateButton.Text = "Go to my blocks";
				var blocksPage = new NavigationPage(new BlocksPage());
				//var blocksPage = new NavigationPage(new BlocksPage(await getBlocks()));
				await Current.MainPage.Navigation.PushModalAsync(blocksPage);

			}
			spinner.IsVisible = false;
			spinner.IsRunning = false;
			buttonLayout.Children.Add(authenticateButton);
			buttonLayout.Children.Remove(spinner);
		}

		protected override void OnStart()
		{
			if (MainPage == null)
			{

				MainPage = getMainPage();
			}
		}

		protected override void OnSleep()
		{
			
		}

		protected override void OnResume()
		{
			//MainPage.Navigation.PopToRootAsync(true);
		}
	}
}

