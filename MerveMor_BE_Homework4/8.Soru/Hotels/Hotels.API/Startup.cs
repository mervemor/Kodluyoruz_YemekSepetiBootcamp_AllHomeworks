using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotels.API.Contexts;
using Hotels.API.Controllers;
using Hotels.API.Filters;
using Hotels.API.Infrastructure;
using Hotels.API.Models;
using Hotels.API.Models.Derived;
using Hotels.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc.Versioning.Conventions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Hotels.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<HotelInfo>(
                Configuration.GetSection("HotelInfo")
                ); //appsettings.json i�erisindeki istenen yani HotelInfo section okuyacak

            services.AddDbContext<HotelApiDbContext>(options =>
            {
                options.UseInMemoryDatabase("HotelDB");
            });
            
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(JsonExceptionFilters));
                //options.Filters.Add<AllowOnlyRequireHttps>(); app.UseHttpsRedirection(); ile ayn� i�i yapacak.
            });

            services.AddAutoMapper(typeof(MappingProfile));
            //services.AddAutoMapper(option => option.AddProfile<MappingProfile>());
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IUserService, UserService>();

            string key = Configuration.GetValue<string>("JwtTokenKey");
            byte[] keyValue = Encoding.UTF8.GetBytes(key);

            //Derste yapt���m�z k�s�m
            /*services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                //development s�recinde false olarak set edilebilir. Bize gelen auth iste�inin kesinlikle true yani https olarak gelmesi laz�m.
                jwt.RequireHttpsMetadata = true;
                jwt.SaveToken = true; //auth i�leminden sonra bu token� kaydeder saklar 
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyValue),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });*/

            //benim ekledi�im k�s�m
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(option =>
            {
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true, 
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Token:Issuer"],
                    ValidAudience = Configuration["Token:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Token:SecurityKey"])),
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddRouting(options => options.LowercaseUrls = true); //urllerin komple lowercase olmas�n� isteriz 

            services.AddSwaggerDocument(); //nuget k�sm�nda nSwag.AspNet.Core y�kledik 

            //kullanabilmek i�in nuget k�sm�ndan Microsoft.AspNetCore.Mvc.Versioning install ettik ve AddApiVersioning servisi geliyor
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0); //default versiyonu 1.0 olarak ayarlad�k 
                options.AssumeDefaultVersionWhenUnspecified = true; //endpoint�te herhangi bir s�r�m belirtilmedi�i taktirde 1.0 versiyonu varsay�lan olarak kabul edilmektedir.
                options.ReportApiVersions = true; //controller �zerinde birden fazla [ApiVersion("1.1")] version ekleyip bu versiyonlar� kullan�c�y� bilgilendirmek i�in ReportApiVersions true olmal�d�r.  
                //postmande get yaparken header i�erisinden key alan�na accept yaz�p value alan�na application/json;v=1.1 yazarak ilgili metotlar� tetikleyebiliriz.
                options.ApiVersionReader = new MediaTypeApiVersionReader();

                /* Versiyonlama stratejileri 3 �ekildedir. Biz derste MediaTypeApiVersionReader i�in olan�n g�rd�k. S�rayla di�erlerini ekleyece�im. */
                //2. y�ntem olan QueryStringApiVersionReader() eklemesi yap�yorum. Postman de Url'e versiyon bilgisi ekleyip get i�lemini sa�layabiliriz.
                //options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
                //https://localhost:44359/test?api-version=2.0 bu �ekilde yazarsak versiyona g�re metotlar tetiklenecetir.

                //3. y�ntem olan HeaderApiVersionReader'� ekliyorum. �HeaderApiVersionReader� nesnesine verilen �api-version� de�eri, header�da s�r�m bilgisini ta��yacak olan key�e kar��l�k gelmektedir.
                //postmanda get yaparken header k�sm�nda key alan�na api-version, value k�sm�nada versiyonu yazarsak ilgili metotlar tetiklenecektir.
                //options.ApiVersionReader = new HeaderApiVersionReader("api-version");

                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);

                //TestController i�erisinde hangi versiyonlar kullan�labilir hangileri deprecated belirttik ve ayr�ca metotlar i�in kullan�labilir s�r�mleri yazd�k
                options.Conventions.Controller<TestController>()
                       .HasDeprecatedApiVersion(1, 0) //deprecated olmu� version
                       .HasApiVersion(1, 1) //kullan�labilir versiyon
                       .HasApiVersion(2, 0) //kullan�labilir versiyon
                       .Action(a => a.GetCustomers()).MapToApiVersion(1, 1) //GetCustomer() metodu 1,1 de �al���r
                       .Action(a => a.GetCustomerV2()).MapToApiVersion(2, 0); //GetCustommerV2() metodu 2,0 da �al���r 
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUi3(); //swagger i�in ekledik
                app.UseOpenApi(); //swagger i�in ekledik
            }

            app.UseHttpsRedirection();//gelen http requestlerini https olmas� i�in kontrol sa�lar, .net core  gelen bir request i�in default olarak https kullan�r.

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
