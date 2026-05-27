using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using SmartTaskManager.Models;

namespace SmartTaskManager.Services
{
    public class TaskStorageService
    {
        private string _filePath;

        public TaskStorageService()
        {
            string folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "SmartTaskManager");

            Directory.CreateDirectory(folder);
            _filePath = Path.Combine(folder, "tasks.json");
        }

        public List<TaskItem> LoadTasks()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return new List<TaskItem>();

                string json = File.ReadAllText(_filePath);
                var options = new JsonSerializerOptions();
                options.Converters.Add(new JsonStringEnumConverter());

                return JsonSerializer.Deserialize<List<TaskItem>>(json, options)
                       ?? new List<TaskItem>();
            }
            catch
            {
                return new List<TaskItem>();
            }
        }

        public void SaveTasks(List<TaskItem> tasks)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                options.Converters.Add(new JsonStringEnumConverter());

                string json = JsonSerializer.Serialize(tasks, options);
                File.WriteAllText(_filePath, json);
            }
            catch
            {
            }
        }
    }
}
