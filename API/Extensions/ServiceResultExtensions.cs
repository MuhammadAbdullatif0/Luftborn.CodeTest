using API.Models;
using Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions;

public static class ServiceResultExtensions
{
    public static IActionResult ToActionResult<T>(this ServiceResult<T> result, ControllerBase controller, string? getActionName = null)
    {
        if (result.IsSuccess)
        {
            var apiResult = ApiResult<T>.Ok(result.Data);
            if (result.Kind == ServiceResultKind.Created && result.CreatedId.HasValue && !string.IsNullOrEmpty(getActionName))
                return controller.CreatedAtAction(getActionName, new { id = result.CreatedId.Value }, apiResult);
            return controller.Ok(apiResult);
        }
        var failResult = ApiResult<T>.Fail(result.Kind == ServiceResultKind.NotFound ? "Resource not found." : result.Error!);
        return result.Kind == ServiceResultKind.NotFound ? controller.NotFound(failResult) : controller.BadRequest(failResult);
    }

    public static IActionResult ToActionResult(this ServiceResult<Unit> result, ControllerBase controller)
    {
        if (result.IsSuccess)
            return controller.Ok(ApiResult<object?>.Ok(null, "Deleted successfully."));
        var failResult = ApiResult<object?>.Fail(result.Kind == ServiceResultKind.NotFound ? "Resource not found." : result.Error!);
        return result.Kind == ServiceResultKind.NotFound ? controller.NotFound(failResult) : controller.BadRequest(failResult);
    }
}
