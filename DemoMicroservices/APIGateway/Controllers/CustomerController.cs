using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using APIGateway.Helper;
using APIGateway.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APIGateway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly String _apiURL;

        public CustomerController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiURL = "https://localhost:7136";
        }

        [HttpGet]
        public IActionResult GetAllCustomers([FromQuery] String? keywords, [FromQuery] PagedListParams pagedListParams)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                String? token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

                httpClient.BaseAddress = new Uri(_apiURL);
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = httpClient.GetAsync("/api/Customer?keywords="+ keywords + "&PageNumber="+ pagedListParams.PageNumber + "&PageSize="+ pagedListParams.PageSize).Result;
                if (response.IsSuccessStatusCode)
                {
                    var paginationHeader = response.Headers.GetValues("Pagination").FirstOrDefault();
                    Response.Headers.Add("Pagination", JsonConvert.SerializeObject(paginationHeader));

                    String responseBody = response.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<IEnumerable<CustomerModel>>(responseBody);

                    return Ok(data);
                }
                else
                {
                    return NotFound();
                }
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetCustomerById(int id)
        {
            if (IsAuthorized() == false)
            {
                return Unauthorized();
            }

            var customer = _httpClient.GetFromJsonAsync<CustomerModel>($"{_apiURL}/api/Customer/{id}").Result;
            if (customer == null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpPost]
        public IActionResult CreateCustomer(CustomerModel customer)
        {
            if (IsAuthorized() == false)
            {
                return Unauthorized();
            }

            var response = _httpClient.PostAsJsonAsync($"{_apiURL}/api/Customer", customer).Result;
            response.EnsureSuccessStatusCode();

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, CustomerModel updatedCustomer)
        {
            if (IsAuthorized() == false)
            {
                return Unauthorized();
            }

            var response = _httpClient.PutAsJsonAsync($"{_apiURL}/api/Customer/{id}", updatedCustomer).Result;
            response.EnsureSuccessStatusCode();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            if (IsAuthorized() == false) return Unauthorized();

            var response = _httpClient.DeleteAsync($"{_apiURL}/api/Customer/{id}").Result;
            response.EnsureSuccessStatusCode();

            return Ok();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
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
