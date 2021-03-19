using CleanCodePrinciples.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleanCodePrinciples
{
    public class AnimalTypeFinder
    {

        public void AnimalTypeFind()
        {
            AnimalType choosenAnimalType = AnimalType.Bird;

            if (choosenAnimalType == AnimalType.Bird)
                Console.WriteLine("Bird");
            else if (choosenAnimalType == AnimalType.Cat)
                Console.WriteLine("Cat");
            else if (choosenAnimalType == AnimalType.Dog)
                Console.WriteLine("Dog");
            else if (choosenAnimalType == AnimalType.Fish)
                Console.WriteLine("Fish");
            else 
                Console.WriteLine("None");

        }

    }
}
