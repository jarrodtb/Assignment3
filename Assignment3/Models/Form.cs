using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Assignment3.Models
{
    public class Form
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "You must enter a first name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "You must enter a last name")]
        public string LastName { get; set; }

        [Display(Name = "Streed Address")]
        [Required(ErrorMessage = "You must enter a street address")]
        public string StreetAddress { get; set; }

        [Display(Name = "State (Capital letters only)")]
        [Required(ErrorMessage = "You must enter a state")]
        [RegularExpression(@"A[KLRZ]|C[AOT]|D[CE]|FL|GA|HI|I[ADLN]|K[SY]|LA|M[ADEINOST]|N[CDEHJMVY]|O[HKR]|P[AR]|RI|S[CD]|T[NX]|UT|V[AIT]|W[AIVY]", ErrorMessage = "Enter capital two-letter state abbreviation only")]
        public string State { get; set; }

        [Display(Name = "Zip Code (xxxxx only)")]
        [Required(ErrorMessage = "You must enter a zip code")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid zip code")]
        public string Zip { get; set; }

        [Display(Name = "Phone Number (xxxxxxxxxx only)")]
        [Required(ErrorMessage = "You must enter a phone number")]
        [StringLength(10, ErrorMessage ="xxxxxxxxxx format only")]
        [Phone(ErrorMessage = "You must enter a valid phone number")]
        public string Phone { get; set; }

        [Display(Name = "Email Address")]
        [Required(ErrorMessage = "You must enter an email address")]
        [EmailAddress(ErrorMessage = "You must entor a valid email address")]
        public string Email { get; set; }

        [Display(Name = "Date of Birth (mm/dd/yyyy)")]
        [Required(ErrorMessage = "You must enter a date of birth")]
        [DateOfBirth.MinimumAge(18, ErrorMessage = "You must be at least 18 years old")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DOB { get; set; }

        [Display(Name = "Message (optional)")]  
        public string Message { get; set; }
    }
}