using dotnet_webgrpc.Repository;
using dotnet_webgrpc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
const string corsPolicy = "_corsPolicy";
builder.Services.AddCors(o => o.AddPolicy(corsPolicy, policy =>
{
    policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    // .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
}));

builder.Services.AddTransient<ITodoRepository, TodoRepository>();

var app = builder.Build();

app.UseRouting();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.UseCors(corsPolicy);

app.MapGrpcService<GreeterService>().EnableGrpcWeb();
app.MapGrpcService<TodoService>().EnableGrpcWeb().RequireCors();
app.MapGet("/",
    () =>
        "This gRPC service is gRPC-Web enabled, CORS enabled, and is callable from browser apps using the gRPC-Web protocol");
app.Run();