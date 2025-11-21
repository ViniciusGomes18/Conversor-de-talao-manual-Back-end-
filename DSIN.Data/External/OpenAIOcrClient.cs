using System.Text;
using System.Text.Json;
using DSIN.Business.DTOs;
using DSIN.Business.Interfaces.IServices;

namespace DSIN.Data.External
{
    public sealed class OpenAiOcrClient : IOcrClient
    {
        private readonly HttpClient _http;
        private readonly IConfiguration _config;

        private static readonly JsonSerializerOptions JsonOpts = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public OpenAiOcrClient(HttpClient http, IConfiguration config)
        {
            _http = http;
            _config = config;
        }

        public async Task<OcrExternalResultDto> AnalyzeAsync(OcrExternalRequestDto request, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.ImageBase64))
                throw new ArgumentException("ImageBase64 obrigatório.", nameof(request.ImageBase64));

            var apiKey = _config["OpenAI:ApiKey"] ?? throw new InvalidOperationException("OpenAI:ApiKey não configurado.");
            var model = _config["OpenAI:Model"] ?? "gpt-4o-mini";

            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

            var systemPrompt = OcrPrompt.Enhanced;

            var payload = new
            {
                model,
                response_format = new { type = "json_object" },
                messages = new object[]
                {
                    new { role = "system", content = systemPrompt },
                    new
                    {
                        role = "user",
                        content = new object[]
                        {
                            new { type = "text", text = "Extraia os campos do talão a partir desta imagem." },
                            new
                            {
                                type = "image_url",
                                image_url = new
                                {
                                    url = $"data:image/jpeg;base64,{request.ImageBase64}"
                                }
                            }
                        }
                    }
                }
            };

            using var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            using var resp = await _http.PostAsync("chat/completions", content, ct);

            if (!resp.IsSuccessStatusCode)
            {
                var errBody = await resp.Content.ReadAsStringAsync(ct);
                throw new InvalidOperationException($"OpenAI HTTP {(int)resp.StatusCode}: {errBody}");
            }

            var raw = await resp.Content.ReadAsStringAsync(ct);
            using var doc = JsonDocument.Parse(raw);

            var jsonFromAi = doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString();

            if (string.IsNullOrWhiteSpace(jsonFromAi))
                throw new InvalidOperationException("Resposta vazia do OpenAI.");

            var parsed = JsonSerializer.Deserialize<OcrExternalResultDto>(jsonFromAi, JsonOpts)
                         ?? throw new InvalidOperationException("JSON inválido retornado pelo OpenAI.");

            parsed.Plate = parsed.Plate?.Trim() ?? "";
            parsed.VehicleModel = parsed.VehicleModel?.Trim() ?? "";
            parsed.VehicleColor = parsed.VehicleColor?.Trim() ?? "";
            parsed.ViolationCode = parsed.ViolationCode?.Trim() ?? "";
            parsed.ViolationDescription = parsed.ViolationDescription?.Trim() ?? "";

            return parsed;
        }
    }
}