using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SolarWatch.Data;
using SolarWatch.Models;

namespace SolarWatch_IntegrationTest
{
    public class SolarWatchFactory : WebApplicationFactory<Program>
    {
    }
}
