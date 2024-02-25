using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private List<string> abilityNames = new List<string>();
    Animator animator;
    public TMP_Text achive;

    private void Start()
    {
        
        animator = GetComponent<Animator>();
        sceneCount = sceneCount + 1; //a static variable to store this scene is opened how much
        audioSourceBlup = GetComponent<AudioSource>();
        DisableAllGameObjects(); //disables all items when the scene is opened
        //sceneNames.Add("Level Green 3");
        sceneNames.Add("red deneme");
        sceneNames.Add("Level Blue 4");
        sceneNames.Add("End");
        abilityNames.Add("Now you will be go color - RED -");
        abilityNames.Add("Now you have Grappling! Try pressing - Left Mouse Button -(Please ensure that the cursor is on the right object)");
    }

    private void Update()
    {
        switch(sceneCount) //to change the item wrt. the level that is passed.
        {
            case 1:
                gameObjects[0].SetActive(true);
                Invoke("LoadScene", 6f);
                break;
            case 2:
                gameObjects[1].SetActive(true);
                Invoke("LoadScene", 6f);
                break;
            case 3:
                gameObjects[2].SetActive(true);
                Invoke("LoadScene", 6f);
                break;
            //case 4:
            //    gameObjects[3].SetActive(true);
            //    Invoke("LoadScene", 6f);
            //    break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        audioSourceBlup.Play();
        animator.SetTrigger("Cauldron");
        gameObjects[sceneCount - 1].SetActive(false);
        Invoke("WriteAbilityNames", 3f);
        achive.text = abilityNames[sceneCount - 1];
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
    private void WriteAbilityNames()
    {
        achive.text = abilityNames[sceneCount - 1];
    }
}
