using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.Helpers
{
    public static class DBHelper
    {
        public static bool IsBsonObjectId(this string? value) => ObjectId.TryParse(value, out _);
    }
}
