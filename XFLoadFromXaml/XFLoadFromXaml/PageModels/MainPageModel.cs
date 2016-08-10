using FreshMvvm;
using Xamarin.Forms;

namespace XFLoadFromXaml.PageModels
{
    public class MainPageModel :
        FreshBasePageModel
    {
        public string DetailsText => "This is the main page. Clicking the button will load a XAML page dynamically.";

        public string LoadDynamicText => "Load Dynamic";

        public Command LoadDynamicCommand
        {
            get
            {
                return new Command(() =>
                {
                    this.CoreMethods.PushPageModel<DynamicContainerPageModel>();
                });
            }
        }
    }
}