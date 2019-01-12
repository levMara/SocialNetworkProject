using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication4.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    
    public class UserModel
    {
        public UserModel( string fullName, DateTime? birthDate, string city, string workPlace)
        {
            FullName = fullName;
            BirthDate = birthDate;
            City = city;
            WorkPlace = workPlace;
        }

        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string City { get; set; }
        public string WorkPlace { get; set; }
    }

}