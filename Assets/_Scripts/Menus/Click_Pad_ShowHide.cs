using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Click_Pad_ShowHide : MonoBehaviour
{

    // Use this for initialization
    public Animator animator;
    public bool show;
   // public Toggle toggle;
    public Image button_image;
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

        if (show)
        {
            button_image.enabled = false;
        } else
            button_image.enabled = true;
  //     toggle.animationTriggers.highlightedTrigger
     //   animator.SetTrigger( toggle.animationTriggers.highlightedTrigger);

    }
}
