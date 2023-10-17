using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Design;

namespace DataAccessObjects
{
    class A
    {

    }
    public static class Class1
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            //services.AddScoped(A);
        }
    }
}