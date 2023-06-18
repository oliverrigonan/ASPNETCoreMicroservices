using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomerManagement.DTOs;
using CustomerManagement.Helper;
using CustomerManagement.Interfaces;
using CustomerManagement.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CustomerManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerInterface _customerRepository;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerInterface customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CustomerModel>))]
        public IActionResult GetCustomers([FromQuery] String? keywords, [FromQuery] PagedListParams pagedListParams)
        {
            var pagedListCustomers = _customerRepository.GetPaginatedCustomers(keywords, pagedListParams);
            var customers = _mapper.Map<List<CustomerDTO>>(pagedListCustomers);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(PagedListInfo.Info(pagedListCustomers)));

            return Ok(customers);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CustomerModel))]
        [ProducesResponseType(400)]
        public IActionResult GetCustomer(Int32 id)
        {
            if (_customerRepository.CustomerExists(id) == false)
            {
                return NotFound();
            }

            var customer = _mapper.Map<CustomerDTO>(_customerRepository.GetCustomer(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(customer);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCustomer([FromBody] CustomerDTO customerData)
        {
            if (customerData == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerMap = _mapper.Map<CustomerModel>(customerData);
            if (_customerRepository.CreateCustomer(customerMap) == false)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Customer successfully created");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCustomer(Int32 id, [FromBody] CustomerDTO customerData)
        {
            if (customerData == null)
            {
                return BadRequest(ModelState);
            }

            if (_customerRepository.CustomerExists(id) == false)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerMap = _mapper.Map<CustomerModel>(customerData);
            if (_customerRepository.UpdateCustomer(customerMap) == false)
            {
                ModelState.AddModelError("", "Something went wrong updating customer");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCustomer(Int32 id)
        {
            if (_customerRepository.CustomerExists(id) == false)
            {
                return NotFound();
            }

            var currentCustomer = _customerRepository.GetCustomer(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_customerRepository.DeleteCustomer(currentCustomer) == false)
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return NoContent();
        }
    }
}
