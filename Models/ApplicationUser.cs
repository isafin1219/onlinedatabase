using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OnlineDatabase.Models
{    
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        public string Surname { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get { return Name + " " + Surname; } }

        [Required]
        [StringLength(50)]
        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Required]
        [Display(Name = "Stations (IATA codes)")]
        public string Stations { get; set; }

        [NotMapped]
        public IList<string> Roles { get; set; }
    }
}
