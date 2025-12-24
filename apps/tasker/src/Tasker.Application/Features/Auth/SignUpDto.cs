using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Application.Features.Auth;

public record SignUpDto(string firstName, 
    string lastName, 
    string gender,
    DateTime? dob,
    int? age,
    string role,
    string email,
    string password,
    string confirmPassword,
    string phone,
    string address,
    string department,
    string designation,
    string position,
    string img
);
