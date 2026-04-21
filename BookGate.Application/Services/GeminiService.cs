using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace BookGate.Application.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        // THÊM BIẾN bookContext VÀO ĐÂY
        public async Task<string> ChatWithAI(string userMessage, string bookContext)
        {
            try
            {
                // Cài đặt "Nhân cách" và "Kiến thức" cho AI
                string systemPrompt = $@"Bạn là nhân viên tư vấn sách xuất sắc của cửa hàng BookGate.
Dưới đây là danh sách các cuốn sách ĐANG CÓ SẴN tại cửa hàng của chúng tôi:
{bookContext}

YÊU CẦU BẮT BUỘC ĐỐI VỚI BẠN:
1. Khi khách yêu cầu gợi ý sách, BẠN CHỈ ĐƯỢC PHÉP đề xuất những cuốn sách có mặt trong danh sách trên. TUYỆT ĐỐI KHÔNG bịa ra sách hoặc lấy sách trên mạng.
2. Hãy tư vấn ngắn gọn, thân thiện, kèm theo Tên sách, Tác giả và Giá tiền để khách dễ mua.
3. Nếu khách hỏi một cuốn sách không có trong danh sách, hãy xin lỗi khéo léo và gợi ý một cuốn sách khác cùng thể loại đang có bán ở cửa hàng.";

                string fullPrompt = systemPrompt + "\n\nKhách hàng hỏi: " + userMessage;

                var payload = new
                {
                    contents = new[]
                    {
                        new { parts = new[] { new { text = fullPrompt } } }
                    }
                };

                var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

                string apiKey = _configuration["Gemini:ApiKey"];
                string url = _configuration["Gemini:Url"] + apiKey;

                var response = await _httpClient.PostAsync(url, content);
                var resultString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return $"[LỖI TỪ GOOGLE] Mã: {response.StatusCode}. Chi tiết: {resultString}";
                }

                using (JsonDocument doc = JsonDocument.Parse(resultString))
                {
                    var root = doc.RootElement;
                    var textResponse = root.GetProperty("candidates")[0]
                                           .GetProperty("content")
                                           .GetProperty("parts")[0]
                                           .GetProperty("text").GetString();
                    return textResponse ?? "AI không trả về nội dung.";
                }
            }
            catch (Exception ex)
            {
                return $"[LỖI C#] Chi tiết: {ex.Message}";
            }
        }
    }
}