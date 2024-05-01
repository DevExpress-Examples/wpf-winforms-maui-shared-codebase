using Autofac;
using Autofac.Features.ResolveAnything;
using Client.Shared;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using DevExpress.Utils.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Windows.Forms;
using System.Windows.Markup;

namespace WinFormsDesktopClient {
    internal static class Program {
        static IContainer container;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            builder.RegisterType<ReportService>().As<IReportService>().SingleInstance();
            builder.RegisterInstance<IOrderDataService>(new OrderDataService(new HttpClient() {
                BaseAddress = new Uri("https://localhost:7033/"),
                Timeout = new TimeSpan(0, 0, 10)
            }));
            container = builder.Build();
            MVVMContextCompositionRoot.ViewModelCreate += (s, e) => {
                e.ViewModel = container.Resolve(e.RuntimeViewModelType);
            };

            Application.Run(new OrdersForm());
        }
    }
}
