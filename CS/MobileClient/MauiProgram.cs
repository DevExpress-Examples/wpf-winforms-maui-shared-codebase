using Client.Shared;
using DataModel;
using DevExpress.Maui;
using DevExpress.Maui.Core;
using MobileClient.Services;
using MobileClient.ViewModels;
using MobileClient.Views;

namespace MobileClient {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            ThemeManager.ApplyThemeToSystemBars = true;
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .RegisterViewModels()
                .RegisterViews()
                .RegisterAppServices()
                .UseDevExpress(useLocalization: true)
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("univia-pro-regular.ttf", "Univia-Pro");
                    fonts.AddFont("roboto-bold.ttf", "Roboto-Bold");
                    fonts.AddFont("roboto-regular.ttf", "Roboto");
                });
            DevExpress.Utils.DeserializationSettings.RegisterTrustedClass(typeof(Order));
            return builder.Build();
        }
        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder) {
            mauiAppBuilder.Services.AddTransient<OrdersViewModel>();
            mauiAppBuilder.Services.AddTransient<InvoicePreviewViewModel>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder) {
            mauiAppBuilder.Services.AddTransient<OrdersPage>();
            mauiAppBuilder.Services.AddTransient<InvoiceReportPreviewPage>();
            return mauiAppBuilder;
        }
        public static MauiAppBuilder RegisterAppServices(this MauiAppBuilder mauiAppBuilder) {
            mauiAppBuilder.Services.AddTransient<IOrderDataService>(sp => new OrderDataService(new HttpClient(MyHttpMessageHandler.GetMessageHandler()) {
                BaseAddress = new Uri(ON.Platform(android: "https://10.0.2.2:7033/", iOS: "https://localhost:7033/")),
                Timeout = new TimeSpan(0, 0, 10)
            })); ;
            mauiAppBuilder.Services.AddTransient<IReportService, ReportService>();
            mauiAppBuilder.Services.AddTransient<INavigationService, NavigationService>();
            return mauiAppBuilder;
        }
    }
}
