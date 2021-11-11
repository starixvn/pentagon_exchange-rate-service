using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ExchangeRate.Subscribers
{
    public record ExchangeRateLog
    {
        public ExchangeRateLog() 
        {
            Time = DateTimeOffset.UtcNow.DateTime;
        }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Time { get; init; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime ExDate { get; init; }

        public int ExId { get; init; }

        public string Source { get; init; }

        public Dictionary<string, decimal> Code { get; init; }
    }
}