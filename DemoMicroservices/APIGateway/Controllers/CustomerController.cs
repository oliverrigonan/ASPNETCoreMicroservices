using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using APIGateway.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private const String microserviceUrl = "http://localhost:5001";
        private readonly HttpClient _httpClient;

        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public IActionResult GetAllCustomers()
        {
            if (IsAuthorized() == false) return Unauthorized();

            var customers = _httpClient.GetFromJsonAsync<List<CustomerModel>>($"{microserviceUrl}/api/customers").Result;
            if (customers == null)
            {
                return NotFound();
            }

            return Ok(customers);
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id)
        {
            if (IsAuthorized() == false) return Unauthorized();

            var customer = _httpClient.GetFromJsonAsync<CustomerModel>($"{microserviceUrl}/api/customers/{id}").Result;
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        public IActionResult CreateCustomer(CustomerModel customer)
        {
            if (IsAuthorized() == false) return Unauthorized();

            var response = _httpClient.PostAsJsonAsync($"{microserviceUrl}/api/customers", customer).Result;
            response.EnsureSuccessStatusCode();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, CustomerModel updatedCustomer)
        {
            if (IsAuthorized() == false) return Unauthorized();

            var response = _httpClient.PutAsJsonAsync($"{microserviceUrl}/api/customers/{id}", updatedCustomer).Result;
            response.EnsureSuccessStatusCode();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            if (IsAuthorized() == false) return Unauthorized();

            var response = _httpClient.DeleteAsync($"{microserviceUrl}/api/customers/{id}").Result;
            response.EnsureSuccessStatusCode();

            return Ok();
        }

        public Boolean IsAuthorized()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return true;
        }
    }
}
