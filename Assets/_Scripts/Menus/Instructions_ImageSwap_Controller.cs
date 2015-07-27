using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Instructions_ImageSwap_Controller : MonoBehaviour
{


    public Sprite Controls_Sprite;
    public Sprite EvolutionTree_Sprite;
    public Sprite Gameplay_Sprite;
    public Sprite Tablet_Controls;

    public int starting_page;
    public GameObject canvas;


    // Use this for initialization
    void Start()
    {
        starting_page = 0;
    }

    // Update is called once per frame
    void Update()
    {







    }

    public void MainMenu()
    {
        Application.LoadLevel("MainMenu");
    }

    public void PreviousPage()
    {

        if (canvas.GetComponent<Image>().sprite == Gameplay_Sprite)
        {
            Application.LoadLevel("MainMenu");
        }
        else if (canvas.GetComponent<Image>().sprite == EvolutionTree_Sprite)
        {
             canvas.GetComponent<Image>().sprite = Gameplay_Sprite;
        }

        else if (canvas.GetComponent<Image>().sprite == Tablet_Controls)
        {

            canvas.GetComponent<Image>().sprite = Controls_Sprite;
        }
        else if (canvas.GetComponent<Image>().sprite == Controls_Sprite)
        {
 
            canvas.GetComponent<Image>().sprite = EvolutionTree_Sprite;
        }
        //starting_page--;

        //switch(starting_page)
        //{
        //    case 0:
        //        break;
        //    case 1:
        //        //starting_page = 0;
        //        Application.LoadLevel("MainMenu");
        //        break;
        //    case 2:
        //        canvas.GetComponent<Image>().sprite = Gameplay_Sprite;
        //        break;
        //    case 3:
        //        canvas.GetComponent<Image>().sprite = EvolutionTree_Sprite;
        //        break;
        //    default:
        //        break;
        //}
    }

    public void NextPage()
    {

        if (canvas.GetComponent<Image>().sprite == Gameplay_Sprite)
        {
            canvas.GetComponent<Image>().sprite = EvolutionTree_Sprite;
        }
        else if (canvas.GetComponent<Image>().sprite == EvolutionTree_Sprite)
        {
            canvas.GetComponent<Image>().sprite = Controls_Sprite;
        }
        else if (canvas.GetComponent<Image>().sprite == Controls_Sprite)
        {
            canvas.GetComponent<Image>().sprite = Tablet_Controls;
        }
        else if (canvas.GetComponent<Image>().sprite == Tablet_Controls)
        {
            Application.LoadLevel("Tutorial_Level");
        }

        //starting_page++;

        //switch (starting_page)
        //{
    
        //    case 0:
        //        // case empty for default strite               

        //        break;

        //    case 1:
        //        canvas.GetComponent<Image>().sprite = EvolutionTree_Sprite;
        //        break;

        //    case 2:
        //        canvas.GetComponent<Image>().sprite = Controls_Sprite;
        //        break;
        //    case 3:
        //        Application.LoadLevel("Tutorial_Level");
        //        break;
        //    default:
        //        starting_page = 0;
        //        Application.LoadLevel("MainMenu");
        //        break;
        //}
    }
}