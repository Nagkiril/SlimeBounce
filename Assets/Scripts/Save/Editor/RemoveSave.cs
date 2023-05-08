using UnityEditor;

namespace SlimeBounce.Save.Editor
{
    public class RemoveSave
    {
        [MenuItem("SlimeBounce/Clear Saves", priority = 90)]
        public static void Init()
        {
            SaveManager.ClearSaves();
        }
    }
}