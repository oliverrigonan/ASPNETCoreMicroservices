using System;
using ProductManagement.Repositories;
using ProductManagement.Interfaces;

namespace ProductManagement.Helper
{
	public class ScopeService
	{
        private readonly WebApplicationBuilder _builder;

        public ScopeService(WebApplicationBuilder builder)
        {
            _builder = builder;
        }

        public void addScopes()
        {
            _builder.Services.AddScoped<IProductInterface, ProductRepository>();
        }
    }
}

