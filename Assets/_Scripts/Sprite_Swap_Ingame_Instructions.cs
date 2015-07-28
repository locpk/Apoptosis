using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Sprite_Swap_Ingame_Instructions : MonoBehaviour
{
    public GameObject canvas;
    public Sprite Mouse_Controls_Sprite;
    public Sprite Tablet_Controls_Sprite;


    // Use this for initialization
    void Start()
    {
        if (Input.touchSupported)
        {
            canvas.GetComponent<Image>().sprite = Tablet_Controls_Sprite;
        }
        else
        {
            canvas.GetComponent<Image>().sprite = Mouse_Controls_Sprite;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
