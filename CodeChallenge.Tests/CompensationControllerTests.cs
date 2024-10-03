
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public async Task CreateCompensationReturns_Ok() 
        {
            var compensation = new Compensation
            {
                Employee = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                Salary = 10,
                EffectiveDate = DateTime.Parse("2025-01-01")
            };

            var content = new JsonSerialization().ToJson(compensation);
            var response  = await _httpClient.PostAsync("api/compensation",
               new StringContent(content, Encoding.UTF8, "application/json"));

            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            
            var result = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(result.Id);

            Assert.AreEqual(compensation.Employee, result.Employee);
            Assert.AreEqual(compensation.Salary, result.Salary);
            Assert.AreEqual(compensation.EffectiveDate, result.EffectiveDate);
        }

        [TestMethod]
        public async Task CreateCompensationReturns_BadRequest_On_InvalidCompensation() 
        {
            var invalidCompensations = new Compensation[] 
            {
                new Compensation
                {
                    Employee = "doesnotexist",
                    Salary = 10,
                    EffectiveDate = DateTime.Parse("2025-01-01")
                },
                new Compensation
                {
                    Employee = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                    EffectiveDate = DateTime.Parse("2025-01-01")
                },
                new Compensation
                {
                    Employee = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                    Salary = 10,
                }
            };

            for (var i = 0; i < invalidCompensations.Length; i++) 
            {
                var comp = invalidCompensations[i];

                var content = new JsonSerialization().ToJson(comp);
                var response  = await _httpClient.PostAsync("api/compensation",
                    new StringContent(content, Encoding.UTF8, "application/json"));

                Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            }
        }

        [TestMethod]
        public async Task GetCompensationReturns_Ok() 
        {
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var response  = await _httpClient.GetAsync($"api/compensation/{employeeId}");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            
            var result = response.DeserializeContent<Compensation[]>();
            Assert.AreEqual(1, result.Length);

            var compensation = result.SingleOrDefault();
            Assert.AreEqual(10, compensation.Salary);
            Assert.AreEqual(DateTime.Parse("2025-01-01"), compensation.EffectiveDate);
        }
    }
}