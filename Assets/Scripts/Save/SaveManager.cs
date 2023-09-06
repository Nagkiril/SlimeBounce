using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

namespace SlimeBounce.Save
{
    public static class SaveManager
    {
        private static string SAVE_FILE_NAME = "savefile.json";
        private static List<SaveData> _saves;

        public static event Action OnSavesCleared;


        private static void ReadSave()
        {
            if (_saves == null)
            {
                if (File.Exists(GetSavePath()))
                {
                    var saveText = File.ReadAllText(GetSavePath());
                    _saves = JsonConvert.DeserializeObject<List<SaveData>>(saveText);
                } else
                {
                    _saves = new List<SaveData>();
                }
            }
        }

        private static void WriteSave()
        {
            var toSave = new List<SaveData>();
            foreach (var save in _saves)
            {
                toSave.Add(new SaveData(save.Name, JsonConvert.SerializeObject(save.Data)));
            }
            var jsonString = JsonConvert.SerializeObject(toSave);
            File.WriteAllText(GetSavePath(), jsonString);
        }

        public static void ClearSaves()
        {
            if (File.Exists(GetSavePath()))
            {
                File.Delete(GetSavePath());
                _saves = null;
            }
            OnSavesCleared?.Invoke();
        }

        public static T Load<T>(string entryName)
        {
            ReadSave();
            SaveData appliedEntry = FindEntry(entryName);
            if (appliedEntry == null)
            {
                return default(T);
            }
            if (appliedEntry.Data is string)
            {
                var readData = JsonConvert.DeserializeObject<T>(appliedEntry.Data.ToString());
                appliedEntry.SetData(readData);
            }
            return (T)appliedEntry.Data;
        }


        public static void Save<T>(string entryName, T entryData)
        {
            SaveData appliedEntry = FindEntry(entryName);
            if (appliedEntry == null)
            {
                appliedEntry = new SaveData(entryName, entryData);
                _saves.Add(appliedEntry);
            } else
            {
                appliedEntry.SetData(entryData);
            }
            WriteSave();
        }

        private static SaveData FindEntry(string entryName)
        {
            for (var i = 0; i < _saves.Count; i++)
            {
                if (_saves[i].Name == entryName)
                {
                    return _saves[i];
                }
            }
            return null;
        }

        private static string GetSavePath() => $"{Application.persistentDataPath}/{SAVE_FILE_NAME}";
    }
}