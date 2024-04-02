using System;
using System.Net;

namespace SampleOrg.WebAPI.Common;

public static class Problems
{
    public static Problem DatabaseTimeout()
    {
        return new Problem
        {
            Status = (int)HttpStatusCode.ServiceUnavailable,
            Title = ProblemType.ServiceUnavailable.ToString(),
            Type = ProblemType.ServiceUnavailable,
            Detail = "Database is temporarily unavailable, please try again later"
        };
    }

    public static Problem UserNotExist(Guid userId)
    {
        return new Problem
        {
            Status = (int)HttpStatusCode.BadRequest,
            Title = ProblemType.UserNotFound.ToString(),
            Type = ProblemType.UserNotFound,
            Detail = $"User {userId} does not exist"
        };
    }

    public static Problem UserWithSameEmailExists(string email)
    {
        return new Problem
        {
            Status = (int)HttpStatusCode.BadRequest,
            Title = ProblemType.UserExists.ToString(),
            Type = ProblemType.UserExists,
            Detail = $"User with {email} already exists"
        };
    }

    public static Problem NotEnoughSalary(Guid userId)
    {
        return new Problem
        {
            Status = (int)HttpStatusCode.BadRequest,
            Title = ProblemType.NotEnoughSalary.ToString(),
            Type = ProblemType.NotEnoughSalary,
            Detail = $"User with {userId} doesn't have enough salary"
        };
    }

    public static Problem InternalServerError()
    {
        return new Problem()
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Title = ProblemType.InternalServerError.ToString(),
            Type = ProblemType.InternalServerError,
            Detail = "Internal Server Error"
        };
    }

    public static Problem TaskCancelled()
    {
        return new Problem()
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Title = ProblemType.TaskCancelled.ToString(),
            Type = ProblemType.TaskCancelled,
            Detail = "Request was cancelled"
        };
    }
}

public enum ProblemType
{
    ServiceUnavailable,
    UserNotFound,
    UserExists,
    NotEnoughSalary,
    InternalServerError,
    TaskCancelled
}