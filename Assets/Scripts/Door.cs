using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }

    [ContextMenu("open")]
    public void Open()
    {
        m_Animator.SetTrigger("Open");
    }
}
