using DataModel;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using System.Text;
using System.Net.Http;
using System.Text.Json;

namespace Client.Shared {
    public class OrderDataService : IOrderDataService {
        readonly HttpClient client;
        public OrderDataService(HttpClient httpClient) {
            client = httpClient;
        }
        public async Task<List<Order>> GetOrdersAsync() {
            List<Order> orders = null;
            try {
                HttpResponseMessage response = await client.GetAsync("api/Orders");
                if (response.IsSuccessStatusCode) {
                    orders = await response.Content.ReadAsAsync<List<Order>>();
                }
            }
            catch (Exception) {
                
            }
            return orders;
        }
    }


    public interface IOrderDataService {
        Task<List<Order>> GetOrdersAsync();
    }


}
