namespace API.UnitTests.Tests;

using System.Text;
using API.DTOs;
using API.UnitTests.Helpers;
using Newtonsoft.Json.Linq;

public class AccountControllerTests
{
    private readonly string apiRoute = "API/account";
    private readonly HttpClient _client;
    private HttpResponseMessage httpResponse;
    private string requestUrl;
    private string registerObject;

    private HttpContent httpContent;

    public AccountControllerTests()
    {
        _client = TestHelper.Instance.Client;
    }

    [Fact]
    public async Task GetUserNameAlreadyInUse()
    {
        // Arrange
        var expectedResponse = "Username already in use";
        requestUrl = "API/account/register";
        var registerRequest = new RegisterRequest
        {
            UserName = "arenita",
            Password = "123456"
        };
 
        registerObject = GetRegisterObject(registerRequest);
        httpContent = GetHttpContent(registerObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var response = await httpResponse.Content.ReadAsStringAsync();
        //var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        requestUrl = $"{apiRoute}/register";

        // Assert
        Assert.Equal(expectedResponse, response);
    }

    [Fact]
    public async Task GetInvalidUserName()
    {
        // Arrange
        var expectedResponse = "Invalid username";
        requestUrl = "API/account/login";
        var registerRequest = new RegisterRequest
        {
            UserName = "",
            Password = "123456"
        };
 
        registerObject = GetRegisterObject(registerRequest);
        httpContent = GetHttpContent(registerObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var response = await httpResponse.Content.ReadAsStringAsync();
        //var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        requestUrl = $"{apiRoute}/register";

        // Assert
        Assert.Equal(expectedResponse, response);
    }

    [Fact]
    public async Task GetInvalidPassword()
    {
        // Arrange
        var expectedResponse = "Invalid password";
        requestUrl = "API/account/login";
        var registerRequest = new RegisterRequest
        {
            UserName = "arenita",
            Password = "12"
        };
 
        registerObject = GetRegisterObject(registerRequest);
        httpContent = GetHttpContent(registerObject);

        httpResponse = await _client.PostAsync(requestUrl, httpContent);
        var response = await httpResponse.Content.ReadAsStringAsync();
        //var userResponse = JsonSerializer.Deserialize<UserResponse>(reponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        requestUrl = $"{apiRoute}/register";

        // Assert
        Assert.Equal(expectedResponse, response);
    }

#region Privated methods

    private static StringContent GetHttpContent(string objectToCode) =>
        new(objectToCode, Encoding.UTF8, "application/json");

    #endregion

    private static string GetRegisterObject(RegisterRequest registerDto)
    {
        var entityObject = new JObject()
            {
                { nameof(registerDto.UserName), registerDto.UserName },
                { nameof(registerDto.Password), registerDto.Password }
            };

        return entityObject.ToString();
    }
}