using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProductManagement.DTOs;
using ProductManagement.Helper;
using ProductManagement.Interfaces;
using ProductManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ProductManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductInterface _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductInterface productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<ProductModel>))]
        public IActionResult GetProducts([FromQuery] String? keywords, [FromQuery] PagedListParams pagedListParams)
        {
            var pagedListProducts = _productRepository.GetPaginatedProducts(keywords, pagedListParams);
            var products = _mapper.Map<List<ProductDTO>>(pagedListProducts);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Response.Headers.Add("Pagination", JsonConvert.SerializeObject(PagedListInfo.Info(pagedListProducts)));

            return Ok(products);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(ProductModel))]
        [ProducesResponseType(400)]
        public IActionResult GetProduct(Int32 id)
        {
            if (_productRepository.ProductExists(id) == false)
            {
                return NotFound();
            }

            var product = _mapper.Map<ProductDTO>(_productRepository.GetProduct(id));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] ProductDTO productData)
        {
            if (productData == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productMap = _mapper.Map<ProductModel>(productData);
            if (_productRepository.CreateProduct(productMap) == false)
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Product successfully created");
        }

        [HttpPut("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(Int32 id, [FromBody] ProductDTO productData)
        {
            if (productData == null)
            {
                return BadRequest(ModelState);
            }

            if (_productRepository.ProductExists(id) == false)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productMap = _mapper.Map<ProductModel>(productData);
            if (_productRepository.UpdateProduct(productMap) == false)
            {
                ModelState.AddModelError("", "Something went wrong updating product");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProduct(Int32 id)
        {
            if (_productRepository.ProductExists(id) == false)
            {
                return NotFound();
            }

            var currentProduct = _productRepository.GetProduct(id);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_productRepository.DeleteProduct(currentProduct) == false)
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return NoContent();
        }
    }
}
