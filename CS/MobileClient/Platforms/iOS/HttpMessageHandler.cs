﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services {
    public static partial class MyHttpMessageHandler {
        static MyHttpMessageHandler() {
            NSUrlSessionHandler nSUrlSessionHandler = new();
            nSUrlSessionHandler.ServerCertificateCustomValidationCallback += (_, cert, _, errors)
                => cert is { Issuer: "CN=localhost" } || errors == System.Net.Security.SslPolicyErrors.None;
            nSUrlSessionHandler.TrustOverrideForUrl = (sender, url, trust) => {
                return true;
            };
            PlatformHttpMessageHandler = nSUrlSessionHandler;
        }
    }
}
