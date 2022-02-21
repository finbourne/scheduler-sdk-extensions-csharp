using Finbourne.Scheduler.Sdk.Api;
using Finbourne.Scheduler.Sdk.Client;
using Finbourne.Scheduler.Sdk.Model;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using System.Net;

namespace Finbourne.Scheduler.Sdk.Extensions.IntegrationTests
{
    [TestFixture]
    public class ApiExceptionExtensionsTest
    {
        private IApiFactory _factory;

        [OneTimeSetUp]
        public void SetUp()
        {
            _factory = IntegrationTestApiFactoryBuilder.CreateApiFactory("secrets.json");
        }

        [Test]
        public void Generate_HttpStatusCode_BadRequest()
        {
            try
            {
                //TODO: Test with a valid API Method
                //_factory.Api<Api.xxxApi>().Methodxxx("$@!-");
            }
            catch (ApiException e)
            {
                Assert.AreEqual((int)HttpStatusCode.BadRequest, e.ErrorCode);
            }
        }

        [Test]
        public void GetRequestId_MalformedInsightsUrl_ReturnsNull()
        {
            try
            {
                _factory.Api<JobsApi>().GetRunHistory("$@!-");
            }
            catch (ApiException e)
            {
                var problemDetails = e.ProblemDetails();

                // Remove the InsightsURL which contains the requestId
                problemDetails.Instance = "";

                var apiExceptionMalformed = new ApiException(
                    errorCode: e.ErrorCode,
                    message: e.Message,
                    errorContent: JsonConvert.SerializeObject(problemDetails));

                var requestId = apiExceptionMalformed.GetRequestId();
                Assert.That(requestId, Is.Null);
            }
        }

        [Test]
        public void ProblemDetails_Converts_To_ProblemDetails_Detail()
        {
            try
            {
                _factory.Api<JobsApi>().GetRunHistory("$@!-");
            }
            catch (ApiException e)
            {
                //    ApiException.ErrorContent contains a JSON serialized ErrorResponse
                LusidProblemDetails errorResponse = e.ProblemDetails();
                Assert.That(errorResponse.Detail, Does.Match("One or more elements of the request were invalid. Please check that all supplied identifiers are valid and of the correct format, and that all provided data is correctly structured."));
            }
        }

        [Test]
        public void ProblemDetails_Converts_To_ProblemDetails_Name()
        {
            try
            {
                _factory.Api<JobsApi>().GetRunHistory("runId");
            }
            catch (ApiException e)
            {
                //    ApiException.ErrorContent contains a JSON serialized ErrorResponse
                LusidProblemDetails errorResponse = e.ProblemDetails();
                Assert.That(errorResponse.Name, Is.EqualTo("InvalidParameterValue"));
            }
        }

        [Test]
        public void ApiException_Converts_To_ValidationProblemDetails_AllowedRegex()
        {
            try
            {
                _factory.Api<JobsApi>().GetSchedulesForAJob("@£$@£%", "#####");
            }
            catch (ApiException e)
            {
                //Returns a 404 Not Found error
                Assert.That(e.ErrorCode, Is.EqualTo((int)HttpStatusCode.BadRequest), "Expect BadRequest error code");
                Assert.That(e.IsValidationProblem, Is.True, "Response should indicate that there was a validation error with the request. ");

                //    An ApiException.ErrorContent thrown because of a request validation contains a JSON serialized LusidValidationProblemDetails
                if (e.TryGetValidationProblemDetails(out var errorResponse))
                {
                    //Should identify that there was a validation error with the code
                    Assert.That(errorResponse.Errors, Contains.Key("code"));
                    Assert.That(errorResponse.Errors["code"].Single(), Is.EqualTo("Values for this field must be non-zero in length and be comprised of either alphanumeric characters, hyphens or underscores."));

                    //Should identify that there was a validation error with the scope
                    Assert.That(errorResponse.Errors, Contains.Key("scope"));
                    Assert.That(errorResponse.Errors["scope"].Single(), Is.EqualTo("Values for this field must be non-zero in length and be comprised of either alphanumeric characters, hyphens or underscores."));

                    Assert.That(errorResponse.Detail, Does.Match("One or more of the bits of input data provided were not valid*"));
                    Assert.That(errorResponse.Name, Is.EqualTo("InvalidParameterValue"));
                }
                else
                {
                    Assert.Fail("The request should have failed due to a validation error, and the validation details should be returned");
                }
            }
        }

        [Test]
        public void ApiException_Converts_To_ValidationProblemDetails_MaxLength()
        {
            try
            {
                var testScope = new string('a', 3000);
                var testCode = new string('b', 3000);
                _factory.Api<JobsApi>().GetSchedulesForAJob(testScope, testCode);
            }
            catch (ApiException e)
            {
                Assert.That(e.IsValidationProblem, Is.True, "Response should indicate that there was a validation error with the request");

                //    An ApiException.ErrorContent thrown because of a request validation contains a JSON serialized LusidValidationProblemDetails
                if (e.TryGetValidationProblemDetails(out var errorResponse))
                {
                    //Should identify that there was a validation error with the code
                    Assert.That(errorResponse.Errors, Contains.Key("code"));
                    Assert.That(errorResponse.Errors["code"].Single(), Is.EqualTo("Supplied text value was too long to be valid"));

                    //Should identify that there was a validation error with the scope
                    Assert.That(errorResponse.Errors, Contains.Key("scope"));
                    Assert.That(errorResponse.Errors["scope"].Single(), Is.EqualTo("Supplied text value was too long to be valid"));

                    Assert.That(errorResponse.Detail, Does.Match("One or more of the bits of input data provided were not valid.*"));
                    Assert.That(errorResponse.Name, Is.EqualTo("InvalidParameterValue"));
                }
                else
                {
                    Assert.Fail("The request should have failed due to a validation error, and the validation details should be returned");
                }
            }
        }
    }
}
