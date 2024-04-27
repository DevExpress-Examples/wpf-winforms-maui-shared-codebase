using Client.Shared;
using DataModel;
using DevExpress.Mvvm;
using DevExpress.Xpf.Printing;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DesktopClient.ViewModel {
    public class OrdersViewModel : ViewModelBase {
        readonly IOrderDataService DataService;
        readonly IReportService ReportService;
        public ObservableCollection<Order> Orders {
            get { return GetProperty(() => Orders); }
            set { SetProperty(() => Orders, value); }
        }
        public Order SelectedOrder {
            get { return GetProperty(() => SelectedOrder); }
            set { SetProperty(() => SelectedOrder, value); }
        }
        public bool IsLoading {
            get { return GetProperty(() => IsLoading); }
            set { SetProperty(() => IsLoading, value); }
        }
        public ICommand ShowInvoiceReportPreviewCommand {
            get;
            set;
        }
        public OrdersViewModel(IOrderDataService dataService, IReportService reportService) {
            IsLoading = true;
            DataService = dataService;
            ReportService = reportService;
            ShowInvoiceReportPreviewCommand = new DelegateCommand(ShowInvoiceReportPreview, CanShowInvoiceReportPreview);
            InitDataLoading();
        }
        void InitDataLoading() {
            Task.Run(() => LoadOrders());
        }
        async void ShowInvoiceReportPreview() {
            IsLoading = true;
            InvoiceReport report = await ReportService.GenerateInvoiceReportAsync(SelectedOrder);
            PrintHelper.ShowRibbonPrintPreview(App.Current.MainWindow, report);
            IsLoading = false;
        }
        bool CanShowInvoiceReportPreview() {
            return SelectedOrder != null;
        }
        async Task LoadOrders() {
            IsLoading = true;
            List<Order> loadedOrders = await DataService.GetOrdersAsync();
            Orders = new ObservableCollection<Order>(loadedOrders);
            IsLoading = false;
        }
    }
}
