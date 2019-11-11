using GoogleMapsDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GoogleMapsDemo.Services
{
    public class NavigationService : INavigationService
    {
        protected Application CurrentApplication => Application.Current;
        //public Task InitializeAsync()
        //{
        //    return NavigateToAsync<ViewModels.MenuLeft.MenuViewModel>();
        //}
        public async Task NavigateToAsync(Type viewModelType, NavigationParameters parameters, Type page = null)
        {
            var view = FindViewByViewModel(viewModelType, page);

            if (view is Views.MenuLeft.MenuView)
            {
                CurrentApplication.MainPage = new CustomNavigationPage(view);
            }
            //else if (view != CurrentApplication.MainPage)
            //{
            //    await CurrentApplication.MainPage.Navigation.PushAsync(view);
            //}
            else if (CurrentApplication.MainPage is CustomNavigationPage customNavigation)
            {
                await customNavigation.PushAsync(view);
            }
            else
            {
                CurrentApplication.MainPage = new CustomNavigationPage(view);
            }

            if (view.BindingContext is ViewModelBase vm)
            {
                await vm.OnNavigationAsync(parameters, NavigationType.New);
            }
        }
        public async Task NavigateBackAsync(NavigationParameters parameters)
        {
            if (CurrentApplication.MainPage is CustomNavigationPage navigationPage)
            {
                await navigationPage.PopAsync();

                if (navigationPage.Navigation.NavigationStack.LastOrDefault() is Page view)
                    if (view.BindingContext is ViewModelBase vm)
                    {
                        await vm.OnNavigationAsync(parameters, NavigationType.Back);
                    }
            }
        }
        public async Task NavigateBackToMainPageAsync()
        {
            if (!(CurrentApplication.MainPage is CustomNavigationPage))
                return;

            for (var i = CurrentApplication.MainPage.Navigation.NavigationStack.Count - 2; i > 0; i--)
                CurrentApplication.MainPage?.Navigation.RemovePage(CurrentApplication.MainPage.Navigation
                    .NavigationStack[i]);

            await CurrentApplication.MainPage.Navigation.PopAsync();
        }
        public Task NavigateBackAsync()
        {
            return NavigateBackAsync(null);
        }
        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return NavigateToAsync(typeof(TViewModel), new NavigationParameters());
        }
        public Task NavigateToAsync<TViewModel>(NavigationParameters parameters) where TViewModel : ViewModelBase
        {
            return NavigateToAsync(typeof(TViewModel), parameters);
        }
        public Task NavigateToAsync<TViewModel, TPage>(NavigationParameters parameters) where TViewModel : ViewModelBase
        {
            return NavigateToAsync(typeof(TViewModel), parameters, typeof(TPage));
        }
        public Task NavigateToAsync(Type viewModelType)
        {
            return NavigateToAsync(viewModelType, new NavigationParameters());
        }
        public async Task NavigateToAsyncNotif(List<Type> listPage, NavigationParameters parameters = null)
        {
            try
            {

                if(!App.IsFirstLoad)
                {
                    await PushPage(0, listPage, parameters);
                    return;
                }

                if (CurrentApplication.MainPage.Navigation.NavigationStack.Count > 1)
                {
                    await CurrentApplication.MainPage.Navigation.PopToRootAsync();
                    await PushPage(1, listPage, parameters);
                    return;
                }
                else if (CurrentApplication.MainPage.Navigation.NavigationStack.Count == 1 && CurrentApplication.MainPage is CustomNavigationPage customNavigation)
                {
                    await PushPage(1, listPage, parameters);
                    return;
                }


            }
            catch (Exception ex)
            {
                Debugger.Break();
            }
        }
        public async Task NavigateBackAsyncWithPage(Type viewModelType, NavigationParameters parameters = null, int pageIndex = 1)
        {
            if (CurrentApplication.MainPage is CustomNavigationPage navigationPage)
            {
                await NavigateToAsync(viewModelType, parameters);

                for (int i = 0; i < pageIndex; i++)
                {
                    navigationPage.Navigation.RemovePage(navigationPage.Navigation.NavigationStack[navigationPage.Navigation.NavigationStack.Count() - 2]);
                }
            }
        }
        protected Page FindViewByViewModel(Type viewModelType, Type page = null)
        {
            try
            {
                Type viewType;

                if (page == null)
                {
                    viewType = Type.GetType(viewModelType.FullName.Replace("ViewModel", "View"));

                    if (viewType == null)
                        throw new Exception($"Mapping type for {viewModelType} is not a page");
                }
                else
                {
                    viewType = page;
                }


                var view = Activator.CreateInstance(viewType) as Page;

                if (view != null)
                {
                    view.BindingContext = ServiceLocator.Instance.Resolve(viewModelType);
                }

                return view;
            }
            catch (Exception ex)
            {
                Debugger.Break();

                throw;
            }
        }
        private async Task PushPage(int index, List<Type> _page, NavigationParameters parameters = null)
        {
            for (int i = index; i < _page.Count(); i++)
            {
                await NavigateToAsync(_page[i], parameters);
            }
        }
    }
}
