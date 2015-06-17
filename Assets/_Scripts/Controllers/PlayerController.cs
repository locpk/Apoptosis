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
    public Texture selector;

    Rect GUISelectRect;

    void Awake()
    {
        // Initialize variables
        selectedTarget = null;
        groups = new System.Collections.Generic.List<BaseCell>[10];
        allSelectableUnits = new System.Collections.Generic.List<BaseCell>();
        selectedUnits = new System.Collections.Generic.List<BaseCell>();
        // ----------

        GameObject[] tmpArr = GameObject.FindGameObjectsWithTag("Unit"); // Get every cell in the game
        foreach (GameObject item in tmpArr) // Iterate through all the cells
        {
            BaseCell bCell = item.GetComponent<BaseCell>(); // Upcast each cell to a base cell
            if (!bCell.isAIPossessed && bCell.isMine) // If the cell belongs to this player
            {
                allSelectableUnits.Add(item.GetComponent<BaseCell>()); // Add the cell to the players controllable units
            }
        }
    }

    public System.Collections.Generic.List<GameObject> GetAllSelectableObjects()
    {
        System.Collections.Generic.List<GameObject> allSelectableObjects = new System.Collections.Generic.List<GameObject>(); // Initialize a list of GameObjects
        foreach (BaseCell item in allSelectableUnits) // For each of the player's controllable cells
        {
            allSelectableObjects.Add(item.gameObject); // Add the cell's GameObject to the list
        }
        return allSelectableObjects; // Return the list
    }

    public void UnitSelection()
    {
        GUISelectRect.xMax = Input.mousePosition.x;
        GUISelectRect.yMax = -Input.mousePosition.y + Screen.height;
        selectedUnits.Clear();
        foreach (BaseCell item in allSelectableUnits)
        {
            if (GUISelectRect.Contains(Camera.main.WorldToScreenPoint(item.transform.position)))
            {
                selectedUnits.Add(item);
            }
        }
    }

    public void UnitMove()
    {
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            double w = .1 * System.Math.Sqrt(i);
            double t = 2 * System.Math.PI * i;
            double x = w * System.Math.Cos(t);
            double y = w * System.Math.Sin(t);
            Vector3 newPos = Input.mousePosition;
            newPos.Set((float)(newPos.x + x), (float)(newPos.y + y), newPos.z);
            selectedUnits[i].Move(Camera.main.ScreenToWorldPoint(Input.mousePosition)); // Set their destination
        }

    }

    public void UnitAttack()
    {
        foreach (BaseCell item in selectedUnits) // For each of the player's selected units
        {
            item.Attack(selectedTarget); // Set the target cell to attack
        }
    }

    public void UnitSplit()
    {
        foreach (BaseCell item in selectedUnits) // For each of the player's selected units
        {
            switch (item.celltype) // Dependent on the type of cell it is
            {
                case CellType.STEM_CELL: // If it is a stem cell
                    item.PerfectSplit(); // Split without a chance of cancer
                    break;
                case CellType.HEAT_CELL: // If it is a heat cell
                case CellType.COLD_CELL: // OR If it is a cold cell
                    item.CancerousSplit(); // Split with a chance of cancer
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
        foreach (BaseCell item in selectedUnits) // For each of the player's selected units
        {
            item.Consume(selectedTarget); // Set the target protein to consume
        }
    }

    public void UnitIncubation()
    {
    }

    public void DoubleClick()
    {
        BaseCell selectedCell = selectedUnits[0].GetComponent<BaseCell>(); // Get double-clicked cell;
        CellType selectedType = selectedCell.celltype; // Get the type of cell it is
        selectedUnits.Clear(); // Remove control of any currently selected units
        foreach (BaseCell item in allSelectableUnits) // For each of the player's selectable units
        {
            if (item.celltype == selectedType) // If the type matches the double-clicked cell
            {
                selectedUnits.Add(item); // Add the cell to the players selected units
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
        if (GUISelectRect.height != 0)
        {
            GUI.DrawTexture(GUISelectRect, selector);
        }
    }

    public void FixedUpdate()
    {
    }

    //Made a pause menu script instead
    //public void PauseMenu()
    //{
    //}

    public void UnitStop()
    {
        foreach (BaseCell item in selectedUnits) // For each of the player's selected units
        {
            //item.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 topleft = new Vector3(GUISelectRect.xMin, GUISelectRect.yMin, Camera.main.transform.position.z);
        Vector3 bottomright = new Vector3(GUISelectRect.xMax, GUISelectRect.yMin, Camera.main.transform.position.z);
        if (Input.GetKeyDown(KeyCode.D)) // If the player presses D
        {
            UnitSplit();
        }

        if (Input.GetKeyDown(KeyCode.S)) // If the player presses S
        {
            UnitStop();
        }

        if (Input.GetKeyDown(KeyCode.C)) // If the player presses C
        {
            foreach (StemCell item in selectedUnits) // For each of the player's selected units
            {
                //item.Mutation(CellType.ACIDIC_CELL)
            }
        }

        if (Input.GetKeyDown(KeyCode.X)) // If the player presses X
        {
            foreach (StemCell item in selectedUnits) // For each of the player's selected units
            {
                //item.Mutation(CellType.HEAT_CELL)
            }
        }

        if (Input.GetKeyDown(KeyCode.V)) // If the player presses V
        {
            foreach (StemCell item in selectedUnits)
            {
                //item.Mutation(CellType.ALKALI_CELL)
            }
        }

        if (Input.GetKeyDown(KeyCode.Z)) // If the player presses Z
        {
            foreach (StemCell item in selectedUnits)
            {
                //item.Mutation(CellType.COLD_CELL)
            }
        }

        if (Input.GetMouseButtonDown(0)) // If the player left-clicks
        {
            GUISelectRect.xMin = Input.mousePosition.x;
            GUISelectRect.yMin = -Input.mousePosition.y + Screen.height;
        }
        else if (Input.GetMouseButtonUp(0)) // When the player releases left-click
        {
            GUISelectRect.yMax = GUISelectRect.yMin;
            GUISelectRect.xMax = GUISelectRect.xMin;
        }
        else if (Input.GetMouseButton(0)) // If the player has left-click held down
        {
            UnitSelection();
        }
    }
}
