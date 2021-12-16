using Blazored.LocalStorage;
using BookStoreUI.Contracts;
using BookStoreUI.Models;
using BookStoreUI.Providers;
using BookStoreUI.Static;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreUI.Service
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly IHttpClientFactory _client;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        

        public AuthenticationRepository(IHttpClientFactory client,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider)
        {
            _client = client;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<bool> Login(LoginModel user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                EndPoints.LoginEndPoint);
            request.Content = new StringContent(JsonConvert.SerializeObject(user),
                Encoding.UTF8, "application/json");
            var client = _client.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return false;
            }
            var content = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<TokenResponse>(content);

            //store token
            await _localStorage.SetItemAsync("suthToken", token.Token);
            //change auth state of app
            await ((APIAuthenticationStateProvider)_authenticationStateProvider).LoggedIn();
            client.DefaultRequestHeaders.Authorization =
                 new AuthenticationHeaderValue("bearer", token.Token);
            return true;

        }

        public Task Logout()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Register(RegistrationModel user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                EndPoints.RegisterEndPoint);
            request.Content = new StringContent(JsonConvert.SerializeObject(user),
                Encoding.UTF8, "application/json");
            var client = _client.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            return response.IsSuccessStatusCode;

        }
    }
}
