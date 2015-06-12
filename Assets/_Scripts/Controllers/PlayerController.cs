using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{


    public const int MAX_CAP = 20;
    public static int cap = 0;
    public GameObject movePin;
    public GameObject attackPin;

    System.Collections.Generic.List<GameObject> allSelectableUnits;
    System.Collections.Generic.List<GameObject> selectedUnits;
    GameObject selectedTarget;
    System.Collections.Generic.List<GameObject>[] groups;

    void Awake()
    {
        groups = new System.Collections.Generic.List<GameObject>[10];
        allSelectableUnits = new System.Collections.Generic.List<GameObject>();
        selectedUnits = new System.Collections.Generic.List<GameObject>();
        GameObject[] tmpArr = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject item in tmpArr)
        {
            allSelectableUnits.Add(item);
        }
    }

    void MouseFeedback()
    {
        RaycastHit hit;
        Ray mousetoWorldRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mousetoWorldRay, out hit, 2000))
        {
            //if (hit.collider.name == "Test_Ground")
            //{
            //    Debug.DrawRay(mousetoWorldRay.origin, mousetoWorldRay.direction * 1000);
            //    if (Input.GetMouseButtonDown(0))
            //    {
            //        //GameObject.Instantiate(attackPin, hit.point, Quaternion.identity);
            //    }
            //    if (Input.GetMouseButtonDown(1))
            //    {
            //      // GameObject.Instantiate(movePin, hit.point, Quaternion.identity);
            //    }

            //}
        }
    }

    public System.Collections.Generic.List<GameObject> GetAllSelectableObjects()
    {
        System.Collections.Generic.List<GameObject> hoopla = new System.Collections.Generic.List<GameObject>();
        return hoopla;
    }

    public void UnitSelection()
    {
    }

    public void UnitMove()
    {
        foreach (GameObject item in selectedUnits)
        {
            item.GetComponent<BaseCell>().Move(Input.mousePosition);
        }
    }

    public void UnitAttack()
    {
        foreach (GameObject item in selectedUnits)
        {
            item.GetComponent<BaseCell>().Attack(selectedTarget);
        }
    }

    public void UnitSplit()
    {
        foreach (GameObject item in selectedUnits)
        {

        }
    }

    public void UnitEvolve()
    {
    }

    public void UnitHarvest()
    {
        foreach (GameObject item in selectedUnits)
        {
            item.GetComponent<BaseCell>().Consume(selectedTarget);
        }
    }

    public void UnitIncubation()
    {
    }

    public void DoubleClick()
    {
        System.Type selectedType = selectedUnits[0].GetType();
        selectedUnits.Clear();
        foreach (GameObject item in allSelectableUnits)
        {
            if (item.GetComponent<BaseCell>().GetType() == selectedType)
            {
                selectedUnits.Add(item);
            }
        }
    }

    public void Grouping()
    {
    }

    public void DrawPin()
    {
    }

    public void OnGUI()
    {
    }

    public void FixedUpdate()
    {
    }

    public void PauseMenu()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // MouseFeedback();


    }
}
