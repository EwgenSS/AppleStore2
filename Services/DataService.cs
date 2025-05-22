using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AppleStore.Services
{
    public class DataService
    {
        private readonly string _dataDirectory;

        public DataService()
        {
            _dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(_dataDirectory))
            {
                Directory.CreateDirectory(_dataDirectory);
            }
        }

        public void SaveData<T>(string fileName, List<T> data)
        {
            string filePath = Path.Combine(_dataDirectory, fileName);
            string jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(filePath, jsonData);
        }

        public List<T> LoadData<T>(string fileName)
        {
            string filePath = Path.Combine(_dataDirectory, fileName);
            if (!File.Exists(filePath))
            {
                return new List<T>();
            }

            string jsonData = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<List<T>>(jsonData) ?? new List<T>();
        }
    }
}
