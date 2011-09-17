using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.IO;

namespace Anito.Data.MongoDBClient
{
    internal class Result
    {
        private BsonReader m_reader = null;

        private int Index { get; set; }
        
        private MongoCursor<BsonDocument> Cursor { get; set; }

        private BsonReader Reader
        {
            get
            {
                return m_reader;
            }
        }
        
        public Result(MongoCursor<BsonDocument> cursor)
        {
            Cursor = cursor;
            Index = -1;
        }

        public object this[int index]
        {
            get
            {
                try
                {
                    return ToNativeType(index);
                }
                catch (System.Exception ex)
                {
                    return new IndexOutOfRangeException();
                }
            }
        }

        public object this[string column]
        {
            get
            {
                try
                {
                    return ToNativeType(column);
                }
                catch (System.Exception ex)
                {
                    throw new Exception.ColumnNotFoundException(column);
                }
            }
        }

        public bool Read()
        {
            Index++;
            if (Index >= Cursor.Count())   
                return false;
            m_reader = BsonReader.Create(Cursor.ElementAt(Index));
            return true;
        }

        private object ToNativeType(string column)
        {
            var value = Cursor.ElementAt(Index)[column];
            switch (Cursor.ElementAt(Index)[column].GetType().Name.ToUpper())
            {                       
                case "BSONBINARY":
                    return (byte[]) value.RawValue;
                case "BSONBOOLEAN":
                    return value.ToBoolean();
                case "BSONDATETIME":
                    return (DateTime)value.RawValue;
                case "BSONDOUBLE":
                    return value.ToDouble();
                case "BSONINT32":
                    return value.ToInt32();
                case "BSONINT64":
                    return value.ToInt64();
                case "BSONSTRING":
                    return value.ToString();                    
                default:
                    return DBNull.Value;
            }
        }

        private object ToNativeType(int index)
        {
            var value = Cursor.ElementAt(Index)[index];
            switch (Cursor.ElementAt(Index)[index].GetType().Name.ToUpper())
            {
                case "BSONBINARY":
                    return (byte[])value.RawValue;
                case "BSONBOOLEAN":
                    return value.ToBoolean();
                case "BSONDATETIME":
                    return (DateTime)value.RawValue;
                case "BSONDOUBLE":
                    return value.ToDouble();
                case "BSONINT32":
                    return value.ToInt32();
                case "BSONINT64":
                    return value.ToInt64();
                case "BSONSTRING":
                    return value.ToString();
                default:
                    return DBNull.Value;
            }
        }
    }
}
