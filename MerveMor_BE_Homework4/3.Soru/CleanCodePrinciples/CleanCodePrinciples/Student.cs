using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CleanCodePrinciples
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public int Avg { get; set; }
        public bool IsGraduate { get; set; }

        

        public void FirstStudent()
        {
            List<Student> students = new List<Student>();
            students.Add(new Student() { Id = 1, Name = "Merve Mor", Address = "Mecidiyeköy", Age = 24, Avg = 3, IsGraduate = true});
            students.Add(new Student() { Id = 2, Name = "Mert Adatepe", Address = "Zeytinburnu", Age = 25, Avg = 2, IsGraduate = false });
            
            Console.WriteLine(students.Where(c => c.IsGraduate == true).ToString());
            
        }

    }
}
