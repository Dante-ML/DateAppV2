using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.DTOs;
using API.UnitTests.Helpers;
using Newtonsoft.Json.Linq;

namespace API.UnitTests.Tests;

public class MemberUpdateRequestTest
{
    private readonly string apiRoute = "API/users";
    private readonly HttpClient _client;
    private HttpResponseMessage httpResponse;
    private string requestUrl;
    private string loginObject;
    private string memberObject;
    private HttpContent httpContent;
    private HttpContent httpContentMember;

    public MemberUpdateRequestTest()
    {
        _client = TestHelper.Instance.Client;
    }

    [Fact]
    public async Task UpdateUserNoUsername()
    {
        // Arrange
        var expectedStatusCode = "NoContent";
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

        //updateData
        var updateRequest = new MemberUpdateRequest
        {
            Introduction = "Hola",
            LookingFor = "esto",
            Interests = "es",
            City = "una",
            Country = "prueba"
        };

        memberObject = GetMemberObject(updateRequest);
        httpContentMember = GetHttpContent(memberObject);

        httpResponse = await _client.PutAsync(requestUrl, httpContentMember);

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

    private static string GetMemberObject(MemberUpdateRequest memberDto)
    {
        var entityObject = new JObject()
            {
                { nameof(memberDto.Introduction), memberDto.Introduction },
                { nameof(memberDto.LookingFor), memberDto.LookingFor },
                { nameof(memberDto.Interests), memberDto.Interests },
                { nameof(memberDto.City), memberDto.City },
                { nameof(memberDto.Country), memberDto.Country }
            };

        return entityObject.ToString();
    }

    private static StringContent GetHttpContent(string objectToCode) =>
        new(objectToCode, Encoding.UTF8, "application/json");

    #endregion
}
