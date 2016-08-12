using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Dynamic;
using XFLoadFromXaml.Infrastructure.Controls;

namespace XFLoadFromXaml.Infrastructure.Services
{
    public class CodePageService :
        BasePageService,
        IPageService
    {
        public async Task<ContentView> GetContent()
        {
            await Task.Delay(1000);

            var modelDictionary = new DictionaryModel(new Dictionary<string, object>
            {
                {"MyTitle", "This has come from dynamic data."},
                {"MyValue", "This can be edited."},
                {"MyReason", " "},
                {"MyHideable", "This control is hidden when Number Two is selected."},
                {"MyHideableVisible", true}
            });

            var content = new ContentView();

            var stackLayout = new StackLayout();

            var label = new Label
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            label.SetBinding(Label.TextProperty, "MyTitle");
            stackLayout.Children.Add(label);

            var entry = new Entry
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            entry.SetBinding(Entry.TextProperty, "MyValue");
            stackLayout.Children.Add(entry);

            var hideable = new Label
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.Center
            };
            hideable.SetBinding(Label.TextProperty, "MyHideable");
            //hideable.SetBinding(VisualElement.IsVisibleProperty, "MyHideableVisible", BindingMode.TwoWay);

            var picker = new BindablePicker(o =>
            {
                var newItem = o as string;
                if (!string.IsNullOrEmpty(newItem))
                {
                    if (newItem == "Number Two")
                    {
                        hideable.IsVisible = false;
                    }
                    else
                    {
                        hideable.IsVisible = true;
                    }
                }
            })
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                ItemsSource = new List<string>
                {
                    " ",
                    "Number One",
                    "Number Two",
                    "Number Three"
                }
            };
            picker.SetBinding(BindablePicker.SelectedItemProperty, "MyReason", BindingMode.TwoWay);
            stackLayout.Children.Add(picker);

            stackLayout.Children.Add(hideable);

            content.Content = stackLayout;
            content.BindingContext = modelDictionary;

            return content;
        }
    }
}