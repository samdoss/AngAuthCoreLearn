using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebBoard.DataAccess.EntityFramework.Interface;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Http;
using WebBoard.DataAccess.EntityFramework.Implement;
using WebBoard.Logic.UnitOfWork.Interface;
using WebBoard.Logic.UnitOfWork.Implement;
using VersionRouting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebBoard.AuthResultsServer;
using WebBoardAuth.Logic;
using WebBoardAuth.Logic.Models;
using WebBoardAuth.DataAccess.Sql;
using WebBoardAuth.Api.Formatters;

namespace WebBoard
{
    public class Startup
    {
        #region Resault Server  Initial Client Id & Secret Key


        /*
       ***************** Initial Client Id & Secret Key *************** 
       */
        private TokenAuthOptions tokenOptions;
        private SymmetricSecurityKey _symmetricSecurityKey;
        public string client_id = "03162ea57ec34b578f1190a9d783d9df";
        public string secret_key = "pmWkWSBCL51Bfkhn79xPuKBKHz//H6B+mY6G9/eieuM=";
        #endregion
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
            //.AddEnvironmentVariables();

            if (env.IsEnvironment("Development"))
            {
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();

#if DEBUG
            Environment.SetEnvironmentVariable("isDebug", "true");
#else
                             Environment.SetEnvironmentVariable("isDebug", "false");
#endif
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AuthContext>
               (options => options
                   .UseSqlServer("Data Source=DESKTOP-KR0EJIO;Initial Catalog=WebBoardAuth;Persist Security Info=True;User ID=sa;Password=123456789"));

            #region Resault server
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);

            // Add Token SecurityKey
            _symmetricSecurityKey = new SymmetricSecurityKey(Convert.FromBase64String(secret_key));
            //_symmetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes( secret_key));
            tokenOptions = new TokenAuthOptions()
            {
                Audience = client_id,
                Issuer = "iAmWebBoardNetCore",
                SigningCredentials = new SigningCredentials(_symmetricSecurityKey, "HS256"),
                Secret_key = secret_key
            };
            services.AddSingleton<TokenAuthOptions>(tokenOptions);
            services.AddAuthorization(auth =>
            {
                auth.AddPolicy("WebBoardAuthorize", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                    .RequireAuthenticatedUser().Build());
            });



            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Audience = Configuration.GetSection("TokenProviderOptions:Audience").Value;
                options.ClaimsIssuer = Configuration.GetSection("TokenProviderOptions:Issuer").Value;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = _symmetricSecurityKey,
                    ValidAudience = tokenOptions.Audience,
                    ValidIssuer = tokenOptions.Issuer,
                    ValidateLifetime = true
                };
                options.SaveToken = true;
            });
            #endregion

            #region Auth Server
           
             
            services.AddIdentity<ApplicationUser, IdentityRole>(
                options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 6;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                    options.Lockout.MaxFailedAccessAttempts = 5;

                    // INITIALIZE TOKEN PROVIDER DESCRIPTOR FOR PRE-RESET PASSWORD'S TIME SPAN
                    options.Tokens.ProviderMap.Add("Default", new TokenProviderDescriptor(typeof(ITokenProviderDescriptor<ApplicationUser>)));
                })
                .AddEntityFrameworkStores<AuthContext>()
                .AddDefaultTokenProviders();

            //services.AddMvc();
            services.AddMvc(options =>
            {
                options.InputFormatters.Insert(0, new JilInputFormatter());
                options.OutputFormatters.Insert(0, new JilOutputFormatter());
                options.Conventions.Add(new NameSpaceVersionRoutingConvention("api"));
            });

            //add swagger interactive documentation
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Info { Title = "WebBoard Auth API", Version = "v1" });
            });

            //add permission enable cross-origin requests (CORS) from angular
            var corsBuilder = new CorsPolicyBuilder();
            corsBuilder.AllowAnyHeader();
            corsBuilder.AllowAnyMethod();
            corsBuilder.AllowAnyOrigin();
            corsBuilder.AllowCredentials();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", corsBuilder.Build());

            });

            //services.AddSingleton<IRedisConnectionMultiplexer, RedisConnectionMultiplexer>();
            services.AddScoped<AuthDbConnection>();

            #endregion


            services.AddScoped<IEntityFrameworkContext, EntityFrameworkContext>();
            services.AddScoped<IEntityUnitOfWork, EntityUnitOfWork>();

            services.AddScoped<Logic.UnitOfWork.Interface.ILogicUnitOfWork, Logic.UnitOfWork.Implement.LogicUnitOfWork>();
            services.AddScoped<WebBoardAuth.Logic.ILogicUnitOfWork, WebBoardAuth.Logic.LogicUnitOfWork>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseIdentity();
            #region AuthServer
            #endregion

            #region Auth api
            app.UseApplicationInsightsRequestTelemetry();
            app.UseApplicationInsightsExceptionTelemetry();


            app.UseWebBoardAuthorize(tokenOptions);

            #endregion

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCors("AllowAll");
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseStatusCodePages();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }

        private void ConfigureAuth(IApplicationBuilder app)
        {
            ///******* External Login (Facebook) ********/
            //app.UseFacebookAuthentication(new FacebookOptions
            //{
            //    AppId = "1765051440400250",
            //    AppSecret = "b2a560cc256cd31e4db5c568b6c63ece"
            //});

            ///******* External Login (Google) ********/
            //app.UseGoogleAuthentication(new GoogleOptions
            //{
            //    AccessType = "offline",
            //    ClientId = "13353494596-pnag6dm234vec63fc5ubgr178jae2mft.apps.googleusercontent.com",
            //    ClientSecret = "LPXLd59b0nNNY4PXySCKpLCh"
            //});

            ///******* External Login (Microsoft Account) ********/
            //app.UseMicrosoftAccountAuthentication(new MicrosoftAccountOptions
            //{
            //    ClientId = "99d0613a-ad69-49cd-9541-ed80d2c2299c",  //Application Id
            //    ClientSecret = "5ePWa6tTbVOQohUaq6Cyi2a" //Application Secrets
            //});

            ///******* External Login (Twitter) ********/
            //app.UseTwitterAuthentication(new TwitterOptions
            //{
            //    RetrieveUserDetails = true,
            //    ConsumerKey = "FdlS1MLZQrnux0ZKSVj2FC935",
            //    ConsumerSecret = "MOKBnUv1PR2qrWMOuLqeT63n9xSQFnp8vAaxLZKnM6TuHnYtQA"
            //});

        }

        public static Task<SigningCredentials> GetHashSecret(string secretKey)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));
            return Task.FromResult(new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));
        }
    }

    internal interface ITokenProviderDescriptor<T>
    {
    }

}
