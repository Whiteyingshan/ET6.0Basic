using System;
using MongoDB.Bson.Serialization.Options;

namespace MongoDB.Bson.Serialization.Attributes
{

    public class BsonDictionaryOptionsAttribute : Attribute
    {
        public BsonDictionaryOptionsAttribute(DictionaryRepresentation dictionaryRepresentation)
        {

        }
    }
}