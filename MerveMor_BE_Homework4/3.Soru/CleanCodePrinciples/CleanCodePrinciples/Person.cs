using System;
using System.Collections.Generic;
using System.Text;

namespace CleanCodePrinciples
{
    public class Person
    {
        public bool Married { get; set; }
        public Person()
        {
            Married = true;
        }

        public void IsMarried(Person person)
        {
            bool married = person.Married;

            if (married)
                Console.WriteLine("Married");
            else
                Console.WriteLine("Not Married");
        }
    }
}
