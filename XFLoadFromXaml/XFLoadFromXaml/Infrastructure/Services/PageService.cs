using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XFLoadFromXaml.Infrastructure.Services
{
    public class PageService :
        IPageService
    {
        public async Task<ContentView> GetContent()
        {
            // This would typically go to a remote service maybe?
            await Task.Delay(1000);

            var content = new ContentView();

            //const string xamlNs = "XFLoadFromXaml.Pages.MyDynamicPage.xaml";
            //const string jsonNs = "XFLoadFromXaml.Pages.MyDynamicPage.json";
            const string xamlNs = "XFLoadFromXaml.Pages.MyDynamicView.xaml";
            const string jsonNs = "XFLoadFromXaml.Pages.MyDynamicView.json";

            var stream = this
                .GetType()
                .GetTypeInfo()
                .Assembly
                .GetManifestResourceStream(xamlNs);
            var xaml = await new StreamReader(stream)
                .ReadToEndAsync();
            content.LoadFromXaml(xaml);

            stream = this
                .GetType()
                .GetTypeInfo()
                .Assembly
                .GetManifestResourceStream(jsonNs);
            var json = await new StreamReader(stream)
                .ReadToEndAsync();
            var model = JsonModel.Parse(json);

            content.BindingContext = model;

            return content;
        }
    }
}