using MobileClient.ViewModels;

namespace MobileClient.Views;

public partial class InvoiceReportPreviewPage : ContentPage
{
    public InvoiceReportPreviewPage(InvoicePreviewViewModel viewModel) {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}