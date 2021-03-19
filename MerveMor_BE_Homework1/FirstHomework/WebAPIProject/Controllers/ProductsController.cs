using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIProject.Data;
using WebAPIProject.Models;

//ilk başta normal class oluşturup inheritance ile sınıfı controllera çeviriyoruz
namespace WebAPIProject.Controller
{
    [Route("api/[controller]")] //products ile başlayan tüm urller products controllerına çağırılacak
    public class ProductsController : ControllerBase //kullanabilmek için using Microsoft.AspNetCore.Mvc; ekledik 
    {
        private readonly DummyDataSingletonPattern _dummyDataSingletonPattern;

        public ProductsController()
        {
            _dummyDataSingletonPattern = DummyDataSingletonPattern.SingletonClass;
        }

        [HttpGet]
        public List<Product> Get()
        {
            return _dummyDataSingletonPattern.Products;
        }

        [HttpGet("{id}")]
        public Product Get(int id)
        {
            var product = _dummyDataSingletonPattern.Products.FirstOrDefault(x => x.Id == id);
            return product;
        }

        [HttpPost]
        public Product Post([FromBody] Product product)
        {
            _dummyDataSingletonPattern.Products.Add(product);
            return product;
        }

        [HttpPut]
        public Product Put([FromBody] Product product)
        {
            var editedProduct = _dummyDataSingletonPattern.Products.FirstOrDefault(x => x.Id == product.Id);
            editedProduct.ProductName = product.ProductName;
            editedProduct.ProductPrice = product.ProductPrice;
            editedProduct.ProductNumber = product.ProductNumber;
            return product;
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var deletedProduct = _dummyDataSingletonPattern.Products.FirstOrDefault(x => x.Id == id);
            _dummyDataSingletonPattern.Products.Remove(deletedProduct);
        }

        //Anladığım kadarıyla frontend tarafında geliştiriciye requestler için hangi isteklerde bulunabileceğine dair bir bilgi sorgu sonrası header kısmında gözüküyor. Swagger tarzında. 
        [HttpOptions]
        public IActionResult Options()
        {
            Response.Headers.Add("Allow", "GET, POST, PUT, DELETE, OPTIONS");
            return Ok();
        }

    }
}
