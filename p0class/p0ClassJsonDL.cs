using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace p0class
{
    public class JsonDatastore<T> : Datastore<T> where T : class
    {
        private string _filename = "./datafiles/datastore.json";
        private List<T> _records;

        public bool AddRecord(T p_rec)
        {
            _records.Add(p_rec);
            string jsonString = JsonSerializer.Serialize<List<T>>(_records);
            try
            {
                File.WriteAllText(_filename, jsonString);
            }
            catch(System.Exception)
            {
                throw new Exception("File path is invalid");
            }
            return true;
        }

        public List<T> LoadAllRecords()
        {
            return new List<T>(_records);
        }

        public T LoadRecord(T _p_rec)
        {
            throw new System.NotImplementedException();
        }

        public JsonDatastore()
        {
            string jsonString;
            try
            {
                jsonString = File.ReadAllText(_filename);
            }
            catch(System.Exception)
            {
                throw new Exception("File path is invalid");
            }

            _records = JsonSerializer.Deserialize<List<T>>(jsonString);
        }
    }
}