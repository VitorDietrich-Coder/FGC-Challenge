using FGC.Api.Swagger.Attributes;
using FGC.Application.Auth.Models.Response;
using FGC.Application.Common;
using FGC.Application.Games.Models.Response;
using FGC.Application.Deals.Models.Response;
using FGC.Application.Users.Models.Response;
using FGC.Domain.Core;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace FGC.Api.Swagger.Filters
{
    public class SwaggerResponseProfileOperationFilter : IOperationFilter
    {
        private static readonly Dictionary<string, (int StatusCode, Type ResponseType, Type ExampleType)[]> Profiles
            = new()
            {
                ["User.Register"] = new[]
                {
                    (201, typeof(SuccessResponse<UserResponse>), typeof(SuccessResponse<UserResponse>)),
                    (400, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorBadRequestExample)),
                    (409, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorConflictExample)),
                    (500, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorInternalServerExample)),
                },
                ["User.Update"] = new[]
                {
                    (400, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorBadRequestExample)),
                    (401, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorUnauthorizedExample)),
                    (403, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorForbiddenExample)),
                    (404, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorNotFoundExample)),
                    (500, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorInternalServerExample)),
                },
                ["User.Delete"] = new[]
                {
                    (400, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorBadRequestExample)),
                    (401, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorUnauthorizedExample)),
                    (403, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorForbiddenExample)),
                    (404, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorNotFoundExample)),
                    (500, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorInternalServerExample)),
                },
                ["User.Get"] = new[]
                {
                    (200, typeof(SuccessResponse<UserResponse>), typeof(SuccessResponse<UserResponse>)),
                    (400, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorBadRequestExample)),
                    (401, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorUnauthorizedExample)),
                    (403, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorForbiddenExample)),
                    (500, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorInternalServerExample)),
                },
                ["User.GetAll"] = new[]
                {
                    (200, typeof(SuccessResponse<UserResponse>), typeof(SuccessResponse<UserResponse>)),
                    (400, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorBadRequestExample)),
                    (401, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorUnauthorizedExample)),
                    (403, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorForbiddenExample)),
                    (500, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorInternalServerExample)),
                },
                ["User.Get.UsersGames"] = new[]
                {
                    (200, typeof(SuccessResponse<UserResponse>), typeof(SuccessResponse<UserResponse>)),
                    (401, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorUnauthorizedExample)),
                    (404, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorNotFoundExample)),
                    (500, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorInternalServerExample)),
                },
                ["Admin.Register"] = new[]
                {
                    (201, typeof(SuccessResponse<UserResponse>), typeof(SuccessResponse<UserResponse>)),
                    (400, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorBadRequestExample)),
                    (403, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorForbiddenExample)),
                    (409, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorConflictExample)),
                    (500, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorInternalServerExample)),
                },
                ["deal.Create"] = new[]
                {
                    (400, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorBadRequestExample)),
                    (401, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorUnauthorizedExample)),
                    (403, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorForbiddenExample)),
                    (500, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorInternalServerExample)),
                },
                ["deal.Update"] = new[]
                {
                    (400, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorBadRequestExample)),
                    (401, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorUnauthorizedExample)),
                    (403, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorForbiddenExample)),
                    (404, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorNotFoundExample)),
                    (500, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorInternalServerExample)),
                },
                ["deal.Get"] = new[]
                {
                    (200, typeof(SuccessResponse<DealResponse>), typeof(SuccessResponse<DealResponse>)),
                    (400, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorBadRequestExample)),
                    (401, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorUnauthorizedExample)),
                    (403, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorForbiddenExample)),
                    (404, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorNotFoundExample)),
                    (500, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorInternalServerExample)),
                },
                ["Games.Create"] = new[]
                {
                    (200, typeof(SuccessResponse<DealResponse>), typeof(SuccessResponse<DealResponse>)),
                    (400, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorBadRequestExample)),
                    (401, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorUnauthorizedExample)),
                    (403, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorForbiddenExample)),
                    (409, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorConflictExample)),
                    (500, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorInternalServerExample)),
                },
                ["Games.Get"] = new[]
                {
                    (200, typeof(SuccessResponse<GameResponse>), typeof(SuccessResponse<GameResponse>)),
                    (401, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorUnauthorizedExample)),
                    (403, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorForbiddenExample)),
                    (404, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorNotFoundExample)),
                    (500, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorInternalServerExample)),
                },
                ["Games.GetAll"] = new[]
                {
                    (200, typeof(SuccessResponse<IEnumerable<GameResponse>>), typeof(SuccessResponse<GameResponse>)),
                    (401, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorUnauthorizedExample)),
                    (403, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorForbiddenExample)),
                    (500, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorInternalServerExample)),
                },
                ["Auth"] = new[]
                {
                    (200, typeof(SuccessResponse<LoginResponse>), typeof(SuccessResponse<LoginResponse>)),
                    (401, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorUnauthorizedExample)),
                    (403, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorForbiddenExample)),
                    (500, typeof(BaseResponse<NotificationModel>), typeof(GenericErrorInternalServerExample)),
                },

            };

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var attr = context.MethodInfo.GetCustomAttribute<SwaggerResponseProfileAttribute>();
            if (attr == null) return;

            if (!Profiles.TryGetValue(attr.ProfileName, out var responses))
                return;

            foreach (var (status, respType, exampleType) in responses)
            {
                var schema = context.SchemaGenerator.GenerateSchema(respType, context.SchemaRepository);
                var exampleInstance = Activator.CreateInstance(exampleType);
                var exampleValue = exampleType.GetMethod("GetExamples")?.Invoke(exampleInstance, null);

                operation.Responses[status.ToString()] = new OpenApiResponse
                {
                    Description = GetDescription(status),
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = schema,
                            Example = OpenApiAnyFactory.CreateFor(exampleValue)
                        }
                    }
                };
            }
        }

        private static string GetDescription(int status) => status switch
        {
            200 => "OK",
            201 => "Created",
            204 => "No Content",
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            409 => "Conflict",
            500 => "Internal Server Error",
            _ => "Response"
        };
    }

    public static class OpenApiAnyFactory
    {
        public static IOpenApiAny CreateFor(object example)
        {
            var json = System.Text.Json.JsonSerializer.Serialize(example);
            return new Microsoft.OpenApi.Any.OpenApiString(json);
        }
    }
}
