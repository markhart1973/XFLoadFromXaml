using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XFLoadFromXaml.Infrastructure.Services
{
    public class XamlPageService :
        BasePageService,
        IPageService
    {
        public async Task<ContentView> GetContent()
        {
            // This would typically go to a remote service maybe?
            await Task.Delay(1000);

            var content = new ContentView();

            var stream = this
                .GetType()
                .GetTypeInfo()
                .Assembly
                .GetManifestResourceStream(XamlNs);
            var xaml = await new StreamReader(stream)
                .ReadToEndAsync();
            content.LoadFromXaml(xaml);

            stream = this
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