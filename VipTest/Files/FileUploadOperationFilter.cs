// using Microsoft.OpenApi.Models;
// using Swashbuckle.AspNetCore.SwaggerGen;
//
// namespace VipTest.Files;
//
// public class FileUploadOperationFilter : IOperationFilter
// {
//     public void Apply(OpenApiOperation operation, OperationFilterContext context)
//     {
//         var fileParameters = context.ApiDescription.ParameterDescriptions
//             .Where(p => p.Type == typeof(IFormFile) || p.Type == typeof(List<IFormFile>))
//             .ToList();
//
//         if (fileParameters.Any())
//         {
//             operation.RequestBody = new OpenApiRequestBody
//             {
//                 Content = new Dictionary<string, OpenApiMediaType>
//                 {
//                     ["multipart/form-data"] = new OpenApiMediaType
//                     {
//                         Schema = new OpenApiSchema
//                         {
//                             Type = "object",
//                             Properties = fileParameters.ToDictionary(
//                                 p => p.Name,
//                                 p => new OpenApiSchema
//                                 {
//                                     Type = "string",
//                                     Format = "binary"  // 'binary' format indicates file upload in OpenAPI
//                                 }
//                             )
//                         }
//                     }
//                 }
//             };
//         }
//     }
// }