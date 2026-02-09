using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Mzstruct.Base.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mzstruct.DB.Providers.MongoDB.Helpers
{
    public class StringEncrypter : SerializerBase<string>
    {
        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType == BsonType.Null)
            {
                context.Reader.ReadNull();
                return "";
            }
            var encryptedValue = context.Reader.ReadString();
            var decryptedValue = SecurityHelper.Decrypt(encryptedValue); //CryptoHelper.Decrypt(encryptedValue);
            return decryptedValue;
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                context.Writer.WriteString("");
            }
            else
            {
                value = SecurityHelper.Encrypt(value); // CryptoHelper.Encrypt(value);
                context.Writer.WriteString(value);
            }
        }
    }
}
