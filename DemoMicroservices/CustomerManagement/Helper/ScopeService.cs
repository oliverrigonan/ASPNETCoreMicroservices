﻿using System;
using CustomerManagement.Repositories;
using CustomerManagement.Interfaces;

namespace CustomerManagement.Helper
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
            _builder.Services.AddScoped<ICustomerInterface, CustomerRepository>();
        }
    }
}

