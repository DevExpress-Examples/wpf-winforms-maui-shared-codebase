using DataModel;
using DevExpress.DataAccess.ObjectBinding;
using DevExpress.XtraRichEdit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Shared {
    public class ReportService : IReportService {
        public async Task<string> ExportInvoiceReportToPdfAsync(Order order, string baseFolder) {
            InvoiceReport report = await GenerateInvoiceReportAsync(order);
            string resultFile = Path.Combine(baseFolder, report.Name + ".pdf");
            await report.ExportToPdfAsync(resultFile);
            return resultFile;
        }
        public async Task<InvoiceReport> GenerateInvoiceReportAsync(Order order) {
            return await Task.Run(async () =>
            {
                InvoiceReport invoiceReport = new InvoiceReport() { Name = $"Invoice_{order.Id}" };
                ObjectDataSource objectDataSource = new ObjectDataSource();
                objectDataSource.DataSource = order;
                invoiceReport.DataSource = objectDataSource;
                await invoiceReport.CreateDocumentAsync();
                return invoiceReport;
            });
        }
    }

    public interface IReportService {
        Task<InvoiceReport> GenerateInvoiceReportAsync(Order order);
        Task<string> ExportInvoiceReportToPdfAsync(Order order, string baseFolder);
    }
}
