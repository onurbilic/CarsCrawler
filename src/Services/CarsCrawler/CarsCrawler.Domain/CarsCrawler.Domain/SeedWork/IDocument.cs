using System;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CarsCrawler.Domain.SeedWork
{
    public interface IDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        ObjectId Id { get; set; }

        DateTime CreatedAt { get; }

        public abstract class Document : IDocument
        {
            [BsonId]
            public ObjectId Id { get; set; }

            public DateTime CreatedAt => Id.CreationTime;
        }
    }
}

