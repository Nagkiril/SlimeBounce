using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SlimeBounce.Save 
{
    [Serializable]
    public class SaveData
    {
        public string Name;
        public object Data;

        public SaveData(string newName, object newData)
        {
            Name = newName;
            SetData(newData);
        }

        public void SetData(object newData)
        {
            Data = newData;
        }
    }
}