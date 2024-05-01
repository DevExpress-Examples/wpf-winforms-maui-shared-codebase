using Autofac;
using Autofac.Features.ResolveAnything;
using Client.Shared;
using DevExpress.Utils.Design;
using DevExpress.Xpf.CodeView.Margins;
using DevExpress.Xpf.Core;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;
using System.Windows.Markup;

namespace DesktopClient {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application {
        public App() {
            CompatibilitySettings.UseLightweightThemes = true;
            ApplicationThemeHelper.ApplicationThemeName = LightweightTheme.Win11Light.Name;
        }
        IContainer container;
        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
            builder.RegisterType<ReportService>().As<IReportService>().SingleInstance();
            builder.RegisterInstance<IOrderDataService>(new OrderDataService(new HttpClient() {
                BaseAddress = new Uri("https://localhost:7033/"),
                Timeout = new TimeSpan(0, 0, 10)
            }));
            container = builder.Build();
            DISource.Resolver = (type) =>
            {
                return container.Resolve(type);
            };
        }
    }

    public class DISource : MarkupExtension {
        public static Func<Type, object> Resolver { get; set; }
        public Type Type { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider) => Resolver?.Invoke(Type);
    }

}
