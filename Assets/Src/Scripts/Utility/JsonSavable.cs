using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public class GenericSaver<T> where T : class
{
    private string filePath;

    public GenericSaver(string fileName)
    {
        filePath = Path.Combine(Application.dataPath, fileName);
    }

    // Method to save selected fields of the object
    public void SaveToFile(T obj)
    {
        try
        {
            // Get all the properties we want to save
            var selectedData = SerializeData(obj);

            // Serialize the data into a JSON string
            string json = JsonUtility.ToJson(new SerializationWrapper(selectedData), true);

            // Write the JSON string to a file
            File.WriteAllText(filePath, json);
            Debug.Log($"Data saved to {filePath}");
        } catch (Exception ex)
        {
            Debug.LogError($"Error saving data: {ex.Message}");
        }
    }

    // Method to load data from a file and return the deserialized object
    public T LoadFromFile()
    {
        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                SerializationWrapper wrapper = JsonUtility.FromJson<SerializationWrapper>(json);

                return DeserializeData(wrapper.data);
            } catch (Exception ex)
            {
                Debug.LogError($"Error loading data: {ex.Message}");
                return null;
            }
        } else
        {
            Debug.LogError("File does not exist.");
            return null;
        }
    }

    // This method will serialize only the fields you want to save
    private Dictionary<string, object> SerializeData(T obj)
    {
        Dictionary<string, object> data = new Dictionary<string, object>();
        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            // Check if the field should be saved (you can add your condition here)
            if (ShouldSaveField(field))
            {
                data[field.Name] = field.GetValue(obj);
            }
        }

        foreach (var prop in properties)
        {
            // Check if the property should be saved (you can add your condition here)
            if (ShouldSaveField(prop))
            {
                data[prop.Name] = prop.GetValue(obj);
            }
        }

        return data;
    }

    // This method checks whether to save a specific field/property (you can add any condition here)
    private bool ShouldSaveField(MemberInfo member)
    {
        // Example condition: you could filter out fields you don't want to save based on their name or type
        if (member.Name.Contains("value3")) // Don't save value3 field in Value_2
        {
            return false;
        }
        return true;
    }

    // This method will deserialize the data back into the object
    private T DeserializeData(Dictionary<string, object> data)
    {
        T obj = Activator.CreateInstance<T>();

        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var field in fields)
        {
            if (data.ContainsKey(field.Name))
            {
                field.SetValue(obj, Convert.ChangeType(data[field.Name], field.FieldType));
            }
        }

        foreach (var prop in properties)
        {
            if (data.ContainsKey(prop.Name))
            {
                prop.SetValue(obj, Convert.ChangeType(data[prop.Name], prop.PropertyType));
            }
        }

        return obj;
    }

    // A helper class to wrap dictionary for JSON serialization
    [System.Serializable]
    public class SerializationWrapper
    {
        public Dictionary<string, object> data;

        public SerializationWrapper(Dictionary<string, object> data)
        {
            this.data = data;
        }
    }
}
