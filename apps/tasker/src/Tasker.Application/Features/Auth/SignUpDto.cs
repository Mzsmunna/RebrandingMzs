using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Features.Auth;

public record SignUpDto(string FirstName, 
    string LastName, 
    string Gender,
    DateTime? DOB,
    int? Age,
    string Role,
    string Email,
    string Password,
    string ConfirmPassword,
    string Phone,
    string Address,
    string Department,
    string Designation,
    string Position,
    string Img
);
