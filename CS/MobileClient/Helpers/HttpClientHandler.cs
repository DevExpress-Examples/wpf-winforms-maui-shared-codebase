using DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MobileClient.Services {
    public static partial class MyHttpMessageHandler {
        static readonly System.Net.Http.HttpMessageHandler PlatformHttpMessageHandler;
        public static System.Net.Http.HttpMessageHandler GetMessageHandler() => PlatformHttpMessageHandler;
    }





    public class WebAPIService {
        private static readonly HttpClient HttpClient = new(MyHttpMessageHandler.GetMessageHandler()) { Timeout = new TimeSpan(0, 0, 10) };

#if ANDROID
        //private readonly string _apiUrl = "https://10.0.2.2:7033/api/";
        private readonly string _apiUrl = "http://10.0.2.2:5072/api/";
        
#else
        private readonly string _apiUrl = "https://localhost:7033/api/";
#endif
        private const string ApplicationJson = "application/json";
        private readonly string _postEndPointUrl;

        public WebAPIService() {
            _postEndPointUrl = _apiUrl + "Orders";
        }

        public async Task<IEnumerable<Order>> GetItemsAsync(bool forceRefresh = false)
            => await RequestItemsAsync();


        private async Task<IEnumerable<Order>> RequestItemsAsync(string query = null) {
            object obj = await HttpClient.GetStringAsync($"{_postEndPointUrl}");
            return null;
        }
    }
}
