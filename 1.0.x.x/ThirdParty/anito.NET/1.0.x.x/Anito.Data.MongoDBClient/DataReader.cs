using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;
using MongoDB.Driver;
using MongoDB.Bson;

namespace Anito.Data.MongoDBClient
{
    internal sealed class DataReader : DbDataReader, IDataRecord
    {
        private ResultSet m_resultSet = null;
        private Result m_currentResult = null;

        private int ResultIndex { get; set; }
        private int RowIndex { get; set; }
        
        private ResultSet ResultSet
        {
            get
            {
                return m_resultSet;
            }
        }

        public Result CurrentResult
        {
            get
            {
                return m_currentResult;   
            }
        }

        internal DataReader(ResultSet resultSet)
        {
            ResultIndex = -1;
            m_resultSet = resultSet;
            NextResult();
            
        }
        
		public override int Depth
		{
			get { return 0; }
		}

		public override int FieldCount
		{
			get { throw new NotImplementedException(); }
		}

		public override bool HasRows
		{
			get { throw new NotImplementedException(); }
		}

		public override bool IsClosed
		{
			get { throw new NotImplementedException(); }
		}

		public override int RecordsAffected
		{
			get { throw new NotImplementedException(); }
		}

		public override object this[int i]
		{
            get
            {
                return CurrentResult[i];
            }
		}

		public override object this[String name]
		{
            get
            {
                return CurrentResult[name];
            }
		}


		public override void Close()
		{

			//throw new NotImplementedException();
		}

		#region TypeSafe Accessors

		public bool GetBoolean(string name)
		{
            return (bool)this[name];
		}

		public override bool GetBoolean(int i)
		{
            return (bool)this[i];
		}

		public byte GetByte(string name)
		{
            return (byte)this[name];
		}

		public override byte GetByte(int i)
		{
            return (byte)this[i];
		}

        public sbyte GetSByte(string name)
        {
            return (sbyte)this[name];
        }


        public sbyte GetSByte(int i)
        {
            return (sbyte)this[i];
        }

		public override long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
            return (long)this[i];
		}

		public char GetChar(string name)
		{
            return (char)this[name];
		}

		public override char GetChar(int i)
		{
            return (char)this[i];
		}

		public override long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
            return (long)this[i];
		}

		public override String GetDataTypeName(int i)
		{
            return this[i].GetType().FullName;
		}

		public DateTime GetDateTime(string column)
		{
            return (DateTime) this[column];
		}

		public override DateTime GetDateTime(int i)
		{
            return (DateTime)this[i];
		}

		public Decimal GetDecimal(string column)
		{
			return GetDecimal(GetOrdinal(column));
		}

		public override Decimal GetDecimal(int i)
		{
			throw new NotImplementedException();
		}

		public double GetDouble(string column)
		{
			return GetDouble(GetOrdinal(column));
		}

		public override double GetDouble(int i)
		{
			throw new NotImplementedException();
		}

        public Type GetFieldType(string column)
        {
            return GetFieldType(GetOrdinal(column));
        }

		public override Type GetFieldType(int i)
		{
			throw new NotImplementedException();
		}

		public float GetFloat(string column)
		{
			return GetFloat(GetOrdinal(column));
		}

		public override float GetFloat(int i)
		{
			throw new NotImplementedException();
		}

		public Guid GetGuid(string column)
		{
			return GetGuid(GetOrdinal(column));
		}

		public override Guid GetGuid(int i)
		{
            throw new NotImplementedException();
		}

		public Int16 GetInt16(string column)
		{
			return GetInt16(GetOrdinal(column));
		}

		public override Int16 GetInt16(int i)
		{
			throw new NotImplementedException();
		}

        public Int32 GetInt32(string column)
		{
			return GetInt32(GetOrdinal(column));
		}

		public override Int32 GetInt32(int i)
		{
			throw new NotImplementedException();
		}

		public Int64 GetInt64(string column)
		{
			return GetInt64(GetOrdinal(column));
		}

		public override Int64 GetInt64(int i)
		{
			throw new NotImplementedException();
		}

		public override String GetName(int i)
		{
            throw new NotImplementedException();
		}

		public override int GetOrdinal(string name)
		{
			throw new NotImplementedException();
		}

		public override DataTable GetSchemaTable()
		{
			throw new NotImplementedException();
		}


		public string GetString(string column)
		{
			return GetString(GetOrdinal(column));
		}

		public override String GetString(int i)
		{
			throw new NotImplementedException();
		}


		public TimeSpan GetTimeSpan(string column)
		{
			return GetTimeSpan(GetOrdinal(column));
		}


		public TimeSpan GetTimeSpan(int column)
		{
			throw new NotImplementedException();
		}

		public override object GetValue(int i)
		{
			throw new NotImplementedException();
		}


		public override int GetValues(object[] values)
		{
			throw new NotImplementedException();
		}

		public UInt16 GetUInt16(string column)
		{
			return GetUInt16(GetOrdinal(column));
		}

		public UInt16 GetUInt16(int column)
		{
			throw new NotImplementedException();
		}

		public UInt32 GetUInt32(string column)
		{
			return GetUInt32(GetOrdinal(column));
		}

		public UInt32 GetUInt32(int column)
		{
			throw new NotImplementedException();
		}

		public UInt64 GetUInt64(string column)
		{
			return GetUInt64(GetOrdinal(column));
		}

		public UInt64 GetUInt64(int column)
		{
			throw new NotImplementedException();
		}


		#endregion

		IDataReader IDataRecord.GetData(int i)
		{
			return base.GetData(i);
		}


		public override bool IsDBNull(int i)
		{
			return DBNull.Value == GetValue(i);
		}


		public override bool NextResult()
		{
            ResultIndex++;
            if (ResultIndex >= ResultSet.Count)
            {
                m_currentResult = null;
                return false;
            }
            m_currentResult = ResultSet[ResultIndex];
            return true;
		}

		public override bool Read()
		{
            return CurrentResult.Read();
		}


		#region IEnumerator


		public override IEnumerator GetEnumerator()
		{
            throw new NotImplementedException();
        }

		#endregion
    }
}
