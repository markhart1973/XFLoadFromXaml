
using System;
using FreshMvvm;
using XFLoadFromXaml.Infrastructure.Services;
using Xamarin.Forms;
using Xamarin.Forms.Dynamic;

namespace XFLoadFromXaml.PageModels
{
    public class DynamicContainerPageModel :
        FreshBasePageModel
    {
        private readonly IPageService _pageService;
        private DictionaryModel _boundData;

        public string DynamicTitle => "Dynamic Page";

        private View _dynamicView;

        public View DynamicView
        {
            get
            {
                return _dynamicView;
            }
            set
            {
                _dynamicView = value;
                this.RaisePropertyChanged();
            }
        }

        public DynamicContainerPageModel(IPageService pageService)
        {
            _pageService = pageService;
        }

        public override async void Init(object initData)
        {
            base.Init(initData);
            this.DynamicView = await _pageService.GetContent();

            _boundData = this.DynamicView.BindingContext as DictionaryModel;
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            // We can get the values back out of the form from here.
            base.ViewIsDisappearing(sender, e);
        }
    }
}