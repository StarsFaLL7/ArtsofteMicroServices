using Api.RpcApi;
using Api.Saga;
using Application;
using IdentityConnectionLib;
using Infrastructure;
using ProjectCore.HttpLogic;
using ProjectCore.RPCLogic;
using ProjectCore.TraceIdLogic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddHttpRequestService();
builder.Services.AddIdentityConnectionService();
builder.Services.TryAddTraceId();
builder.Services.TryAddRpc();

builder.Services.AddSaga();

builder.Services.TryAddApplicationLayer();
builder.Services.TryAddInfrastructure();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();

app.UseTraceId();
app.UseRpcServerConnectionLib();

app.Run();