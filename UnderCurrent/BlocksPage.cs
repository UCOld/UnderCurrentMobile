using System;
using Xamarin.Forms;

namespace UnderCurrent
{
	public class BlocksPage : ContentPage
	{

		public BlocksPage(Block[] blocks)
		{
			var layout = new StackLayout
			{
				// Accomodate iphone status bar
				Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5),
				Spacing = 20,
			};
			layout.Children.Add(new Label { Text = "" });

			Title = Application.Current.Properties["username"].ToString();

			try
			{

				if (blocks != null)
				{

					foreach (Block block in blocks)
					{
						var blockButton = new Button
						{
							HorizontalOptions = LayoutOptions.Center,
							VerticalOptions = LayoutOptions.Center,
							FontFamily = ("Roboto Thin"),
							FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Button)),
							Text = block.generalBlockInfo.name
						};

						blockButton.Clicked += (sender, EventArgs) => { blockButtonClicked(sender, EventArgs, block); };

						layout.Children.Add(
							new StackLayout
							{
								HorizontalOptions = LayoutOptions.Center,
								Orientation = StackOrientation.Vertical,
								Children = {
									new Label
									{
										Text = "x: " + block.generalBlockInfo.xCoord + " y: " + block.generalBlockInfo.yCoord + " z: " + block.generalBlockInfo.zCoord,
										HorizontalOptions = LayoutOptions.Center,
										VerticalOptions = LayoutOptions.Center,
										VerticalTextAlignment = TextAlignment.End,
										FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label))
									},
									blockButton
								}
							});

					}

				}

			}
			catch (TimeoutException)
			{
				// TODO Build exception message
			}
			catch (Exception ex)
			{
				throw ex;
			}

			Content = new ScrollView()
			{
				Content = layout
			};

		}


		protected async static void blockButtonClicked(object sender, EventArgs e, Block block)

		{
			Application.Current.Properties["currentBlock"] = block.generalBlockInfo.name;
			var blockPage = new NavigationPage(new BlockPage(block));
			await Application.Current.MainPage.Navigation.PushModalAsync(blockPage);
		}

	}


}

