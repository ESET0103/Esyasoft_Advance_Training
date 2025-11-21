using System.Linq;
using System.Collections.Generic;

using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.Json;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace HealthHub.Data.Entities

{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string UserType { get; set; }
        = string.Empty;
        [Required]
        public string Gender { get; set; }
        = string.Empty;
        [Required]
        public string Phone { get; set; }
            
        //List<Users> user = new List<Users> { 
        //    new Users { Id = 101, Name = "ddddd", Email = "dknowief@gmail.com", Password = "123456", UserType = "Patient", 
        //        Gender = "Female", Phone = "1234567891" },
        //    new Users { Id = 102, Name = "yyyyy", Email = "dknowief@gmail.com", Password = "123456", UserType = "Patient",
        //        Gender = "Male", Phone = "1234567891" },
        //    new Users { Id = 103, Name = "ttttt", Email = "dknowief@gmail.com", Password = "123456", UserType = "Patient",
        //        Gender = "Female", Phone = "1234567891" }
        //};

    }

  




}
