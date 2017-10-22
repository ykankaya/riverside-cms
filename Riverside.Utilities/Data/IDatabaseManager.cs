using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Riverside.Utilities.Data
{
    public interface IDatabaseManager : IDisposable, ITransactional
    {
        bool Read();
        int DataReaderFieldCount();
        int ExecuteNonQuery();
        int ExecuteNonQuery(Dictionary<string, object> outputValues);
        string DataReaderFieldName(int index);
        object DataReaderValue(int index);
        object DataReaderValue(string name);
        void AddOutputParameter(string parameterName, FieldType fieldType);
        void AddOutputParameter(string parameterName, FieldType fieldType, byte precision, byte scale);
        void AddOutputParameter(string parameterName, FieldType fieldType, int size);
        void AddParameter(string parameterName, FieldType fieldType, int size, object value);
        void AddParameter(string parameterName, FieldType fieldType, object value);
        void AddReturnParameter(string parameterName, FieldType fieldType);
        void AddReturnParameter(string parameterName, FieldType fieldType, int size);
        void AddTypedParameter(string parameterName, FieldType fieldType, object value, string typeName);
        void ExecuteReader();
        void Fill(DataSet dataSet);
        void SetCommandTimeout(int seconds);
        void SetSQL(string sql);
        void SetStoredProcedure(string storedProcedure);
    }
}
