using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SampleOrg.WebAPI.Common;

public static class ProblemExtensions
{
    public static IActionResult AsJsonResult(this Problem problem)
    {
        return new JsonResult(problem)
        {
            StatusCode = problem.Status,
            ContentType = Problem.ContentType
        };
    }

    public static async Task WriteAsync(this HttpResponse response, Problem problem)
    {
        response.ContentType = Problem.ContentType;
        response.StatusCode = problem.Status;
        await response.WriteAsync(JsonConvert.SerializeObject(problem)).ConfigureAwait(false);
    }
}