using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using SolarWatch.Contracts;
using Xunit;

namespace SolarWatch_IntegrationTest;

public class AuthenticationTest : IDisposable
{
    private SolarWatchFactory _factory;
    private HttpClient _client;
    
    public AuthenticationTest()
    {
        _factory = new SolarWatchFactory();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Test_LoginAndRegistration()
    {
        // Login Attempt
        var loginRequest = new AuthRequest("user1@email.com", "password1");
        var loginResponse = await _client.PostAsync("/Auth/Login",
            new StringContent(JsonConvert.SerializeObject(loginRequest),
                Encoding.UTF8, "application/json"));

        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());

        // Assert Login
        if (loginResponse.StatusCode == HttpStatusCode.OK)
        {
            Assert.NotNull(authResponse.Token);
            Assert.Equal("user1@email.com", authResponse.Email);
            Assert.Equal("user1", authResponse.UserName);
        }
        else
        {
            // Registration Attempt
            var registrationRequest = new RegistrationRequest("user2@email.com", "user2", "password2");
            var registrationResponse = await _client.PostAsync("/Auth/Register",
                new StringContent(JsonConvert.SerializeObject(registrationRequest),
                    Encoding.UTF8, "application/json"));

            // Assert Registration
            Assert.Equal(HttpStatusCode.BadRequest, registrationResponse.StatusCode);
        }
    }

    
    [Fact]
    public async Task Test_GetSolarWatch()
    {
        // Arrange
        var loginRequest = new AuthRequest("admin@admin.com", "admin123");
    
        // Act
        var loginResponse = await _client.PostAsync("/Auth/Login",
            new StringContent(JsonConvert.SerializeObject(loginRequest), 
                Encoding.UTF8, "application/json"));

        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());
        var adminToken = authResponse.Token;

        // Assert
        Assert.NotNull(authResponse.Token);
        Assert.Equal("admin@admin.com", authResponse.Email);
        Assert.Equal("admin", authResponse.UserName);

        // Act
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
    
        var solarWatchRequest = new SolarWatchRequest("Budapest");

        // Act
        var solarWatchResponse = await _client.PostAsync("/SolarWatch/GetSunriseAndSunset", new StringContent(JsonConvert.SerializeObject(solarWatchRequest), 
            Encoding.UTF8, "application/json"));
    
        // Assert
        Assert.Equal(HttpStatusCode.OK, solarWatchResponse.StatusCode);
    }


    [Fact]
    public async Task Test_NoAuthorization()
    {
        // Step 1: Authenticate user and obtain a token
        var loginRequest = new AuthRequest("user1@email.com", "password1");

        var loginResponse = await _client.PostAsync("/Auth/Login",
            new StringContent(JsonConvert.SerializeObject(loginRequest),
                Encoding.UTF8, "application/json"));
        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());
        Assert.NotNull(authResponse.Token);

        // Step 2: Reset authorization header to null (no authorization)
        _client.DefaultRequestHeaders.Authorization = null;

        // Step 3: Attempt to access the protected endpoint without proper authorization
        var solarWatchResponse = await _client.GetAsync("/SolarWatch/GetSolarWatches");

        // Step 4: Assert that the response is Forbidden
        Assert.Equal(HttpStatusCode.Unauthorized, solarWatchResponse.StatusCode);
    }

    
    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }
}