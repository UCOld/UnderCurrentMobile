using System;

using Xamarin.Forms;

namespace UnderCurrent
{
	public class App : Application
	{
		static ListView blockList = new ListView();

		public static Page getMainPage()
		{

			Button sendGetButton = new Button
			{
				Text = "Send GET request"
			};

			sendGetButton.Clicked += sendGetButtonClicked;
			blockList.ItemTemplate = new DataTemplate(typeof(TextCell));
			blockList.SetBinding(TextCell.TextProperty, "blockName");
			return new ContentPage
			{

				Content = new StackLayout
				{
					Children = {
					sendGetButton,
						blockList
					}
				}

			};
		}

		static async void sendGetButtonClicked(object sender, EventArgs e)
		{
			BlocksService blocksService = new BlocksService();
			Block[] blocks = await blocksService.GetBlocksAsync();
				blockList.ItemsSource = blocks;
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

