using GoogleMapsDemo.Mvvm;
using GoogleMapsDemo.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsDemo.ViewModels
{
    public class ViewModelBase : BindableBase
    {
        protected INavigationService NavigationService { get; }
        private string _title;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value, () => RaisePropertyChanged(nameof(IsNotBusy)));
        }

        public bool IsNotBusy => !IsBusy;

        protected bool IsPhanTrang { get; set; }

        private bool _isLoading = false;
        public bool IsLoading
        {
            get { return _isLoading; }
            set { SetProperty(ref _isLoading, value); }
        }

        private bool _isLoadingInit = false;
        public bool IsLoadingInit
        {
            get { return _isLoadingInit; }
            set { SetProperty(ref _isLoadingInit, value); }
        }

        private bool _isHaveData;
        public bool IsHaveData
        {
            get { return _isHaveData; }
            set { SetProperty(ref _isHaveData, value); }
        }

        private bool _isLoadingMore;
        public bool IsLoadingMore
        {
            get { return _isLoadingMore; }
            set { SetProperty(ref _isLoadingMore, value); }
        }


        public ViewModelBase()
        {
            NavigationService = ServiceLocator.Instance.Resolve<INavigationService>();
        }

        public virtual Task OnNavigationAsync(NavigationParameters parameters, NavigationType navigationType)
        {
            return Task.CompletedTask;
        }

        public virtual Task OnleavePage()
        {
            return Task.CompletedTask;
        }



        //private DelegateCommand _backCommand;
        //public DelegateCommand BackCommand => _backCommand ?? (_backCommand = new DelegateCommand(Back));
        //private async void Back()
        //{
        //    await NavigationService.NavigateBackAsync();
        //}
    }
}
