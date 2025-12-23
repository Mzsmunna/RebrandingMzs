using Mzstruct.Base.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.Base.Entities
{
    public class IEntity
    {
        public string Id { get; set; } = string.Empty; //ObjectId.GenerateNewId().ToString();
        public AppEvent Created { get; set; } = new AppEvent();
        public AppEvent? Modified { get; set; }
    }
}
