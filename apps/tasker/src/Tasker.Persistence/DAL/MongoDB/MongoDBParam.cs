using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tasker.Persistence.DAL.MongoDB
{
    public class MongoDBParam : BsonDocument
    {
        public BsonDocument Parameters
        {
            get
            {
                return bsonDocument;
            }
        }
        
        private readonly BsonDocument bsonDocument;
        
        public MongoDBParam()
        {
            bsonDocument = new BsonDocument();
        }

        public void AddParameter(string parameterName, string value)
        {
            bsonDocument.Add(new BsonElement(parameterName, value));
        }

        public void AddParameter(string parameterName, string value, bool IsIdentity)
        {
            if (IsIdentity)
            {
                bsonDocument.Add(new BsonElement(parameterName, ObjectId.Parse(value)));
            }
            else
                AddParameter(parameterName, value);
        }

        public void AddParameter(string parameterName, bool value)
        {
            bsonDocument.Add(new BsonElement(parameterName, value));
        }
    }
}
