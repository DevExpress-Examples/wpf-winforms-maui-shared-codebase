using Client.Shared;
using DataModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using DevExpress.Office.Utils;
using DevExpress.Utils.MVVM.Services;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WinFormsDesktopClient.ViewModels {
    public class OrdersViewModel {
        readonly IOrderDataService DataService;
        readonly IReportService ReportService;
        public virtual ObservableCollection<Order> Orders { get; set; }
        public virtual Order SelectedOrder { get; set; }
        public virtual bool IsLoading { get; set; }
        protected ISplashScreenService OrdersSplashScreenService => this.GetService<ISplashScreenService>();

        public OrdersViewModel(IOrderDataService dataService, IReportService reportService) {
            DataService = dataService;
            ReportService = reportService;
            InitDataLoading();
        }
        protected void OnSelectedOrderChanged() {
            this.RaiseCanExecuteChanged(x => x.ShowInvoiceReportPreview());
        }
        async void InitDataLoading() {
            OrdersSplashScreenService.ShowSplashScreen("CrmWaitForm");
            Orders = new ObservableCollection<Order>(await DataService.GetOrdersAsync());
            OrdersSplashScreenService.HideSplashScreen();
        }
        public async void ShowInvoiceReportPreview() {
            OrdersSplashScreenService.ShowSplashScreen("CrmWaitForm");
            InvoiceReport report = await ReportService.GenerateInvoiceReportAsync(SelectedOrder);
            ReportPrintTool printTool = new ReportPrintTool(report);
            printTool.ShowRibbonPreview();
            OrdersSplashScreenService.HideSplashScreen();
        }
        public bool CanShowInvoiceReportPreview() {
            return SelectedOrder != null;
        }
    }
}
