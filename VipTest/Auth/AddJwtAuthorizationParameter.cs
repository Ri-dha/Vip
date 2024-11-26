// using Microsoft.AspNetCore.Authorization;
// using Microsoft.OpenApi.Models;
// using Swashbuckle.AspNetCore.SwaggerGen;
//
// namespace VipTest.Auth;
//
// public class AddJwtAuthorizationParameter : IOperationFilter
// {
//     public void Apply(OpenApiOperation operation, OperationFilterContext context)
//     {
//         // Check if the endpoint has the [Authorize] attribute
//         var hasAuthorizeAttribute = context.MethodInfo.DeclaringType.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ||
//                                     context.MethodInfo.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any();
//
//         // If the endpoint requires authorization, add the Authorization header parameter
//         if (hasAuthorizeAttribute)
//         {
//             if (operation.Parameters == null)
//                 operation.Parameters = new List<OpenApiParameter>();
//
//             operation.Parameters.Add(new OpenApiParameter
//             {
//                 Name = "Authorization",
//                 In = ParameterLocation.Header,
//                 Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer 12345abcdef'",
//                 Required = true,
//                 Schema = new OpenApiSchema
//                 {
//                     Type = "string"
//                 }
//             });
//         }
//     }
// }
