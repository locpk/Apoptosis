using UnityEngine;
using System.Collections;

public class Click_Pad_ShowHide : MonoBehaviour
{

    // Use this for initialization
    public Animator animator;
    public bool show;
    void Start()
    {
        if (!Input.touchSupported)
        {
            Show_Hide_Panel();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Show_Hide_Panel()
    {
        show = !show;
        animator.SetBool("SHOW", show);



    }
}
