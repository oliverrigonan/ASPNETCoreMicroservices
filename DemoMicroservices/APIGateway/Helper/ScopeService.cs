using System;
using APIGateway.Repositories;
using APIGateway.Interfaces;

namespace APIGateway.Helper
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
            _builder.Services.AddScoped<IAccountInterface, AccountRepository>();
        }
    }
}

