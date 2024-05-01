using DevExpress.Dialogs.Core.View;
using DevExpress.Utils.MVVM;
using DevExpress.Utils.MVVM.Services;
using DevExpress.XtraBars.Customization;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinFormsDesktopClient.ViewModels;

namespace WinFormsDesktopClient {
    public partial class OrdersForm : DevExpress.XtraBars.Ribbon.RibbonForm {
        SplashScreenManager crmSplashScreenManager;
        public OrdersForm() {
            InitializeComponent();
            crmSplashScreenManager = new SplashScreenManager(this, typeof(CrmWaitForm), true, true);


            ordersMvvmContext.ViewModelType = typeof(OrdersViewModel);
            var fluent = ordersMvvmContext.OfType<OrdersViewModel>();
            ordersMvvmContext.RegisterService(SplashScreenService.Create(crmSplashScreenManager));
            fluent.BindCommand(invoiceReportButton, x => x.ShowInvoiceReportPreview());
            fluent.SetBinding(gridView1, gridView => gridView.FocusedRowObject, x => x.SelectedOrder);
            fluent.SetBinding(gridControl1, grid => grid.DataSource, x => x.Orders);
        }
    }
}
