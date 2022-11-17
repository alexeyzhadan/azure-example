using AzureExample.Configurations;
using AzureExample.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<MediaBlobStorageSettings>(
    builder.Configuration.GetSection("StorageAccount:MediaBlobStorage"));

builder.Services.AddTransient<BlobService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseRouting();
app.UseEndpoints(endpoints => endpoints.MapControllers());

app.Run();
