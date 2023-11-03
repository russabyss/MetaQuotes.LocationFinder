using MetaQuotes.LocationFinder.Core.Interfaces;
using MetaQuotes.LocationFinder.Core.Services;
using MetaQuotes.LocationFinder.Core.Settings;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .Configure<DbSettings>(builder.Configuration.GetSection(DbSettings.SectionName))
            .AddSingleton<SearchIndexFactory>()
            .AddSingleton<ISearchEngine, SearchEngineService>();

        // ��� �������
        builder.Services.AddCors(options => options.AddPolicy("ApiCorsPolicy", builder =>
        {
            builder
                .WithOrigins("*")
                .AllowAnyMethod()
                .AllowAnyHeader();
        }));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // TODO: �������, �����������, ����������
        
        var app = builder.Build();

        // ��������� ������������� �� ��� ������ ������,
        // � �� ������ ����������.
        var engine = app.Services.GetRequiredService<ISearchEngine>();
        engine.Initialize();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseCors("ApiCorsPolicy");
        
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}