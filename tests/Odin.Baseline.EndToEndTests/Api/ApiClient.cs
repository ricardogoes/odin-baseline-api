using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.AspNetCore.WebUtilities;
using Odin.Baseline.Domain.DTO.Common;
using Odin.Baseline.Infra.Messaging.JsonPolicies;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Odin.Baseline.EndToEndTests.Api
{
    public class ApiClient
    {
        private readonly AppSettings _appSettings;
        private readonly AmazonCognitoIdentityProviderClient _awsIdentityProvider;

        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _defaultSerializeOptions;

        private readonly string _username = Environment.GetEnvironmentVariable("OdinSettings__AdminUsername")!;
        private readonly string _password = Environment.GetEnvironmentVariable("OdinSettings__AdminPassword")!;

        public ApiClient(HttpClient httpClient)
        {
            var cognitoSettings = new AWSCognitoSettings
            (
                Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__AccessKeyId")!,
                Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__AccessSecretKey")!,
                Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__AppClientId")!,
                Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__CognitoAuthorityUrl")!,
                Environment.GetEnvironmentVariable("OdinSettings__AWSCognitoSettings__Region")!
            );

            _appSettings = new AppSettings(cognitoSettings);

            _httpClient = httpClient;

            _defaultSerializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new JsonSnakeCasePolicy(),
                PropertyNameCaseInsensitive = true
            };

            _awsIdentityProvider = new AmazonCognitoIdentityProviderClient(
                _appSettings.AWSCognitoSettings.AccessKeyId,
                _appSettings.AWSCognitoSettings.AccessSecretKey,
                RegionEndpoint.GetBySystemName(_appSettings.AWSCognitoSettings.Region)
            );

            AddAuthorizationHeader();
        }

        private void AddAuthorizationHeader()
        {
            var accessToken = GetAccessTokenAsync().Result;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var authRequest = new InitiateAuthRequest()
            {
                AuthFlow = "USER_PASSWORD_AUTH",
                ClientId = _appSettings.AWSCognitoSettings.AppClientId,
                AuthParameters =
                {
                    { "USERNAME", _username },
                    { "PASSWORD", _password }
                }
            };

            var authResponse = await _awsIdentityProvider.InitiateAuthAsync(authRequest);
            var result = authResponse.AuthenticationResult;
            return result.IdToken;
        }

        public async Task<(HttpResponseMessage, TOutput)> PostAsync<TOutput>(string route, object? request) where TOutput : class
        {
            var requestJson = JsonSerializer.Serialize(request, _defaultSerializeOptions);

            var response = await _httpClient.PostAsync(route, new StringContent(requestJson, Encoding.UTF8, "application/json"));
            var output = await GetOutputAsync<TOutput>(response);

            return (response, output);
        }

        public async Task<(HttpResponseMessage, TOutput)> PutAsync<TOutput>(string route, object? request) where TOutput : class
        {
            var requestJson = JsonSerializer.Serialize(request, _defaultSerializeOptions);

            var response = await _httpClient.PutAsync(route, new StringContent(requestJson, Encoding.UTF8, "application/json"));
            var output = await GetOutputAsync<TOutput>(response);

            return (response, output);
        }

        public async Task<(HttpResponseMessage, TOutput)> GetByIdAsync<TOutput>(string route, object? queryStringParametersObject = null) where TOutput : class
        {
            var urlToCall = PrepareGetRoute(route, queryStringParametersObject);

            var response = await _httpClient.GetAsync(urlToCall);
            var output = await GetOutputAsync<TOutput>(response);

            return (response, output);
        }

        public async Task<(HttpResponseMessage, TOutput)> GetAsync<TOutput>(string route, object? queryStringParametersObject = null) where TOutput : class
        {
            var urlToCall = PrepareGetRoute(route, queryStringParametersObject);

            var response = await _httpClient.GetAsync(urlToCall);
            var output = await GetOutputAsync<TOutput>(response);

            return (response, output);
        }

        private async Task<TOutput> GetOutputAsync<TOutput>(HttpResponseMessage response) where TOutput : class
        {
            var outputString = await response.Content.ReadAsStringAsync();

            TOutput? output = null;

            if (!string.IsNullOrWhiteSpace(outputString))
                output = JsonSerializer.Deserialize<TOutput>(outputString, _defaultSerializeOptions);

            return output!;
        }

        private string PrepareGetRoute(string route, object? queryStringParametersObject)
        {
            if (queryStringParametersObject is null)
                return route;

            var parametersJson = JsonSerializer.Serialize(queryStringParametersObject, _defaultSerializeOptions);
            var parametersDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(parametersJson);

            return QueryHelpers.AddQueryString(route, parametersDictionary!);
        }
    }
}
