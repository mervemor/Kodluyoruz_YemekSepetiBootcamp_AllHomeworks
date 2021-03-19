using SingletonScopedTransient.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SingletonScopedTransient.Services
{
    public class NumberGenerator : ISingletonService, IScopedService, ITransientService
    {
        public int generateNumber { get; }
        public NumberGenerator()
        {
            //Random randomText = new Random();
            //string letters = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZabcçdefgğhıijklmnoöprsştuüvyz";
            //string generateText = "";
            //for (int i = 0; i < 6; i++)
            //    generateText += letters[randomText.Next(letters.Length)];

            generateNumber = new Random().Next(100000);
  
        }
    }
}
