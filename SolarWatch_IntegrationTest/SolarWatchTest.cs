// using System.Net;
// using System.Net.Http.Headers;
// using System.Text;
// using Newtonsoft.Json;
// using SolarWatch.Contracts;
// using Xunit;
// using Xunit.Abstractions;
//
// namespace SolarWatch_IntegrationTest;
//
// public class SolarWatchTest
// {
//     private SolarWatchFactory _factory;
//     private HttpClient _client;
//     private ITestOutputHelper _output;
//     private readonly EnvironmentFixture _environmentFixture;
//
//     public SolarWatchTest(ITestOutputHelper output, EnvironmentFixture environmentFixture)
//     {
//         _output = output;
//         _environmentFixture = environmentFixture;
//         _factory = new SolarWatchFactory();
//         _client = _factory.CreateClient();
//     }
//     
//     [Fact]
//     public async Task Test_GetSolarWatch()
//     {
//         // Step1: Registration
//         var registrationRequest = new RegistrationRequest("user1@email.com", "user1", "password1");
//         var registrationResponse = await _client.PostAsync("/Auth/Register",
//             new StringContent(JsonConvert.SerializeObject(registrationRequest),
//                 Encoding.UTF8, "application/json"));
//
//         // Step2: Login
//         var loginRequest = new AuthRequest("user1@email.com", "password1");
//         var loginResponse = await _client.PostAsync("/Auth/Login",
//             new StringContent(JsonConvert.SerializeObject(loginRequest),
//                 Encoding.UTF8, "application/json"));
//
//         var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());
//
//         _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authResponse.Token);
//         _output.WriteLine($"Authorization Header: {_client.DefaultRequestHeaders.Authorization}");
//
//         //Step3: Get SolarWatch data
//         var solarWatchRequest = new SolarWatchRequest("Budapest");
//         var solarWatchResponse = await _client.PostAsync("/SolarWatch/GetSunriseAndSunset", new StringContent(
//             JsonConvert.SerializeObject(solarWatchRequest),
//             Encoding.UTF8, "application/json"));
//
//         _output.WriteLine($"Response: {solarWatchResponse}");
//
//         // Step4: Assert
//         Assert.Equal(HttpStatusCode.OK, solarWatchResponse.StatusCode);
//     }
//
//
//     [Fact]
//     public async Task Test_ForbiddenRequest()
//     {
//         //Register
//         var registrationRequest = new RegistrationRequest("user1@email.com", "user1", "password1");
//
//         var registrationResponse = await _client.PostAsync("/Auth/Register",
//             new StringContent(JsonConvert.SerializeObject(registrationRequest),
//                 Encoding.UTF8, "application/json"));
//
//         // Login
//         var loginRequest = new AuthRequest("user1@email.com", "password1");
//
//         var loginResponse = await _client.PostAsync("/Auth/Login",
//             new StringContent(JsonConvert.SerializeObject(loginRequest),
//                 Encoding.UTF8, "application/json"));
//         var authResponse = JsonConvert.DeserializeObject<AuthResponse>(await loginResponse.Content.ReadAsStringAsync());
//         Assert.NotNull(authResponse.Token);
//
//         _client.DefaultRequestHeaders.Authorization = null;
//
//         var solarWatchResponse = await _client.GetAsync("/SolarWatch/GetSolarWatches");
//
//         // Assert
//         Assert.Equal(HttpStatusCode.Forbidden, solarWatchResponse.StatusCode);
//     }
// }