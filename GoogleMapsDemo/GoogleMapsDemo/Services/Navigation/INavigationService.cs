using GoogleMapsDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsDemo.Services
{
    public interface INavigationService
    {
        //Task InitializeAsync();

        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;

        Task NavigateToAsync<TViewModel>(NavigationParameters parameters) where TViewModel : ViewModelBase;

        Task NavigateToAsync<TViewModel, TPage>(NavigationParameters parameters = null) where TViewModel : ViewModelBase;

        Task NavigateToAsync(Type viewModelType);

        Task NavigateToAsync(Type viewModelType, NavigationParameters parameters, Type page = null);

        Task NavigateBackAsync();

        Task NavigateBackAsync(NavigationParameters parameters);

        Task NavigateBackToMainPageAsync();
        Task NavigateBackAsyncWithPage(Type viewModelType, NavigationParameters parameters = null, int pageIndex = 1);

        Task NavigateToAsyncNotif(List<Type> listPage, NavigationParameters parameters = null);
    }
}
