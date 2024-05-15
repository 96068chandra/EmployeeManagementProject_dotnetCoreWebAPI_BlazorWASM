using BaseLibrary.Dtos;
using BaseLibrary.Responses;
using ClientLibrary.Helpers;
using ClientLibrary.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClientLibrary.Services.Implementation
{
    public class UserAccountService(GetHttpClient getHttpClient) : IUserAccountService
    {
        public const string AuthUrl = "api/authentication";
        public async Task<GeneralRespose> CreateAsync(Register user)
        {
            var httpclient=getHttpClient.GetPublicHttpClient();
            var result = await httpclient.PostAsJsonAsync($"{AuthUrl}/register", user);
            if (!result.IsSuccessStatusCode) return new GeneralRespose(false, "Error occured");
            return await result.Content.ReadFromJsonAsync<GeneralRespose>();
            
        }
        public async Task<LoginResponse> SignInAsync(Login user)
        {
            var httpclient = getHttpClient.GetPublicHttpClient();
            var result = await httpclient.PostAsJsonAsync($"{AuthUrl}/login", user);
            if (!result.IsSuccessStatusCode) return new LoginResponse(false, "Error occured");
            return await result.Content.ReadFromJsonAsync<LoginResponse>()!;
        }
        public Task<LoginResponse> RefreshTokenAsync(RefreshToken token)
        {
            throw new NotImplementedException();
        }

        
    }
}
