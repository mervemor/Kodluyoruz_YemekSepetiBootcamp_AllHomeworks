using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using Hotels.API.Models;

namespace Hotels.API.Filters
{
    public class JsonExceptionFilters : IExceptionFilter
    {
        private readonly IWebHostEnvironment _env;

        public JsonExceptionFilters(IWebHostEnvironment env)
        {
            _env = env;
        }
        public void OnException(ExceptionContext context)
        {
            //postmandan gelen ve production ortamından gelen exception farklı olsun istiyoruz. 
            var isDevelopment = _env.IsDevelopment();
            
            var err = new ApiError
            {
                Version = context.HttpContext.GetRequestedApiVersion(),
                Message = isDevelopment ? context.Exception.Message : "Api Error",
                Detail = isDevelopment ? context.Exception.StackTrace : context.Exception.Message
            };

            context.Result = new ObjectResult(err) { StatusCode = 500 };
        }
    }
}
