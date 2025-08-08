using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using System;

namespace FGC.Api.Swagger.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class SwaggerResponseProfileAttribute : Attribute
    {
        public string ProfileName { get; }

        public SwaggerResponseProfileAttribute(string profileName)
        {
            ProfileName = profileName;
        }
    }
}
