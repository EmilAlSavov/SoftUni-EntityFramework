using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        public Student()
        {
            StudentsCourses = new HashSet<StudentCourse>();
            Homeworks = new HashSet<Homework>();
        }
        public int StudentId { get; set; }

        [StringLength(100)]
        [Unicode(true)]
        public string Name { get; set; }

        [StringLength(10)]
        [Unicode(false)]
        public string? PhoneNumber { get; set; }

        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday { get; set; }

        public ICollection<Homework> Homeworks { get; set; }

        public ICollection<StudentCourse> StudentsCourses { get; set; }
    }
}
