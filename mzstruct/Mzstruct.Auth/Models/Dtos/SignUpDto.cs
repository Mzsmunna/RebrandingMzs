using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Mzstruct.Auth.Models.Dtos
{
    public record SignUpDto([Required] string FirstName, 
    [Required] string LastName,
    string Gender,
    DateTime? DOB,
    int? Age,
    string Role,
    [Required] string Email,
    [Required] string Password,
    [Required] string ConfirmPassword,
    string Phone,
    string Address,
    string Department,
    string Designation,
    string Position,
    string Img
);
}
