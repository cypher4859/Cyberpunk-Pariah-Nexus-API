using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using CyberpunkPariahNexusApi.Models;

public class Program {
	public static void Main(string[] args) {
		var builder = WebApplication.CreateBuilder(args);

		// Add services to the container.

		builder.Services.AddControllers();
		// builder.Services.AddDbContext<DataContext>(
		//     opt => opt.UseSqlite(builder.Configuration.GetConnectionString("sqlite-database"))
		// );
		builder.Services.AddDbContext<DataContext>(options =>
						options.UseMySql(
							builder.Configuration.GetConnectionString("mysql-database"),
							ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("mysql-database"))
						));

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		var app = builder.Build();

		app.UseSwagger();
		app.UseSwaggerUI();

		app.UseHttpsRedirection();
		
		app.UseAuthorization();

		app.MapControllers();

		app.Run();

	}
}

