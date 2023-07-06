using eShop.Web.Models.Dto;
using eShop.Web.Services.IService;
using eShop.Web.Utilities;
using Newtonsoft.Json;
using System.Net;
using System.Text.Json.Serialization;
using static eShop.Web.Utilities.StaticDetails;

namespace eShop.Web.Services
{
    public class BaseService : IBaseService
    {
        private IHttpClientFactory _httpClientFactory;
        private ITokenProvider _tokenProvider;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenProvider tokenProvider)
        {

            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }

        public async Task<ResponseDto?> SendAsync(RequestDto request, bool useToken = true)
        {
            try
            {
                HttpClient httpClient = _httpClientFactory.CreateClient("eShop Http Client");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.Headers.Add(HeaderAuthorization, $"Bearer {_tokenProvider.GetToken()}");
                
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

                var data = await responseMessage.Content.ReadAsStringAsync();
                if (data?.Length > 0 && responseMessage.StatusCode < HttpStatusCode.InternalServerError)
                {
                    responseDto = JsonConvert.DeserializeObject<ResponseDto>(data);
                }

                if (responseMessage.StatusCode == HttpStatusCode.OK)
                {
                    responseDto.IsSuccess = true;
                }
                else
                {
                    if (responseDto.Message?.Length < 1)
                    {
                        switch (responseMessage.StatusCode)
                        {
                            case HttpStatusCode.NotFound:
                                responseDto.Message = "Not Found"; break;
                            case HttpStatusCode.Forbidden:
                                responseDto.Message = "Forbidden"; break;
                            case HttpStatusCode.Unauthorized:
                                responseDto.Message = "Unauthorized"; break;
                            case HttpStatusCode.InternalServerError:
                                responseDto.Message = "Internal Server Error"; break;
                            case HttpStatusCode.BadRequest:
                                responseDto.Message = "Bad Request"; break;
                            default:
                                responseDto.Message = responseMessage.StatusCode.ToString(); break;
                        }
                    }
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
