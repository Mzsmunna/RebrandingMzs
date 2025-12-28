using Mzstruct.Base.Entities;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Tasker.Application.Features.Users
{
    public class User : Identity
    {
        public required string FirstName { get; set; }
        public required string Password { get; set; }
        //public bool IsActive { get; set; } = true;
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public int? Age { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Department { get; set; }
        public string? Designation { get; set; }
        public string? Position { get; set; }
        public string? Img { get; set; }
    }
}
