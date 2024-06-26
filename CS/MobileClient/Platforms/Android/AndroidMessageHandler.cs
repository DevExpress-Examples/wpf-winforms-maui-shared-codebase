﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileClient.Platforms.Android;

namespace MobileClient.Platforms.Android {
    class AndroidMessageHandler : Xamarin.Android.Net.AndroidMessageHandler {
        public AndroidMessageHandler()
           => ServerCertificateCustomValidationCallback = (_, cert, _, errors)
              => cert is { Issuer: "CN=localhost" } || errors == System.Net.Security.SslPolicyErrors.None;

        protected override Javax.Net.Ssl.IHostnameVerifier GetSSLHostnameVerifier(Javax.Net.Ssl.HttpsURLConnection connection)
           => new HostnameVerifier();

        private sealed class HostnameVerifier : Java.Lang.Object, Javax.Net.Ssl.IHostnameVerifier {
            public bool Verify(string hostname, Javax.Net.Ssl.ISSLSession session)
               => Javax.Net.Ssl.HttpsURLConnection.DefaultHostnameVerifier!.Verify(hostname, session) ||
               hostname == "10.0.2.2" && session.PeerPrincipal?.Name == "CN=localhost";
        }
    }
}

namespace MobileClient.Services {
    public static partial class MyHttpMessageHandler {
        static MyHttpMessageHandler()
           => PlatformHttpMessageHandler = new AndroidMessageHandler();
    }
}


//namespace MobileClient.Platforms.Android {
//    internal class AndroidMessageHandler {
//    }
//}
