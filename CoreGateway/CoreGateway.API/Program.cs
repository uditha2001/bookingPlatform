var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/product/swagger.json", "Product Service");
        c.SwaggerEndpoint("/swagger/cart/swagger.json", "Cart Service");
        c.SwaggerEndpoint("/swagger/order/swagger.json", "Order Service");
        c.SwaggerEndpoint("/swagger/abcadapter/swagger.json", "ABC Adapter");
        c.SwaggerEndpoint("/swagger/cdeadapter/swagger.json", "CDE Adapter");
    });


}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapReverseProxy();


app.Run();
