using PicketMobile.Models;
using SharedModel.Requests;
using SharedModel.Responses;
using System.Net.Http.Json;
using System.Text.Json;

namespace PicketMobile.Services
{

    public interface IPicketService
    {
        Task<PaginationResponse<PicketResponse>> Get(PaginationRequest req);
        Task<PicketResponse> GetById(int id);
        Task<PicketResponse> GetPicketToday();
        Task<PicketModel> Create(PicketModel model);
        Task<LateAndGoHomeEarlyResponse> AddLateandEarly(StudentToLateAndEarlyRequest model);
        Task<bool> DeleteToLate(int studentTolateId);
        Task<bool> DeleteToComeHomeEarly(int studentGoHomeErly);
        Task<bool> Put(int id, PicketRequest model);
    }

    public class PicketService : IPicketService
    {
        private static PicketResponse? picket;
        private readonly HttpClient _httpClient;

        public PicketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PicketModel> Create(PicketModel model)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/picket", model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.GetResultAsync<PicketModel>();
                if (result != null)
                    return result;
            }
            throw new HttpRequestException(await _httpClient.Error(response));
        }

        public async Task<PicketResponse> GetPicketToday()
        {
            if (picket != null && DateOnly.FromDateTime(picket.CreateAt) == DateOnly.FromDateTime(DateTime.UtcNow))
                return picket;

            HttpResponseMessage response = await _httpClient.GetAsync($"api/picket");
            if (response.IsSuccessStatusCode)
            {
                picket = await response.GetResultAsync<PicketResponse>();
                return picket;
            }
            throw new HttpRequestException(await _httpClient.Error(response));
        }

        public async Task<bool> DeleteToComeHomeEarly(int studentGoHomeErly)
        {
            var response = await _httpClient.DeleteAsync($"/picket/removehomeearly");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteToLate(int studentTolateId)
        {
            var response = await _httpClient.DeleteAsync($"/picket/removelate");
            return response.IsSuccessStatusCode;
        }

        public async Task<LateAndGoHomeEarlyResponse> AddLateandEarly(StudentToLateAndEarlyRequest model)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"/api/picket/lateandearly", model);
            if (response.IsSuccessStatusCode)
            {
                var result = JsonSerializer.Deserialize<LateAndGoHomeEarlyResponse>(await response.Content.ReadAsStringAsync(), Helper.JsonOption);
                if (result != null)
                {
                    if (picket != null && picket.StudentsLateAndComeHomeEarly != null)
                    {
                        picket.StudentsLateAndComeHomeEarly.Add(result);
                    }
                    return result;
                }
            }
            throw new HttpRequestException(await _httpClient.Error(response));
        }

        public async Task<bool> Put(int id, PicketRequest model)
        {
            HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/picket/{id}", model);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.GetResultAsync<bool>();
                if (result)
                    return result;
            }
            throw new HttpRequestException(await _httpClient.Error(response));
        }

        public async Task<PaginationResponse<PicketResponse>> Get(PaginationRequest req)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync($"api/picket/paginate", req);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.GetResultAsync<PaginationResponse<PicketResponse>>();
                if (result != null)
                    return result;
            }
            throw new HttpRequestException(await _httpClient.Error(response));
        }

        public async Task<PicketResponse> GetById(int id)
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"api/picket/{id}");
            if (response.IsSuccessStatusCode)
            {
                picket = await response.GetResultAsync<PicketResponse>();
                return picket;
            }
            throw new HttpRequestException(await _httpClient.Error(response));
        }
    }
}
