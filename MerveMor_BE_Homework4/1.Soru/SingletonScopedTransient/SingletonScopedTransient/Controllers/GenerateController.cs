using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SingletonScopedTransient.Services.Interfaces;

/*  Singleton: Uygulama bazlı tekil nesne oluşturur. Tüm taleplere container içerisinden aynı nesneyi döndürür. 
    Scoped: Her request başına bir nesne üretir ve o request pipeline'nında olan tüm isteklere o nesneyi gönderir. 
    Transient : Her request'in her talebine karşılık yeni bir nesne üretir ve gönderir. En maliyetlisidir. 
*/ 

namespace SingletonScopedTransient.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GenerateController : ControllerBase
    {
        private readonly ITransientService _transientService1;
        private readonly ITransientService _transientService2;
        private readonly IScopedService _scopedService1;
        private readonly IScopedService _scopedService2;
        private readonly ISingletonService _singletonService1;
        private readonly ISingletonService _singletonService2;
        public GenerateController(ITransientService transientService1,
                                  ITransientService transientService2,
                                  IScopedService scopedService1,
                                  IScopedService scopedService2,
                                         ISingletonService singletonService1,
                                         ISingletonService singletonService2)
        {
            _transientService1 = transientService1;
            _transientService2 = transientService2;
            _scopedService1 = scopedService1;
            _scopedService2 = scopedService2;
            _singletonService1 = singletonService1;
            _singletonService2 = singletonService2;
        }

        [HttpGet]
        public string Get()
        {
            string result = $"Transient 1 = {_transientService1.generateNumber} {Environment.NewLine}" +
                            $"Transient 2 = {_transientService2.generateNumber} {Environment.NewLine}" +
                            $"Scoped 1    = {_scopedService1.generateNumber } {Environment.NewLine}" +
                            $"Scoped 2    = {_scopedService2.generateNumber} {Environment.NewLine}" +
                            $"Singleton 1  = {_singletonService1.generateNumber } {Environment.NewLine}" +
                            $"Singleton 2    = {_singletonService2.generateNumber} {Environment.NewLine}";

            return result;

        }
    }
}