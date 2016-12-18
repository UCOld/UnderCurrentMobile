using System.Collections.Generic;
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

			Entry tileValue;
			Label name;
			Label description;

			List<EditableField> currentEditableFields = new List<EditableField>();

			currentEditableFields = block.editableFields;

			if (currentEditableFields != null)
			{

				foreach (EditableField field in currentEditableFields)
				{

					name = new Label
					{
						Text = field.displayName
					};

					description = new Label
					{
						Text = field.displayDescription
					};

					layout.Children.Add(name);
					layout.Children.Add(description);
					switch (field.editorType)
					{
						case "STRING":

							tileValue = new Entry
							{
								Placeholder = field.fieldValue
							};

							layout.Children.Add(tileValue);

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

							layout.Children.Add(label);
							layout.Children.Add(stepper);

							break;

						case "BOOLEAN":

							var tableView = new TableView
							{
								Intent = TableIntent.Form,
								Root = new TableRoot
										{
											new TableSection
											{
												new SwitchCell
												{
													Text = field.fieldName,
													On = bool.Parse(field.fieldValue)
												}
											}
										}
							};

							layout.Children.Add(tableView);

							break;
					}

				}

			}
			Content = new ScrollView()
			{
				Content = layout
			};
		}

	}

}

