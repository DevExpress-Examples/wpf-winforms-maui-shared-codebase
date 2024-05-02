<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/792476725/23.2.5%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T1230298)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
<!-- default badges end -->

# WPF or WinForms & .NET MAUI - Share Code Between Desktop and Mobile Projects

This example uses a loosely coupled architecture to share codebase between desktop ([WPF](https://www.devexpress.com/products/net/controls/wpf/) or [WinForms](https://www.devexpress.com/products/net/controls/winforms/)) and mobile ([.NET MAUI](https://www.devexpress.com/maui/)) UI clients. The application displays orders loaded from a Web service and allows users to generate a [report](https://www.devexpress.com/subscriptions/reporting/).

![Demo Video](./img/DemoVideo.gif)

## Run Project

1. Rebuild the solution.
2. Start the WebApiService project without debugging.
3. Start the WPFDesktopClient, WinFormsDesktopClient or MobileClient project.

## Implementation Details

The following schema outlines application architecture:

![Application Architecture](./img/Architecture.jpg)

The application includes the following projects:
- **Client.Shared**. Contains client-side services and common helpers.
- **DataModel**. A model for database objects.
- **WPFDesktopClient** - WPF application.
- **WinFormsDesktopClient** - WinForms application.
- **MobileClient** - .NET MAUI application.
- **WebApiService** - a web service that handles access to a database.

> **Note**:
> Although this example does not include authentication and role-based data access, you can use the free [DevExpress Web API Service](https://www.devexpress.com/products/net/application_framework/security-web-api-service.xml) o generate a project with this capability.

### Shared Client Classes

Desktop and mobile clients reuse the following classes:

- `OrderDataService`. Incorporates logic to communicate with the Web API service.
- `ReportService`. Generates a report based on the selected order.

We use [Dependency Injection](https://community.devexpress.com/blogs/wpf/archive/2022/02/07/dependency-injection-in-a-wpf-mvvm-application.aspx) to introduce these services into desktop and mobile projects.

#### WPF

```cs
protected override void OnStartup(StartupEventArgs e) {
    base.OnStartup(e);
    var builder = new ContainerBuilder();
    builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
    builder.RegisterType<ReportService>().As<IReportService>().SingleInstance();
    builder.RegisterInstance<IOrderDataService>(new OrderDataService(new HttpClient() {
        BaseAddress = new Uri("https://localhost:7033/"),
        Timeout = new TimeSpan(0, 0, 10)
    }));
    IContainer container = builder.Build();
    DISource.Resolver = (type) =>
    {
        return container.Resolve(type);
    };
}
```

#### WinForms

```cs
static void Main() {
    //...
    ContainerBuilder builder = new ContainerBuilder();
    builder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());
    builder.RegisterType<ReportService>().As<IReportService>().SingleInstance();
    builder.RegisterInstance<IOrderDataService>(new OrderDataService(new HttpClient() {
        BaseAddress = new Uri("https://localhost:7033/"),
        Timeout = new TimeSpan(0, 0, 10)
    }));
    container = builder.Build();
    MVVMContextCompositionRoot.ViewModelCreate += (s, e) =>
    {
        e.ViewModel = container.Resolve(e.RuntimeViewModelType);
    };
    //...
}
```

#### .NET MAUI

```cs
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
```


### Shared Web API Service

The Web API service includes basic endpoints to retrieve orders from a database connected with Entity Framework Core:

```cs
public class OrdersController : ControllerBase {
    //...
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders() {
        return await _context.Orders.Include(order => order.Customer)
            .Include(order => order.Items)
            .ThenInclude(orderItem => orderItem.Product)
            .ToListAsync();
    }
}
```

### Shared Model

Both client and server sides use the same model to work with business objects.

```cs
public class Customer {
    //...
}
public class Order {
    //...
}
public class Product {
    //...
}
```


## Files to Review

- [OrderDataService.cs](./CS/Client.Shared/OrderDataService.cs)
- [ReportService.cs](./CS/Client.Shared/ReportService.cs)
- [OrdersController.cs](./CS/WebApiService/Controllers/OrdersController.cs)
- [DesktopClient/App.xaml.cs](./CS/DesktopClient/App.xaml.cs)
- [MobileClient/MauiProgram.cs](./CS/MobileClient/MauiProgram.cs)

## Documentation

- [Get Started with DevExpress Controls for .NET Multi-platform App UI](https://docs.devexpress.com/MAUI/403249/get-started/get-started)
- [Get Started with DevExpress WPF Controls](https://docs.devexpress.com/WPF/401166/dotnet-core-support/getting-started)
- [Get Started with DevExpress WinForms Controls](https://docs.devexpress.com/WindowsForms/7874/winforms-controls#getting-started)


## More Examples

- [DevExpress Mobile UI for .NET MAUI](https://github.com/DevExpress-Examples/maui-demo-app)
- [DevExpress Stocks App for .NET MAUI](https://github.com/DevExpress-Examples/maui-stocks-mini)
