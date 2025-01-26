using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace CustomArchitecture
{
    public enum FileType
    {
        ConfigFile,
        SaveFile,
    }

    public class SaveUtilitary<T>
    {
        private readonly string                 m_configPath = "Assets/Src/Resources/ConfigFile/";
        private string                          m_paths = "default_path";
        private bool                            m_backup = true;
        private string                          m_backupExtension = ".backup";
        private DataFormat                      m_dataFormat = DataFormat.JSON;
        private Dictionary<DataFormat, string>  m_extensions = new()
        {
            { DataFormat.JSON, ".json" },
            { DataFormat.Binary, ".bin" },
            { DataFormat.Nodes, ".nodes" }, // better not using this one
        };

        public DataFormat Format
        {
            get { return m_dataFormat; }
            set { m_dataFormat = value; }
        }

        public SaveUtilitary(string file_path, FileType type, bool backup = true)
        {
            m_backup = backup;

            if (type == FileType.ConfigFile)
            {
                m_paths = Path.Combine(Application.streamingAssetsPath, file_path);
            }
            else if (type == FileType.SaveFile)
            {
                m_paths = Path.Combine(Application.persistentDataPath, file_path);
            }
        }

        private string GetPath(bool backup)
        {
            if (backup)
                return new string(m_paths + m_extensions[m_dataFormat] + m_backupExtension);
            else
                return new string(m_paths + m_extensions[m_dataFormat]);
        }

        public void Save(T save_object)
        {
            byte[] data = SerializationUtility.SerializeValue(save_object, m_dataFormat);

            string json = Encoding.UTF8.GetString(data);

            File.WriteAllText(GetPath(false), json);

            if (m_backup)
                File.WriteAllText(GetPath(true), json);
        }

        public T Load()
        {
            if (!File.Exists(GetPath(false)))
            {
                Debug.LogWarning($"{GetPath(false)}: file not found");
                return default(T);
            }

            string json = File.ReadAllText(GetPath(false));
            byte[] jsonBytes = Encoding.UTF8.GetBytes(json);

            T loadedObject = SerializationUtility.DeserializeValue<T>(jsonBytes, m_dataFormat);
            return loadedObject;
        }
    }
}