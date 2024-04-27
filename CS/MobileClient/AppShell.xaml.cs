using MobileClient.Views;

namespace MobileClient {
    public partial class AppShell : Shell {
        public AppShell() {
            InitializeComponent();
            Routing.RegisterRoute("invoiceReportPreview", typeof(InvoiceReportPreviewPage));
        }
    }
}
