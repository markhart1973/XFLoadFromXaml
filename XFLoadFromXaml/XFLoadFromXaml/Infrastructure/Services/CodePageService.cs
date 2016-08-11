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

            var picker = new BindablePicker
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

            content.Content = stackLayout;

            //var stream = this
            //    .GetType()
            //    .GetTypeInfo()
            //    .Assembly
            //    .GetManifestResourceStream(JsonNs);
            //var json = await new StreamReader(stream)
            //    .ReadToEndAsync();
            //var model = JsonModel.Parse(json);

            var model = new DictionaryModel(new Dictionary<string, object>
            {
                {"MyTitle", "This has come from dynamic data."},
                {"MyValue", "This can be edited."},
                {"MyReason", " "}
            });

            content.BindingContext = model;

            return content;
        }
    }
}