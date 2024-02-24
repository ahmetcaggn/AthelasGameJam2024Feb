using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Cinemachine.DocumentationSortingAttribute;

public class CauldronController : MonoBehaviour
{
    AudioSource audioSourceBlup;
    public List<GameObject> gameObjects = new List<GameObject>(); //A list which stores items
    private static int sceneCount;
    private List<string> sceneNames = new List<string>();

    private void Start()
    {
        sceneCount = sceneCount + 1; //a static variable to store this scene is opened how much
        audioSourceBlup = GetComponent<AudioSource>();
        DisableAllGameObjects(); //disables all items when the scene is opened
        sceneNames.Add("Level Red 2");
        sceneNames.Add("Level Green 3");
        sceneNames.Add("Level Blue 4");
        sceneNames.Add("End");
    }

    private void Update()
    {
        switch(sceneCount) //to change the item wrt. the level that is passed.
        {
            case 1:
                gameObjects[0].SetActive(true);
                Invoke("LoadScene", 2f);
                break;
            case 2:
                gameObjects[1].SetActive(true);
                Invoke("LoadScene", 2f);
                break;
            case 3:
                gameObjects[2].SetActive(true);
                Invoke("LoadScene", 2f);
                break;
            case 4:
                gameObjects[3].SetActive(true);
                Invoke("LoadScene", 2f);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioSourceBlup.Play();
    }

    private void DisableAllGameObjects()
    {
        foreach (GameObject obj in gameObjects)
        {
            obj.gameObject.SetActive(false);
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneNames[sceneCount-1]);
    }
}
