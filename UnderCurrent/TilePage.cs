using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

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

                                if (field.editorType == "STRING")
                                {

                                    tileValue = new Entry {
                                        Placeholder = field.fieldValue
                                    };

                                    layout.Children.Add(tileValue);

                                } else if (field.editorType == "INTEGER")
                                {
                                    var label = new Label
                                    {
                                        Text = field.fieldValue
                                    };

                                    Stepper stepper = new Stepper
                                    {
                                        Value = Double.Parse(field.fieldValue),
                                        Minimum = Double.Parse(field.minValue),
                                        Maximum = Double.Parse(field.maxValue),
                                        Increment = 0.1,
                                        HorizontalOptions = LayoutOptions.Center,
                                        VerticalOptions = LayoutOptions.CenterAndExpand
                                    };

                                    stepper.ValueChanged += (sender, args) => { label.Text = stepper.Value.ToString(); };

                                    layout.Children.Add(label);
                                    layout.Children.Add(stepper);
                                    
                                }

                            }
                        }
                    }
                }

            }

        }

    }
}
