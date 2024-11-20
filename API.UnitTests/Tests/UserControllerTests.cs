using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using API.DTOs;
using API.UnitTests.Helpers;
using Newtonsoft.Json.Linq;

namespace API.UnitTests.Tests;
public class UserControllerTests
{
    private readonly string apiRoute = "API/users";
    private readonly HttpClient _client;
    private HttpResponseMessage httpResponse;
    private string requestUrl;
    private string loginObject;
    private HttpContent httpContent;

    public UserControllerTests()
    {
        _client = TestHelper.Instance.Client;
    }

    [Fact]
    public async Task GetUsersOK()
    {
        // Arrange
        var expectedStatusCode = "OK";
        requestUrl = "API/account/login";
        var loginRequest = new LoginRequest
        {
            UserName = "arenita",
            Password = "123456"
        };

        loginObject = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse.Token);

        requestUrl = $"{apiRoute}";

        httpResponse = await _client.GetAsync(requestUrl);

        //assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(expectedStatusCode, true), httpResponse.StatusCode);
        Assert.Equal(expectedStatusCode, httpResponse.StatusCode.ToString());
    }

    [Fact]
    public async Task GetUsersByUsernameOK()
    {
        // Arrange
        var expectedStatusCode = "OK";
        requestUrl = "API/account/login";
        var loginRequest = new LoginRequest
        {
            UserName = "arenita",
            Password = "123456"
        };

        loginObject = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse.Token);

        requestUrl = $"{apiRoute}/bob";

        httpResponse = await _client.GetAsync(requestUrl);

        //assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(expectedStatusCode, true), httpResponse.StatusCode);
        Assert.Equal(expectedStatusCode, httpResponse.StatusCode.ToString());
    }

    [Fact]
    public async Task GetUsersByUsernameNotFound()
    {
        // Arrange
        var expectedStatusCode = "NotFound";
        requestUrl = "API/account/login";
        var loginRequest = new LoginRequest
        {
            UserName = "arenita",
            Password = "123456"
        };

        loginObject = GetLoginObject(loginRequest);
        httpContent = GetHttpContent(loginObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var reponse = await httpResponse.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", userResponse.Token);

        requestUrl = $"{apiRoute}/fake";

        httpResponse = await _client.GetAsync(requestUrl);

        //assert
        Assert.Equal(Enum.Parse<HttpStatusCode>(expectedStatusCode, true), httpResponse.StatusCode);
        Assert.Equal(expectedStatusCode, httpResponse.StatusCode.ToString());
    }

    #region Privated methods

    private static string GetLoginObject(LoginRequest loginDto)
    {
        var entityObject = new JObject()
            {
                { nameof(loginDto.UserName), loginDto.UserName },
                { nameof(loginDto.Password), loginDto.Password }
            };

        return entityObject.ToString();
    }

    private static StringContent GetHttpContent(string objectToCode) =>
        new(objectToCode, Encoding.UTF8, "application/json");

    #endregion
}