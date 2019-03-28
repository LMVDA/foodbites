namespace WebProject.Configurations
{
    using Data.DAOS.Context;
    using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;
    using Data.DAOS;
    using Domain.DAOS.Interfaces;
    using Domain.Utilizador;
    using Domain.Pesquisa;
    using Domain.Avaliacao;
    using Domain.Backoffice;
    using Microsoft.CodeAnalysis.CSharp;

    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
			services.AddDbContext<FoodbitesContext>(options =>
		        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			// Add framework services.
			services.AddMvc().AddRazorOptions(options =>
	            options.ParseOptions = new CSharpParseOptions(LanguageVersion.CSharp7));

            services.AddScoped<IAvaliacaoDAO, AvaliacaoDAO>();
            services.AddScoped<IEspecialidadeDAO, EspecialidadeDAO>();
            services.AddScoped<IEstabelecimentoDAO, EstabelecimentoDAO>();
			services.AddScoped<IUtilizadorDAO, UtilizadorDAO>();
            services.AddScoped<IPetiscoDAO, PetiscoDAO>();

            services.AddScoped<UtilizadorFacade, UtilizadorFacade>();
            services.AddScoped<PesquisaFacade, PesquisaFacade>();
			services.AddScoped<AvaliacaoFacade, AvaliacaoFacade>();
			services.AddScoped<BackofficeFacade, BackofficeFacade>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

			app.UseStaticFiles();

			app.UseMvc(routes =>
			{
				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");
			});
        }
    }
}
