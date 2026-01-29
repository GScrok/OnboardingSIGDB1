using Microsoft.Extensions.DependencyInjection;
using OnboardingSIGDB1.Data;
using OnboardingSIGDB1.Data.Repositories;
using OnboardingSIGDB1.Domain.AutoMapper;
using OnboardingSIGDB1.Domain.Interfaces;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OnboardingSIGDB1.Domain.Interfaces.Repositories;
using OnboardingSIGDB1.Domain.Interfaces.Repositories.ReadRepositories;
using OnboardingSIGDB1.Domain.Interfaces.Services;
using OnboardingSIGDB1.Domain.Notifications;
using OnboardingSIGDB1.Domain.Services;
using OnboardingSIGDB1.Query.Sqls;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace OnboardingSIGDB1.IOC
{
    public static class Startup
    {
        public static void InitializeServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<INotificator, Notificator>();

            // -- DataBase --
            var dbConnectionString = configuration.GetConnectionString("DefaultConnection");
            
            services.AddDbContext<DataContext>(options =>
                options.UseSqlServer(dbConnectionString));
            
            services.AddScoped<QueryFactory>((sp) => {
                var connection = new SqlConnection(dbConnectionString);

                return new QueryFactory(connection, new SqlServerCompiler());
            });
        }
        
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IRoleService, RoleService>();
        }

        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IEmployeeRoleRepository, EmployeeRoleRepository>();
            
            services.AddScoped<ICompanyReadRepository, CompanyReadSql>();
            services.AddScoped<IEmployeeReadRepository, EmployeeReadSql>();
            services.AddScoped<IRoleReadRepository, RoleReadSql>();
        }
        
        public static void ConfigureMappers(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CompanyProfile));
            services.AddAutoMapper(typeof(EmployeeProfile));
            services.AddAutoMapper(typeof(RoleProfile));
        }
    }
}