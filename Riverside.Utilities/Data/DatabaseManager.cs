using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Riverside.Utilities.Data
{
    public class DatabaseManager : IDatabaseManager
    {
        // Member variables
        private bool _disposed;
        private SqlCommand _cmd;
        private SqlConnection _conn;
        private SqlDataAdapter _da;
        private SqlDataReader _dr;
        private SqlTransaction _tran;

        public DatabaseManager(string connectionString, bool transaction)
        {
            _conn = new SqlConnection(connectionString);
            _conn.Open();
            if (transaction)
                _tran = _conn.BeginTransaction();
        }

        private void TidyCommand()
        {
            if (_cmd != null)
            {
                _cmd.Dispose();
                _cmd = null;
            }
        }

        public void SetStoredProcedure(string storedProcedure)
        {
            TidyDataReader();
            TidyCommand();

            _cmd = new SqlCommand(storedProcedure, _conn);
            _cmd.CommandType = CommandType.StoredProcedure;
            if (_tran != null)
                _cmd.Transaction = _tran;
        }

        public void SetCommandTimeout(int seconds)
        {
            _cmd.CommandTimeout = seconds;
        }

        public void SetSQL(string sql)
        {
            TidyDataReader();
            TidyCommand();

            _cmd = new SqlCommand(sql, _conn);
            _cmd.CommandType = CommandType.Text;
            if (_tran != null)
                _cmd.Transaction = _tran;
        }

        private SqlDbType GetSqlDbType(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.BigInt:
                    return SqlDbType.BigInt;
                case FieldType.Binary:
                    return SqlDbType.Binary;
                case FieldType.Bit:
                    return SqlDbType.Bit;
                case FieldType.Char:
                    return SqlDbType.Char;
                case FieldType.DateTime:
                    return SqlDbType.DateTime;
                case FieldType.Decimal:
                    return SqlDbType.Decimal;
                case FieldType.Float:
                    return SqlDbType.Float;
                case FieldType.Image:
                    return SqlDbType.Image;
                case FieldType.Int:
                    return SqlDbType.Int;
                case FieldType.Money:
                    return SqlDbType.Money;
                case FieldType.NChar:
                    return SqlDbType.NChar;
                case FieldType.NText:
                    return SqlDbType.NText;
                case FieldType.NVarChar:
                    return SqlDbType.NVarChar;
                case FieldType.Real:
                    return SqlDbType.Real;
                case FieldType.SmallDateTime:
                    return SqlDbType.SmallDateTime;
                case FieldType.SmallInt:
                    return SqlDbType.SmallInt;
                case FieldType.SmallMoney:
                    return SqlDbType.SmallMoney;
                case FieldType.Structured:
                    return SqlDbType.Structured;
                case FieldType.Text:
                    return SqlDbType.Text;
                case FieldType.Timestamp:
                    return SqlDbType.Timestamp;
                case FieldType.TinyInt:
                    return SqlDbType.TinyInt;
                case FieldType.UniqueIdentifier:
                    return SqlDbType.UniqueIdentifier;
                case FieldType.VarBinary:
                    return SqlDbType.VarBinary;
                case FieldType.VarChar:
                    return SqlDbType.VarChar;
                case FieldType.Variant:
                    return SqlDbType.Variant;
                case FieldType.Xml:
                    return SqlDbType.Xml;
            }

            throw new Exception("Invalid SqlDbType.");
        }

        public object DataReaderValue(string name)
        {
            return _dr[name];
        }

        public object DataReaderValue(int index)
        {
            return _dr[index];
        }

        public int DataReaderFieldCount()
        {
            return _dr.FieldCount;
        }

        public string DataReaderFieldName(int index)
        {
            return _dr.GetName(index);
        }

        public void AddParameter(string parameterName, FieldType fieldType, object value)
        {
            _cmd.Parameters.Add(parameterName, GetSqlDbType(fieldType)).Value = value;
        }

        public void AddTypedParameter(string parameterName, FieldType fieldType, object value, string typeName)
        {
            SqlParameter sqlParameter = _cmd.Parameters.Add(parameterName, GetSqlDbType(fieldType));
            sqlParameter.Value = value;
            sqlParameter.TypeName = typeName;
        }

        public void AddParameter(string parameterName, FieldType fieldType, int size, object value)
        {
            _cmd.Parameters.Add(parameterName, GetSqlDbType(fieldType), size).Value = value;
        }

        public void AddOutputParameter(string parameterName, FieldType fieldType)
        {
            SqlParameter param = _cmd.Parameters.Add(parameterName, GetSqlDbType(fieldType));
            param.Direction = ParameterDirection.Output;
        }

        public void AddOutputParameter(string parameterName, FieldType fieldType, int size)
        {
            SqlParameter param = _cmd.Parameters.Add(parameterName, GetSqlDbType(fieldType), size);
            param.Direction = ParameterDirection.Output;
        }

        public void AddReturnParameter(string parameterName, FieldType fieldType)
        {
            SqlParameter param = _cmd.Parameters.Add(parameterName, GetSqlDbType(fieldType));
            param.Direction = ParameterDirection.ReturnValue;
        }

        public void AddReturnParameter(string parameterName, FieldType fieldType, int size)
        {
            SqlParameter param = _cmd.Parameters.Add(parameterName, GetSqlDbType(fieldType), size);
            param.Direction = ParameterDirection.ReturnValue;
        }

        public void AddOutputParameter(string parameterName, FieldType fieldType, byte precision, byte scale)
        {
            SqlParameter param = _cmd.Parameters.Add(parameterName, GetSqlDbType(fieldType), precision);
            param.Precision = precision;
            param.Scale = scale;
            param.Direction = ParameterDirection.Output;
        }

        private void TidyDataReader()
        {
            if (_dr != null && !_dr.IsClosed)
                _dr.Close();
            _dr = null;
        }

        private void TidyDataAdapter()
        {
            if (_da != null)
                _da.Dispose();
            _da = null;
        }

        public void ExecuteReader()
        {
            TidyDataReader();

            _dr = _cmd.ExecuteReader();
        }

        public void Fill(DataSet dataSet)
        {
            TidyDataAdapter();

            _da = new SqlDataAdapter(_cmd);
            _da.Fill(dataSet);
        }

        public bool Read()
        {
            bool read = _dr.Read();
            if (!read && !_dr.NextResult())
            {
                TidyDataReader();
                TidyCommand();
            }
            return read;
        }

        public int ExecuteNonQuery()
        {
            int rows = _cmd.ExecuteNonQuery();

            TidyCommand();

            return rows;
        }

        public int ExecuteNonQuery(Dictionary<string, object> outputValues)
        {
            int rows = _cmd.ExecuteNonQuery();

            for (int param = 0; param < _cmd.Parameters.Count; param++)
            {
                if (_cmd.Parameters[param].Direction == ParameterDirection.Output || _cmd.Parameters[param].Direction == ParameterDirection.ReturnValue)
                {
                    string outputParameterName = _cmd.Parameters[param].ParameterName;
                    outputValues[outputParameterName] = _cmd.Parameters[param].Value;
                }
            }

            TidyCommand();

            return rows;
        }

        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called
            if (!_disposed)
            {
                // If disposing equals true, dispose all managed and unmanaged resources
                if (disposing)
                {
                    // Dispose managed resources
                    TidyDataReader();
                    TidyDataAdapter();
                    TidyCommand();
                    if (_tran != null)
                        _tran.Dispose();
                    if (_conn != null)
                        _conn.Dispose();
                }

                // Clean up unmanaged resources here (there are none)
            }
            _disposed = true;
        }

        public void Commit()
        {
            _tran.Commit();
        }

        public void Rollback()
        {
            _tran.Rollback();
        }

        ~DatabaseManager()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            // Clean up this object
            Dispose(true);

            // Take this object off the finalization queue and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }
    }
}
