
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CodeChallenge.Config;
using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        // Attribute ClassInitialize requires this signature
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
        public static void InitializeClass(TestContext context)
        {
            //reseed since EmployeeController tests modifies the data used by these tests
            App.SeedEmployeeDB(reseed: true);
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
        public void GetReportingStructureById_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";

            // Execute
            var getRequestTask = _httpClient.GetAsync($"api/numberOfReports/{employeeId}");
            var response = getRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var reportingStructure = response.DeserializeContent<ReportingStructure>();
            Assert.AreEqual(4, reportingStructure.NumberOfReports);
            Assert.AreEqual(employeeId, reportingStructure.Employee.EmployeeId);
        }

		[TestMethod]
		public void GetReportingStructureById_Returns_NotFound()
		{
			// Arrange
			var fakeEmployeeId = "c59b1235-e2e7-4802-9c72-7a1e971f83b8";

			// Execute
			var getRequestTask = _httpClient.GetAsync($"api/numberOfReports/{fakeEmployeeId}");
			var response = getRequestTask.Result;

			// Assert
			Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
		}
	}
}
