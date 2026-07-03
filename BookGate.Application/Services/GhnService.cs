using Microsoft.Extensions.Configuration; // Sửa lỗi IConfiguration
using System.Net.Http;
using System.Text; // Sửa lỗi StringContent và Encoding
using System.Text.Json; // Thay thế cho Newtonsoft

namespace BookGate.Application.Services
{
    public class GhnService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GhnService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpClient.DefaultRequestHeaders.Add("Token", _configuration["GHN:Token"]);
            string baseUrl = _configuration["GHN:BaseUrl"];
            if (!baseUrl.EndsWith("/")) baseUrl += "/";

            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        // 1. Hàm lấy danh sách Tỉnh/Thành
        public async Task<string> GetProvincesAsync()
        {
            var response = await _httpClient.GetAsync("master-data/province");
            return await response.Content.ReadAsStringAsync();
        }

        // 2. Hàm lấy danh sách Quận/Huyện theo ID Tỉnh
        public async Task<string> GetDistrictsAsync(int provinceId)
        {
            var payload = new { province_id = provinceId };
            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("master-data/district", content);
            return await response.Content.ReadAsStringAsync();
        }

        // 3. Hàm lấy danh sách Phường/Xã theo ID Huyện
        public async Task<string> GetWardsAsync(int districtId)
        {
            var payload = new { district_id = districtId };
            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("master-data/ward", content);
            return await response.Content.ReadAsStringAsync();
        }

        // 4. Hàm tính phí ship tự động
        public async Task<decimal> CalculateShippingFeeAsync(int toDistrictId, string toWardCode, int weightInGrams)
        {
            try
            {
                var payload = new
                {
                    shop_id = int.Parse(_configuration["GHN:ShopId"]),
                    from_district_id = 3440,
                    to_district_id = toDistrictId, 
                    to_ward_code = toWardCode, 
                    weight = weightInGrams, 
                    service_type_id = 2            // Loại dịch vụ: 2 = Đi chuẩn
                };

                var jsonPayload = JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                // Ép thêm mã ShopId vào Header theo yêu cầu của GHN
                _httpClient.DefaultRequestHeaders.Remove("ShopId");
                _httpClient.DefaultRequestHeaders.Add("ShopId", _configuration["GHN:ShopId"]);

                var response = await _httpClient.PostAsync("v2/shipping-order/fee", content);
                var resultString = await response.Content.ReadAsStringAsync();

                // Đọc JSON bằng System.Text.Json
                using (JsonDocument doc = JsonDocument.Parse(resultString))
                {
                    var root = doc.RootElement;
                    if (root.TryGetProperty("code", out JsonElement codeElement) && codeElement.GetInt32() == 200)
                    {
                        if (root.TryGetProperty("data", out JsonElement dataElement) &&
                            dataElement.TryGetProperty("total", out JsonElement totalElement))
                        {
                            return totalElement.GetDecimal(); // Trả về số tiền ship thành công!
                        }
                    }
                }
            }
            catch
            {

            }

            return 32000; 
        }
    }
}