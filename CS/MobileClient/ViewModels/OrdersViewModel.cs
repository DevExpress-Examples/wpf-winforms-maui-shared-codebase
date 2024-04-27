using Client.Shared;
using DataModel;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Maui.Core;
using DevExpress.XtraRichEdit.Import.OpenXml;
using MobileClient.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MobileClient.ViewModels {
    public class OrdersViewModel : BindableBase {
        readonly IOrderDataService DataService;
        readonly INavigationService NavigationService;
        public ObservableCollection<Order> Orders {
            get { return GetValue<ObservableCollection<Order>>(nameof(Orders)); }
            set { SetValue(value, nameof(Orders)); }
        }
        public Order SelectedOrder {
            get { return GetValue<Order>(nameof(SelectedOrder)); }
            set { SetValue(value, nameof(SelectedOrder)); }
        }
        public string Filter {
            get { return GetValue<string>(nameof(Filter)); }
            set { SetValue(value, nameof(Filter)); }
        }
        public ObservableCollection<FilterItem> PredefinedFilters {
            get { return GetValue<ObservableCollection<FilterItem>>(nameof(PredefinedFilters)); }
            set { SetValue(value, nameof(PredefinedFilters)); }
        }
        public ICommand NavigateToInvoiceReportCommand {
            get;
            set;
        }
        public bool IsDataLoading {
            get { return GetValue<bool>(nameof(IsDataLoading)); }
            set { SetValue(value, nameof(IsDataLoading)); }
        }
        public BindingList<FilterItem> SelectedFilters { get; set; }
        public OrdersViewModel(IOrderDataService dataService, INavigationService navigationService) {
            IsDataLoading = true;
            DataService = dataService;
            NavigationService = navigationService;
            InitDataLoading();
            PopulatePredefinedFiltersAsync();
            InitSelectedFilters();
            NavigateToInvoiceReportCommand = new Command(NavigateToInvoiceReport);
        }
        public async void NavigateToInvoiceReport() {
            var navigationParameters = new Dictionary<string, object> { { "SelectedOrder", SelectedOrder } };
            await NavigationService.GoToAsync("invoiceReportPreview", navigationParameters);
        }
        void InitSelectedFilters() {
            SelectedFilters = new BindingList<FilterItem>();
            SelectedFilters.ListChanged += SelectedFiltersChanged;
        }
        void InitDataLoading() {
            Task.Run(() => LoadOrders());
        }
        async Task LoadOrders() {
            IsDataLoading = true;
            List<Order> loadedOrders = await DataService.GetOrdersAsync();
            Orders = new ObservableCollection<Order>(loadedOrders);
            IsDataLoading = false;
        }
        void SelectedFiltersChanged(object sender, ListChangedEventArgs e) {
            if (SelectedFilters.Count > 0)
                Filter = string.Join(" AND ", SelectedFilters.Select(f => f.Filter));
            else
                Filter = string.Empty;
        }
        private void PopulatePredefinedFiltersAsync() {
            PredefinedFilters = new ObservableCollection<FilterItem>() {
            new FilterItem(){ DisplayText= "Amount > $3000", Filter = "[TotalAmount] > 3000" },
            new FilterItem(){ DisplayText= "Peinding", Filter = "[State] == 'Draft'" },
            new FilterItem(){ DisplayText= "Shipping", Filter = "[State] == 'Shipping'" },
            new FilterItem(){ DisplayText= "Paid", Filter = "[State] == 'Paid'" },
            new FilterItem(){ DisplayText= "Processed", Filter = "[State] == 'Processed'" }};
        }
    }
    public class FilterItem {
        public string DisplayText { get; set; }
        public string Filter { get; set; }
    }
}
