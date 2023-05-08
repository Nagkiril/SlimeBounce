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
        static string SAVE_FILE_NAME = "savefile.json";
        static List<SaveData> _saves;

        static public event Action OnSavesCleared;


        static private void ReadSave()
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

        static private void WriteSave()
        {
            var toSave = new List<SaveData>();
            foreach (var save in _saves)
            {
                toSave.Add(new SaveData(save.Name, JsonConvert.SerializeObject(save.Data)));
            }
            var jsonString = JsonConvert.SerializeObject(toSave);
            File.WriteAllText(GetSavePath(), jsonString);
        }

        static public void ClearSaves()
        {
            if (File.Exists(GetSavePath()))
            {
                File.Delete(GetSavePath());
                _saves = null;
            }
            OnSavesCleared?.Invoke();
        }

        static public T Load<T>(string entryName)
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


        static public void Save<T>(string entryName, T entryData)
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

        static private SaveData FindEntry(string entryName)
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

        static private string GetSavePath() => $"{Application.persistentDataPath}/{SAVE_FILE_NAME}";
    }
}