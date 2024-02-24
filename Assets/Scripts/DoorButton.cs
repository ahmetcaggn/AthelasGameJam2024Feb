using UnityEngine;

public class DoorButton : MonoBehaviour
{
    private Animator m_Animator;
    private Door doorScript;
    public GameObject door;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
        doorScript = door.GetComponent<Door>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        doorScript.Open();
    }
}
