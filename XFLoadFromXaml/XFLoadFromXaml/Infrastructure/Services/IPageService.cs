using System.Threading.Tasks;
using Xamarin.Forms;

namespace XFLoadFromXaml.Infrastructure.Services
{
    public interface IPageService
    {
        Task<ContentView> GetContent();
    }
}