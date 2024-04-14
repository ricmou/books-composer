using System;
using BookAPIComposer.Services.APIAuthors.Authors;
using BookAPIComposer.Services.APIAuthors.Books;
using BookAPIComposer.Services.APIBooks;
using BookAPIComposer.Services.APICategories.Books;
using BookAPIComposer.Services.APICategories.Categories;
using BookAPIComposer.Services.APIClients;
using BookAPIComposer.Services.APIExemplar;
using BookAPIComposer.Services.APIPublisher.Books;
using BookAPIComposer.Services.APIPublisher.Publishers;
using BookAPIComposer.Services.Composer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookAPIComposer
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
            ConfigureMyServices(services);

            services.AddControllers().AddNewtonsoftJson();
            
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureMyServices(IServiceCollection services)
        {
            if (Convert.ToBoolean(Configuration["UseGrpc"]))
            {
                Console.WriteLine("Communication mode: gRPC");
                //APIAuthors
                services.AddSingleton<IAuthorsService>(dep => new AuthorsGrpcService(Configuration["APIAuthorsURL"]));
                services.AddSingleton<IAuthorsBookService>(dep => new AuthorsBookGrpcService(Configuration["APIAuthorsURL"]));
                
                //APIBooks
                services.AddSingleton<IBookService>(dep => new BookGrpcService(Configuration["APIBooksGrpcURL"]));
                
                //APICategories
                services.AddSingleton<ICategoryService>(dep => new CategoryGrpcService(Configuration["APICategoriesURL"]));
                services.AddSingleton<ICategoryBookService>(dep => new CategoryBookGrpcService(Configuration["APICategoriesURL"]));
                
                //APIClients
                services.AddSingleton<IClientsService>(dep => new ClientsGrpcService(Configuration["APIClientsURL"]));
                
                //APIExemplar
                services.AddSingleton<IExemplarService>(dep => new ExemplarGrpcService(Configuration["APIExemplarURL"]));
                
                //APIPublisher
                services.AddSingleton<IPublisherService>(dep => new PublisherGrpcService(Configuration["APIPublisherURL"]));
                services.AddSingleton<IPublisherBookService>(dep => new PublisherBookGrpcService(Configuration["APIPublisherURL"]));
                
            }
            else
            {
                Console.WriteLine("Communication mode: REST");
                //APIAuthors
                services.AddSingleton<IAuthorsService>(dep => new AuthorsRestService(new Uri(Configuration["APIAuthorsURL"])));
                services.AddSingleton<IAuthorsBookService>(dep => new AuthorsBookRestService(new Uri(Configuration["APIAuthorsURL"])));
                
                //APIBooks
                services.AddSingleton<IBookService>(dep => new BookRestService(new Uri(Configuration["APIBooksURL"])));
                
                //APICategories
                services.AddSingleton<ICategoryService>(dep => new CategoryRestService(new Uri(Configuration["APICategoriesURL"])));
                services.AddSingleton<ICategoryBookService>(dep => new CategoryBookRestService(new Uri(Configuration["APICategoriesURL"])));
                
                //APIClients
                services.AddSingleton<IClientsService>(dep => new ClientsRestService(new Uri(Configuration["APIClientsURL"])));
                
                //APIExemplar
                services.AddSingleton<IExemplarService>(dep => new ExemplarRestService(new Uri(Configuration["APIExemplarURL"])));
                
                //APIPublisher
                services.AddSingleton<IPublisherService>(dep => new PublisherRestService(new Uri(Configuration["APIPublisherURL"])));
                services.AddSingleton<IPublisherBookService>(dep => new PublisherBookRestService(new Uri(Configuration["APIPublisherURL"])));
            }
            
            services.AddSingleton<IBookComposerService, BookComposerService>();
            services.AddSingleton<BookComposerService>();

            services.AddSingleton<IExemplarComposerService, ExemplarComposerService>();
            services.AddSingleton<ExemplarComposerService>();
        }
    }
}
