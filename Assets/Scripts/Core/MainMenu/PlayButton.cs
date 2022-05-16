using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void LoadSceneOnClick(string sceneName)
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        LevelManager.instance.LoadScene(sceneName);
    }
}
