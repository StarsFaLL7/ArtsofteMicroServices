using IdentityApi.RpcApi;
using IdentityApi.Saga;
using IdentityDal;
using IdentityLogic;
using ProjectCore.RPCLogic;
using ProjectCore.TraceIdLogic;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.TryAddLogic();
builder.Services.TryAddDal();
builder.Services.TryAddTraceId();
builder.Services.TryAddRpc();

builder.Services.AddSaga();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseTraceId();
app.UseRpcServer();

app.MapControllers();

app.Run();
