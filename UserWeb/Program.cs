using UserWeb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<UserService>();

builder.Services.AddSingleton<ExceptionLogger>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(o => o.AddPolicy("Cors", builder =>
{
    builder
        .WithOrigins("http://127.0.0.1:5500")
        .WithHeaders("*")
        .WithMethods("*")
        .AllowCredentials();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Cors");


app.Use((context, next) =>
{
    context.Request.EnableBuffering(); // ���������� ��� ����������� �������� ��� ����������� � ���������� ����������� 
    return next();
});
app.UseUserExceptionHandler(); // ���������� ����������

app.MapControllers();

app.Run();