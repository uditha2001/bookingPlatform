using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using ProductService.API.Data;
using ProductService.API.Repository;
using ProductService.API.Repository.RepositoryInterfaces;
using ProductService.API.Services;
using ProductService.API.Services.serviceInterfaces;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductService, ProductServiceImpl>();
builder.Services.AddScoped<IProductRepo,ProductRepositoryIMPL>();
builder.Services.AddScoped<ProductContentServiceIMPL>();
builder.Services.AddScoped<IProductContentRepository,ProductContentRepositoryIMPL>();
builder.Services.AddScoped<IProductContentService, ProductContentServiceIMPL>();
builder.Services.AddScoped<IProductAttributeService,ProductAttributeServiceIMPL>();
builder.Services.AddScoped<IProductAttriuteRepository,ProductAttributeRepositoryIMPL>();
builder.Services.AddDbContext<ProductDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("productConnection")));
//builder.Services.AddQuartz(q =>
//{
//    q.UseMicrosoftDependencyInjectionJobFactory();

//    var jobKey = new JobKey("ProductContentSyncJob");
//    q.AddJob<ProductContentServiceIMPL>(opts => opts.WithIdentity(jobKey));

//    q.AddTrigger(opts => opts
//        .ForJob(jobKey)
//        .WithIdentity("ProductContentSyncTrigger")
//        .WithCronSchedule("0 0 23 ? * SUN") 
//    );
//});


//builder.Services.AddQuartzHostedService();

builder.Services.AddScoped<ProductContentServiceIMPL>();
builder.Services.AddScoped<ProductServiceImpl>();





var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<ProductServiceImpl>();
    await service.UpdateProductsFromAdapterAsync(); 
}



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
var imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

if (!Directory.Exists(imageFolderPath))
{
    Directory.CreateDirectory(imageFolderPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imageFolderPath),
    RequestPath = "/images"
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
