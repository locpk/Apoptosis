using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{


    public const int MAX_CAP = 20;
    public static int cap = 0;
    public GameObject movePin;
    public GameObject attackPin;

    System.Collections.Generic.List<BaseCell> allSelectableUnits;
    System.Collections.Generic.List<BaseCell> selectedUnits;
    GameObject selectedTarget;
    System.Collections.Generic.List<BaseCell>[] groups;

    Rect selectionRect;

    void Awake()
    {
        selectedTarget = null;
        groups = new System.Collections.Generic.List<BaseCell>[10];
        allSelectableUnits = new System.Collections.Generic.List<BaseCell>();
        selectedUnits = new System.Collections.Generic.List<BaseCell>();
        GameObject[] tmpArr = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject item in tmpArr)
        {
            BaseCell bCell = item.GetComponent<BaseCell>();
            if (!bCell.isAIPossessed && bCell.isMine)
            {
                allSelectableUnits.Add(item.GetComponent<BaseCell>());
            }
        }
        selectionRect = new Rect();
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
        System.Collections.Generic.List<GameObject> allSelectableObjects = new System.Collections.Generic.List<GameObject>();
        foreach (BaseCell item in allSelectableUnits)
        {
            allSelectableObjects.Add(item.gameObject);
        }
        return allSelectableObjects;
    }

    public void UnitSelection()
    {
        selectionRect.size.Set(selectionRect.x - Input.mousePosition.x, selectionRect.y - Input.mousePosition.y);
    }

    public void UnitMove()
    {
        foreach (BaseCell item in selectedUnits)
        {
            item.Move(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    public void UnitAttack()
    {
        foreach (BaseCell item in selectedUnits)
        {
            item.Attack(selectedTarget);
        }
    }

    public void UnitSplit()
    {
        foreach (BaseCell item in selectedUnits)
        {
            switch (item.celltype)
            {
                case CellType.STEM_CELL:
                    item.PerfectSplit();
                    break;
                case CellType.HEAT_CELL:
                case CellType.COLD_CELL:
                    item.CancerousSplit();
                    break;

                default:
                    break;
            }
        }
    }

    public void UnitEvolve()
    {
    }

    public void UnitHarvest()
    {
        foreach (BaseCell item in selectedUnits)
        {
            item.Consume(selectedTarget);
        }
    }

    public void UnitIncubation()
    {
    }

    public void DoubleClick()
    {
        BaseCell selectedCell = selectedUnits[0].GetComponent<BaseCell>();
        CellType selectedType = selectedCell.celltype;
        selectedUnits.Clear();
        foreach (BaseCell item in allSelectableUnits)
        {
            if (item.celltype == selectedType)
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

    public void UnitStop()
    {
        foreach (BaseCell item in selectedUnits)
        {
            //item.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("D"))
        {
            UnitSplit();
        }

        if (Input.GetKeyDown("S"))
        {
            UnitStop();
        }

        if (Input.GetKeyDown("C"))
        {
            foreach (StemCell item in selectedUnits)
            {
                //item.Mutate(CellType.ALKALI_CELL)
            }
        }

        if (Input.GetKeyDown("X"))
        {

        }

        if (Input.GetKeyDown("V"))
        {

        }

        if (Input.GetKeyDown("Z"))
        {

        }

        if (Input.GetMouseButtonDown(1))
        {
            selectionRect.position.Set(Input.mousePosition.x, Input.mousePosition.y);
        }
        else if (Input.GetMouseButtonUp(1))
        {

        }
        else if (Input.GetMouseButton(1))
        {
            UnitSelection();
        }
    }
}
