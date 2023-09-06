using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SlimeBounce.Loading
{
    public class SceneLoader
    {

        public static void ReloadScene()
        {
            //Loading screen interaction could go here, prior to loading
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}