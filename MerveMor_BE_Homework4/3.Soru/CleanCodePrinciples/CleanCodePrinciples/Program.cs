using System;
using System.Collections.Generic;

namespace CleanCodePrinciples
{
    class Program
    {
        static void Main(string[] args)
        {
            #region BooleanKarşılaştırma
            //Rule 1 class --> Person.cs
            //Person person = new Person();
            //person.IsMarried(person);
            #endregion

            #region BooleanDeğerAtamaları
            //Rule 2 class --> NumberGenerator.cs
            //NumberGenerator numberGenerator = new NumberGenerator();
            //numberGenerator.Generator();
            #endregion

            #region PozitifOl
            //Rule 3 class --> LettersGenerator
            //LettersGenerator lettersGenerator = new LettersGenerator();
            //lettersGenerator.CharGenerator();
            #endregion

            #region TernaryIf
            //Rule 4 
            /*
            Product product1 = new Product();
            Product product2 = new Product();

            bool IsEqualObject = product1 == product2;
            string result = IsEqualObject ? "Same Object" : "Different Object";
            Console.WriteLine(result);
            */
            #endregion

            #region StronglyTypeKullan
            //Rule 5 class --> AnimalTypeFinder
            //AnimalTypeFinder animalTypeFinder = new AnimalTypeFinder();
            //animalTypeFinder.AnimalTypeFind();
            #endregion

            #region BaşıBoşİfadelerdenKaçın
            //Rule 6 
            //Console.WriteLine("Enter a number => ");
            //int number = Convert.ToInt32(Console.ReadLine());

            //if(number > 0)
            //    Console.WriteLine($"Number is more than zero : {number}");
            //else
            //    Console.WriteLine($"Number is less than zero : {number}");
            #endregion

            #region AçıklayıcıOl
            //Rule 7 class --> Student.cs Düzgün çalıştıramadım
            //Student student = new Student();
            //student.FirstStudent();
            #endregion

            # region Değişkenler
            //Program program = new Program();
            //program.CalculateIdealKg(156, 60);
            #endregion

            # region Parametreler
            //Program program = new Program();
            //program.UserInfo("Merve", "123", "abc@hotmail.com", false, true, false);
            #endregion


        }

        public void CalculateIdealKg(int a, int b)
        {
            //a --> boy, b --> kilo olsun
            
            int idealKg = a * b / 2;

            if (idealKg < 50)
                Console.WriteLine("Çok zayıf");
            else if (idealKg > 50 && idealKg < 80)
                Console.WriteLine("İdeal");
            else
                Console.WriteLine("Kilolu");

        }

        public void UserInfo(string userName,
                            string password,
                            string email,
                            bool sendEmail,
                            bool sendBill,
                            bool printReport)
        {
            Console.WriteLine(userName);
        }
            
    }    
}
