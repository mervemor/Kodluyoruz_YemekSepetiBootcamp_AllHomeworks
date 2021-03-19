using Bogus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIProject.Models;

/*kullanmış olduğum singleton patterni nesne, sınıf belleğe yüklendiği sırada değil, o nesne ilk defa kullanılmak istendiğii sırada yaratılmaktadır. Multi-thread bir uygulama
  olmadığını düşünerekten kullandım, multi-thread olması durumunda derste yaptığımız gibi başka kontrollerinde yapılması gerektiği bir yapı kullanabilirdim. */

namespace WebAPIProject.Data
{
    public class DummyDataSingletonPattern
    {
        private static DummyDataSingletonPattern _dummyDataSingletonPattern;
        private DummyDataSingletonPattern()
        {
            FillData();
        }

        public static DummyDataSingletonPattern SingletonClass
        {
            get
            {
                if (_dummyDataSingletonPattern == null)
                    _dummyDataSingletonPattern = new DummyDataSingletonPattern();
                return _dummyDataSingletonPattern;
            }
        }

        private void FillData()
        {
            for(int i = 0; i <= 10; i++)
            {
                Products.Add(new Product 
                { 
                    Id = i, 
                    ProductName = "Product_" + i, 
                    ProductPrice = 100 + (i * 10), 
                    ProductNumber = i * 5 
                });
            }
        }

        public List<Product> Products = new List<Product>();

    }
}
