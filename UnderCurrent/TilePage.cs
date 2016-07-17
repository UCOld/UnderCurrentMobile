using Xamarin.Forms;

namespace UnderCurrent
{
    public class TilePage : ContentPage
    {
        public TilePage(Tile[] tiles)
        {

            var layout = new StackLayout();

            Entry tileValue;
            Label name;
            Label description;

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
                    }
                }

            }

        }

    }
}
