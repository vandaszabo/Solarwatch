using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using SolarWatch.Contracts;
using Xunit;
using Xunit.Abstractions;

namespace SolarWatch_IntegrationTest;

public class AuthenticationTest : IClassFixture<EnvironmentFixture>
{
    private SolarWatchFactory _factory;
    private HttpClient _client;
    private ITestOutputHelper _output;
    private readonly EnvironmentFixture _environmentFixture;

    public AuthenticationTest(ITestOutputHelper output, EnvironmentFixture environmentFixture)
    {
        _output = output;
        _environmentFixture = environmentFixture;
        _factory = new SolarWatchFactory();
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Test_Registration()
    {
        // Step1: Registration
        var registrationRequest = new RegistrationRequest("user1@email.com", "user1", "password1");
        var registrationResponse = await _client.PostAsync("/Auth/Register",
            new StringContent(JsonConvert.SerializeObject(registrationRequest),
                Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.Created, registrationResponse.StatusCode);
    }

    [Fact]
    public async Task Test_RegistrationAndLogin()
    {
        // Step1: Registration
        var registrationRequest = new RegistrationRequest("user1@email.com", "user1", "password1");
        var registrationResponse = await _client.PostAsync("/Auth/Register",
            new StringContent(JsonConvert.SerializeObject(registrationRequest),
                Encoding.UTF8, "application/json"));
        Assert.Equal(HttpStatusCode.Created, registrationResponse.StatusCode);

        // Step2: Login
        var loginRequest = new AuthRequest("user1@email.com", "password1");

        var loginResponse = await _client.PostAsync("/Auth/Login",
            new StringContent(JsonConvert.SerializeObject(loginRequest),
                Encoding.UTF8, "application/json"));

        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());

        // Step3: Assert
        Assert.Equal(HttpStatusCode.OK, loginResponse.StatusCode);
        Assert.NotNull(authResponse.Token);
        Assert.Equal("user1@email.com", authResponse.Email);
        Assert.Equal("user1", authResponse.UserName);
    }

    [Fact]
    public async Task Test_GetUserRoles()
    {
        // Step1: Registration
        var registrationRequest = new RegistrationRequest("user1@email.com", "user1", "password1");
        var registrationResponse = await _client.PostAsync("/Auth/Register",
            new StringContent(JsonConvert.SerializeObject(registrationRequest),
                Encoding.UTF8, "application/json"));

        // Step2: Login
        var loginRequest = new AuthRequest("user1@email.com", "password1");
        var loginResponse = await _client.PostAsync("/Auth/Login",
            new StringContent(JsonConvert.SerializeObject(loginRequest),
                Encoding.UTF8, "application/json"));

        var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.Token);
        _output.WriteLine($"Authorization Header: {_client.DefaultRequestHeaders.Authorization}");
        
        // Step3: GetRoles
        var userRequest = new UserRequest("user1");
        var userResponse = await _client.PostAsync("/User/Roles",
            new StringContent(JsonConvert.SerializeObject(userRequest),
                Encoding.UTF8, "application/json"));

        var responseContent = await userResponse.Content.ReadAsStringAsync();
        _output.WriteLine($"Response Content: {responseContent}");
        
        var roleResponse = JsonConvert.DeserializeObject<IList<string>>(await userResponse.Content.ReadAsStringAsync());
        _output.WriteLine($"User Roles: {roleResponse.First()}");
        
        Assert.NotNull(roleResponse);
        Assert.Equal("User", roleResponse.First());

    }
    
    public void Dispose()
    {
        _factory.Dispose();
        _client.Dispose();
    }
}
