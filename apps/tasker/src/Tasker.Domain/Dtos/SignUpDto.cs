using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Domain.Dtos;

public record SignUpDto(string firstName, 
    string lastName, 
    string gender,
    DateTime? dob,
    int? age,
    string role,
    string email,
    string password,
    string phone,
    string address,
    string department,
    string designation,
    string position,
    string img
);
