using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace UnderCurrent
{
	public class BlockPage : ContentPage
	{
		
		public BlockPage(Block block)
		{
			Title = Application.Current.Properties["currentBlock"].ToString();

			var layout = new StackLayout
			{
				// Accomodate iphone status bar
				Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5),
				Spacing = 20,
			};
			layout.Children.Add(new Label { Text = "" });

			Entry stringEntry;
			Label name;
			Label description;

			List<EditableField> currentEditableFields = new List<EditableField>();

			currentEditableFields = block.editableFields;

			if (currentEditableFields != null)
			{

				foreach (EditableField field in currentEditableFields)
				{
					var success = new bool();
					name = new Label
					{
						Text = field.displayName,
						TextColor = Color.White,
						FontFamily = ("Roboto"),
						FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
						FontAttributes = FontAttributes.Bold,
						HorizontalOptions = LayoutOptions.Center
					};

					description = new Label
					{
						Text = field.displayDescription,
						FontFamily = ("Roboto"),
						FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)),
						FontAttributes = FontAttributes.Italic,
						HorizontalOptions = LayoutOptions.Center
					};

					layout.Children.Add(name);
					layout.Children.Add(description);
					switch (field.editorType)
					{
						case "STRING":

							stringEntry = new Entry
							{
								Placeholder = field.fieldValue
							};

							layout.Children.Add(stringEntry);
							stringEntry.Unfocused += async (sender, EventArgs) =>
							{
								success = await updatedEvent(block.generalBlockInfo.internalName, field.fieldName, field.fieldValue, stringEntry.Text);
								if (success)
								{
									field.fieldValue = stringEntry.Text;
								}
								else {
									stringEntry.Text = field.fieldValue;
								}
							};
							break;

						case "INTEGER":

							var label = new Label
							{
								Text = field.fieldValue
							};

							var stepper = new Stepper
							{
								Value = double.Parse(field.fieldValue),
								Minimum = double.Parse(field.minValue),
								Maximum = double.Parse(field.maxValue),
								Increment = 0.1,
								HorizontalOptions = LayoutOptions.Center,
								VerticalOptions = LayoutOptions.CenterAndExpand
							};

							stepper.ValueChanged += (sender, args) => { label.Text = stepper.Value.ToString(); };
							stepper.Unfocused += async (sender, EventArgs) =>
							{
								success = await updatedEvent(block.generalBlockInfo.internalName, field.fieldName, field.fieldValue, stepper.Value.ToString());
								if (success)
								{
									field.fieldValue = stepper.Value.ToString();
								}
								else
								{
									stepper.Value = double.Parse(field.fieldValue);
								}
							};
							layout.Children.Add(label);
							layout.Children.Add(stepper);

							break;

						case "BOOLEAN":

							var switchCell = new SwitchCell()
							{
								Text = field.fieldName,
								On = bool.Parse(field.fieldValue)
							};

							var tableView = new TableView
							{
								HorizontalOptions = LayoutOptions.CenterAndExpand,
								Intent = TableIntent.Form,
								Root = new TableRoot
										{
											new TableSection
											{
												switchCell
											}
										}
							};


							switchCell.OnChanged += async (sender, EventArgs) =>
							{
								success = await updatedEvent(block.generalBlockInfo.internalName, field.fieldName, field.fieldValue, switchCell.On.ToString());
								if (success)
								{
									field.fieldValue = switchCell.On.ToString();
								}
								else {
									switchCell.On = bool.Parse(field.fieldValue);
								}
							};
							System.Diagnostics.Debug.WriteLine("Success: " + success);

							layout.Children.Add(tableView);

							break;
					}

				}


			}



			var scrollView = new ScrollView
			{
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Content = layout
			};

			Content = scrollView;



		}




		protected async static Task<bool> updatedEvent(string internalName, string fieldName, string oldFieldValue, string fieldValue)
		{
			System.Diagnostics.Debug.WriteLine("Internal Name: " + internalName);
			System.Diagnostics.Debug.WriteLine("Field Name: " + fieldName);
			System.Diagnostics.Debug.WriteLine("Old Field Value: " + oldFieldValue);
			System.Diagnostics.Debug.WriteLine("Field Value: " + fieldValue);
			if (!oldFieldValue.Equals(fieldValue))
			{
				var updateService = new CoreService();
				var response = await updateService.UpdateBlock(internalName, fieldName, fieldValue);
				if (response)
				{
					System.Diagnostics.Debug.WriteLine(fieldName + " data saved successfully");
				}
				else {
					System.Diagnostics.Debug.WriteLine(fieldName + " data not saved");
				}
				return response;

			}
			return false;

		}

	}

}

