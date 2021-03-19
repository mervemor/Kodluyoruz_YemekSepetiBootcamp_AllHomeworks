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
                ); //appsettings.json içerisindeki istenen yani HotelInfo section okuyacak

            services.AddDbContext<HotelApiDbContext>(options =>
            {
                options.UseInMemoryDatabase("HotelDB");
            });
            
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(JsonExceptionFilters));
                //options.Filters.Add<AllowOnlyRequireHttps>(); app.UseHttpsRedirection(); ile ayný iþi yapacak.
            });

            services.AddAutoMapper(typeof(MappingProfile));
            //services.AddAutoMapper(option => option.AddProfile<MappingProfile>());
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IUserService, UserService>();

            string key = Configuration.GetValue<string>("JwtTokenKey");
            byte[] keyValue = Encoding.UTF8.GetBytes(key);

            //Derste yaptýðýmýz kýsým
            /*services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                //development sürecinde false olarak set edilebilir. Bize gelen auth isteðinin kesinlikle true yani https olarak gelmesi lazým.
                jwt.RequireHttpsMetadata = true;
                jwt.SaveToken = true; //auth iþleminden sonra bu tokený kaydeder saklar 
                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyValue),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });*/

            //benim eklediðim kýsým
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

            services.AddRouting(options => options.LowercaseUrls = true); //urllerin komple lowercase olmasýný isteriz 

            services.AddSwaggerDocument(); //nuget kýsmýnda nSwag.AspNet.Core yükledik 

            //kullanabilmek için nuget kýsmýndan Microsoft.AspNetCore.Mvc.Versioning install ettik ve AddApiVersioning servisi geliyor
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0); //default versiyonu 1.0 olarak ayarladýk 
                options.AssumeDefaultVersionWhenUnspecified = true; //endpoint’te herhangi bir sürüm belirtilmediði taktirde 1.0 versiyonu varsayýlan olarak kabul edilmektedir.
                options.ReportApiVersions = true; //controller üzerinde birden fazla [ApiVersion("1.1")] version ekleyip bu versiyonlarý kullanýcýyý bilgilendirmek için ReportApiVersions true olmalýdýr.  
                //postmande get yaparken header içerisinden key alanýna accept yazýp value alanýna application/json;v=1.1 yazarak ilgili metotlarý tetikleyebiliriz.
                options.ApiVersionReader = new MediaTypeApiVersionReader();

                /* Versiyonlama stratejileri 3 þekildedir. Biz derste MediaTypeApiVersionReader için olanýn gördük. Sýrayla diðerlerini ekleyeceðim. */
                //2. yöntem olan QueryStringApiVersionReader() eklemesi yapýyorum. Postman de Url'e versiyon bilgisi ekleyip get iþlemini saðlayabiliriz.
                //options.ApiVersionReader = new QueryStringApiVersionReader("api-version");
                //https://localhost:44359/test?api-version=2.0 bu þekilde yazarsak versiyona göre metotlar tetiklenecetir.

                //3. yöntem olan HeaderApiVersionReader'ý ekliyorum. ‘HeaderApiVersionReader’ nesnesine verilen ‘api-version’ deðeri, header’da sürüm bilgisini taþýyacak olan key’e karþýlýk gelmektedir.
                //postmanda get yaparken header kýsmýnda key alanýna api-version, value kýsmýnada versiyonu yazarsak ilgili metotlar tetiklenecektir.
                //options.ApiVersionReader = new HeaderApiVersionReader("api-version");

                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);

                //TestController içerisinde hangi versiyonlar kullanýlabilir hangileri deprecated belirttik ve ayrýca metotlar için kullanýlabilir sürümleri yazdýk
                options.Conventions.Controller<TestController>()
                       .HasDeprecatedApiVersion(1, 0) //deprecated olmuþ version
                       .HasApiVersion(1, 1) //kullanýlabilir versiyon
                       .HasApiVersion(2, 0) //kullanýlabilir versiyon
                       .Action(a => a.GetCustomers()).MapToApiVersion(1, 1) //GetCustomer() metodu 1,1 de çalýþýr
                       .Action(a => a.GetCustomerV2()).MapToApiVersion(2, 0); //GetCustommerV2() metodu 2,0 da çalýþýr 
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwaggerUi3(); //swagger için ekledik
                app.UseOpenApi(); //swagger için ekledik
            }

            app.UseHttpsRedirection();//gelen http requestlerini https olmasý için kontrol saðlar, .net core  gelen bir request için default olarak https kullanýr.

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
