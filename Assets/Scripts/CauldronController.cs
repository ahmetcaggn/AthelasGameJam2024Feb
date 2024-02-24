using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CauldronController : MonoBehaviour
{
    AudioSource audioSourceBlup;
    public List<GameObject> gameObjects = new List<GameObject>(); //A list which stores items
    private static int sceneCount;

    private void Start()
    {
        sceneCount = sceneCount + 1; //a static variable to store this scene is opened how much
        audioSourceBlup = GetComponent<AudioSource>();
        DisableAllGameObjects(); //disables all items when the scene is opened
    }

    private void Update()
    {
        switch(sceneCount) //to change the item wrt. the level that is passed.
        {
            case 1:
                gameObjects[0].SetActive(true);
                break;
            case 2:
                gameObjects[1].SetActive(true);
                break;
            case 3:
                gameObjects[2].SetActive(true);
                break;
            case 4:
                gameObjects[3].SetActive(true);
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
}
