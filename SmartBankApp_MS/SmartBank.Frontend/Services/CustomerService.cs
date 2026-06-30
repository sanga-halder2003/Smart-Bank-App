using System.Net.Http.Json;
using SmartBank.Frontend.Models.Customer;

namespace SmartBank.Frontend.Services
{
    public class CustomerService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public CustomerService(HttpClient httpClient,
                               IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        private string BaseUrl =>
            $"{_configuration["Gateway:BaseUrl"]}/api/customer";

        public async Task<List<CustomerModel>> GetAllCustomersAsync()
        {
            var result = await _httpClient.GetFromJsonAsync<List<CustomerModel>>(BaseUrl);

            return result ?? new List<CustomerModel>();
        }

        public async Task<CustomerModel?> GetCustomerByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<CustomerModel>($"{BaseUrl}/{id}");
        }

        public async Task<HttpResponseMessage> CreateCustomerAsync(CustomerModel customer)
        {
            return await _httpClient.PostAsJsonAsync(BaseUrl, customer);
        }

        public async Task<HttpResponseMessage> UpdateCustomerAsync(CustomerModel customer)
        {
            return await _httpClient.PutAsJsonAsync($"{BaseUrl}/{customer.Id}", customer);
        }

        public async Task<HttpResponseMessage> DeleteCustomerAsync(int id)
        {
            return await _httpClient.DeleteAsync($"{BaseUrl}/{id}");
        }
    }
}