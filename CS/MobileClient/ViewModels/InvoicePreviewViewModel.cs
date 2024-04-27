using Client.Shared;
using DataModel;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.Maui.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MobileClient.ViewModels
{
    public class InvoicePreviewViewModel : BindableBase, IQueryAttributable {
        IReportService ReportService;
        Order baseOrder;
        public string ReportFilePath {
            get { return GetValue<string>(nameof(ReportFilePath)); }
            set { SetValue(value, nameof(ReportFilePath)); }
        }
        public bool IsReportGenerating {
            get { return GetValue<bool>(nameof(IsReportGenerating)); }
            set { SetValue(value, nameof(IsReportGenerating)); }
        }
        public ICommand ShareReportCommand {
            get;
            set;
        }
        public InvoicePreviewViewModel(IReportService reportService) {
            ReportService = reportService;
            ShareReportCommand = new Command(ShareReport);
        }
        async void IQueryAttributable.ApplyQueryAttributes(IDictionary<string, object> query) {
            object parameter;
            if (query.TryGetValue("SelectedOrder", out parameter)) {
                baseOrder = (Order)parameter;
                await GenerateReport();
            }
        }
        async Task GenerateReport() {
            IsReportGenerating = true;
            ReportFilePath = await ReportService.ExportInvoiceReportToPdfAsync(baseOrder, FileSystem.Current.AppDataDirectory);
            IsReportGenerating = false;
        }
        public async void ShareReport() {
            await Share.Default.RequestAsync(new ShareFileRequest {
                Title = "Share PDF file",
                File = new ShareFile(ReportFilePath)
            });
        }
    }
}
