using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PhoneNumberFormatter.API.Interfaces.Services.Formatting;
using PhoneNumberFormatter.API.Interfaces.Services.User;
using PhoneNumberFormatter.API.Middleware;
using PhoneNumberFormatter.API.Services.Formatting;
using PhoneNumberFormatter.API.Services.User;
using PhoneNumberFormatter.FormattingRepository.Interfaces;
using PhoneNumberFormatter.FormattingRepository.Stores;
using PhoneNumberFormatter.Hashing.Interfaces;
using PhoneNumberFormatter.Hashing.Passwords;
using PhoneNumberFormatter.UserRepository.Interfaces;
using PhoneNumberFormatter.UserRepository.Stores;

namespace PhoneNumberFormatter.API
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
            services.AddControllers();

            services.AddSwaggerGen();

            ConfigureAuthentication(services);

            ConfigureApplicationDI(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Phone Number Formatter v1");
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            // Configure basic authentication 
            services.AddAuthentication("BasicAuthentication")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);
        }

        private void ConfigureApplicationDI(IServiceCollection services)
        {
            services.AddSingleton<IPasswordHasher, PasswordHasher>();

            services.AddSingleton<IUserVerificationService, UserVerificationService>();
            services.AddSingleton<IPhoneNumberFormattingService, PhoneNumberFormattingService>();

            services.AddSingleton<IGetUserStore, StubGetUserStore>();
            services.AddSingleton<IPhoneNumberFormatsStore, PhoneNumberFormatsStore>();
        }
    }
}
