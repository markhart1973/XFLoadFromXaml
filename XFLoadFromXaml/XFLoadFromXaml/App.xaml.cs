using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FreshMvvm;
using XFLoadFromXaml.Infrastructure.Services;
using XFLoadFromXaml.PageModels;
using Xamarin.Forms;

namespace XFLoadFromXaml
{
    public partial class App : Application
    {
        public App()
        {
            SetupIoc();

            var page = FreshPageModelResolver.ResolvePageModel<MainPageModel>(null);
            var nav = new FreshNavigationContainer(page);
            this.MainPage = nav;
        }

        private static void SetupIoc()
        {
            FreshIOC.Container.Register<IPageService, PageService>();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
