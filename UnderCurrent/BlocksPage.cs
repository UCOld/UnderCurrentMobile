using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Refractored.XamForms.PullToRefresh;
using Xamarin.Forms;

namespace UnderCurrent
{
	public class BlocksPage : ContentPage
	{
		//static Command refreshCommand = new Command(() => refresh());
		//static PullToRefreshLayout refreshView = new PullToRefreshLayout();
		static StackLayout layout = new StackLayout();
		static ScrollView scrollView = new ScrollView();

		public BlocksPage()
		{
			Title = Application.Current.Properties["username"].ToString();
			Content = scrollView;
			//Content = refreshView;
			buildView();
		}

		async static Task<Block[]> getBlocks()
		{
			var getBlockService = new CoreService();
			System.Diagnostics.Debug.WriteLine("Get Block Service created");
			return await getBlockService.GetBlocksAsync();

		}

		//protected async static void refresh()
		//{
		//	await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new BlocksPage()));
		//}

		protected async static void blockButtonClicked(object sender, EventArgs e, Block block)
		{
			Application.Current.Properties["currentBlock"] = block.generalBlockInfo.name;
			var blockPage = new NavigationPage(new BlockPage(block));
			await Application.Current.MainPage.Navigation.PushModalAsync(blockPage);
		}

		static async void buildView()
		{
			Block[] blocksArray = await getBlocks();

			List<Block> blocks = new List<Block>();
			blocks = blocksArray.ToList();
			layout = new StackLayout
			{
				// Accomodate iphone status bar
				Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5),
				Spacing = 20
			};
			layout.Children.Add(new Label { Text = "" });

			scrollView.VerticalOptions = LayoutOptions.FillAndExpand;
			scrollView.HorizontalOptions = LayoutOptions.FillAndExpand;
			scrollView.Content = layout;

			//refreshView.VerticalOptions = LayoutOptions.FillAndExpand;
			//refreshView.HorizontalOptions = LayoutOptions.FillAndExpand;
			//refreshView.RefreshColor = Color.FromHex("#3498db");
			//refreshView.RefreshCommand = refreshCommand;
			//refreshView.Content = scrollView;

			if (blocks != null)
			{
				var orderedBlocks = blocks.OrderByDescending(x => x.generalBlockInfo.lastModified).ToList();
				//blocks.OrderByDescending(new Comparison<Block>((x, y) => x.generalBlockInfo.lastModified), y.generalBlockInfo.lastModified));
				//blocks.Sort(new Comparison<Block>((x, y) => DateTime.Compare(DateTime.Parse("1970-01-01T00:00:00").Add(TimeSpan.FromMilliseconds(double.Parse(x.generalBlockInfo.lastModified))), DateTime.Parse("1970-01-01T00:00:00").Add(TimeSpan.FromMilliseconds(double.Parse(y.generalBlockInfo.lastModified))))));

				foreach (Block block in orderedBlocks)
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
	}


}

