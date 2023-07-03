using eShop.Web.Models.Dto;
using eShop.Web.Services.IService;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using static eShop.Web.Utilities.StaticDetails;

namespace eShop.Web.Services
{
    public class BaseService : IBaseService
    {
        private IHttpClientFactory _httpClientFactory;

        public BaseService(IHttpClientFactory httpClientFactory)
        {

            _httpClientFactory = httpClientFactory;

        }

        public async Task<ResponseDto?> SendAsync(RequestDto request)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("eShop Http Client");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                // token
                message.RequestUri = new Uri(request.Url);

                switch (request.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post; break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put; break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete; break;
                    default:
                        message.Method = HttpMethod.Get; break;
                }

                if (request.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(request.Data), 
                        System.Text.Encoding.UTF8,
                        "application/json");
                }

                HttpResponseMessage responseMessage = await httpClient.SendAsync(message);

                ResponseDto responseDto = new ResponseDto() { IsSuccess = false, Result = null };

                switch (responseMessage.StatusCode)
                {
                    case System.Net.HttpStatusCode.NotFound:
                        responseDto.Message = "Not Found"; break;
                    case System.Net.HttpStatusCode.Forbidden:
                        responseDto.Message = "Forbidden"; break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        responseDto.Message = "Unauthorized"; break;
                    case System.Net.HttpStatusCode.InternalServerError:
                        responseDto.Message = "Internal Server Error"; break;
                    case System.Net.HttpStatusCode.BadRequest:
                        responseDto.Message = "Bad Request"; break;
                    case System.Net.HttpStatusCode.OK:
                        var data = await responseMessage.Content.ReadAsStringAsync();
                        responseDto = JsonConvert.DeserializeObject<ResponseDto>(data);
                        responseDto.IsSuccess = true;
                        break;
                    default:
                        responseDto.Message = responseMessage.StatusCode.ToString(); break;
                }
                return responseDto;
            }
            catch (Exception ex)
            {
                return new ResponseDto() {
                    Result = null,
                    IsSuccess = false,
                    Message = ex.InnerException?.Message ?? ex.Message
                };
            }
        }
    }
}
