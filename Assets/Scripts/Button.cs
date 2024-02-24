using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Button : MonoBehaviour
{
    public void changeSceneToCutscene()
    {
        SceneManager.LoadScene("Cutscene");
    }
    public void changeSceneToGreen()
    {
        SceneManager.LoadScene("Level Green 3");
    }
}
