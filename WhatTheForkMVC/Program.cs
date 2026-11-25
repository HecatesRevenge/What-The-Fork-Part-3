using Anthropic;
using Microsoft.Extensions.AI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

String apiKey = builder.Configuration["Anthropic:ApiKey"];
String model = builder.Configuration["Anthropic:Model"];
IChatClient chatClient = new AnthropicClient { ApiKey = apiKey }
    //cap tokens 20 2048
    .AsIChatClient(model, 2048)
    .AsBuilder()
    //used by claude to call methods
    .UseFunctionInvocation()
    .Build();
builder.Services.AddSingleton(chatClient);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");


app.Run();
