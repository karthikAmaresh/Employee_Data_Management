using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Employee
    {
        [Key]
        public string id { get; set; }
        public int age { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        public string eyeColor { get; set; }
        [Required]
        public string company { get; set; }
        [Required]
        public string email { get; set; }
        public long phone { get; set; }
        public string address { get; set; }
        public string about { get; set; }
    }
}
