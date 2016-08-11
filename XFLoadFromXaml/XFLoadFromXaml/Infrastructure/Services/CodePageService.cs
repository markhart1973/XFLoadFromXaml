using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XFLoadFromXaml.Infrastructure.Services
{
    public class CodePageService :
        BasePageService,
        IPageService
    {
        public async Task<ContentView> GetContent()
        {
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

            content.Content = stackLayout;

            var stream = this
                .GetType()
                .GetTypeInfo()
                .Assembly
                .GetManifestResourceStream(JsonNs);
            var json = await new StreamReader(stream)
                .ReadToEndAsync();
            var model = JsonModel.Parse(json);

            content.BindingContext = model;

            return content;
        }
    }
}