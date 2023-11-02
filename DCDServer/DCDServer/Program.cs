using DCDServer.Hubs;
using DCDServer.Models;
using DCDServer.Subscription;
using DCDServer.Subscription.Middlewares;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowCredentials().SetIsOriginAllowed(x => true);
    });
});
builder.Services.AddSignalR();
builder.Services.AddSingleton<DatabaseSubscription<Employee>>();
builder.Services.AddSingleton<DatabaseSubscription<Sale>>();


var app = builder.Build();

app.UseDatabaseSubscription<DatabaseSubscription<Sale>>("Sales");
app.UseDatabaseSubscription<DatabaseSubscription<Employee>>("Employees");

app.UseCors();
app.UseRouting();
app.MapHub<SalesHub>("/salesHub");

app.Run();

