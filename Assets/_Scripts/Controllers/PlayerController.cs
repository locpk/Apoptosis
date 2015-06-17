using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    private int terrainLayer;

    public const int MAX_CAP = 20;
    public static int cap = 0;
    public GameObject movePin;
    public GameObject attackPin;
	

    public List<BaseCell> allSelectableUnits;
    public List<BaseCell> selectedUnits;
    GameObject selectedTarget;
    List<BaseCell>[] groups;
    public Texture selector;

    Rect GUISelectRect;

    void Awake()
    {
        // Initialize variables
        selectedTarget = null;
        groups = new List<BaseCell>[10];
        allSelectableUnits = new List<BaseCell>();
        selectedUnits = new List<BaseCell>();
        terrainLayer = 1 << LayerMask.NameToLayer ("Terrain");  // Layer masking for raycast clicking
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

	public void AddNewCell(BaseCell _in){
		allSelectableUnits.Add (_in);
		selectedUnits.Add (_in);
	}

    public List<GameObject> GetAllSelectableObjects()
    {
        List<GameObject> allSelectableObjects = new List<GameObject>(); // Initialize a list of GameObjects
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
            Vector3 itemPos = Camera.main.WorldToScreenPoint(item.transform.position);
            itemPos.y = -itemPos.y + Screen.height;
            if (GUISelectRect.Contains(itemPos))
            {
                selectedUnits.Add(item);
                item.isSelected = true;
            }
        }
    }

    public void UnitMove()
    {
        // Modified by using raycast
        RaycastHit hitInfo;
        Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(screenRay, out hitInfo, 1000.0f, terrainLayer)) {
            foreach (BaseCell item in selectedUnits)
            {
                //item.Move(Camera.main.ScreenToWorldPoint(Input.mousePosition)); // Set their destination
                item.Move(hitInfo.point); // Set their destination
            }
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
        int i = 0;
        for (int count = selectedUnits.Count; i < count; ++i) // For each of the player's selected units
        {
            switch (selectedUnits[i].celltype) // Dependent on the type of cell it is
            {
                case CellType.STEM_CELL: // If it is a stem cell
                    selectedUnits[i].PerfectSplit();
                    break;
                case CellType.HEAT_CELL: // If it is a heat cell
                case CellType.COLD_CELL: // OR If it is a cold cell
                    selectedUnits[i].CancerousSplit();
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
                item.isSelected = true;
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
        if (GUISelectRect.height != 0 && GUISelectRect.width != 0)
        {
            GUI.DrawTexture(GUISelectRect, selector);
        }
        foreach (BaseCell item in selectedUnits)
        {
            if (item) {
                Vector3 drawLoc = Camera.main.WorldToScreenPoint(item.transform.position);
                float left = drawLoc.x - (float)4;
                float top = -(drawLoc.y - (float)4) + Screen.height;
                Rect location = new Rect(left, top, (float)8, (float)8);
                GUI.DrawTexture(location, selector);
            }
        }
    }


    public void FixedUpdate()
    {
    }

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
        int i = 0;
        while (i < selectedUnits.Count)
        {
            if (selectedUnits[i] == null)
            {
                selectedUnits.RemoveAt(i);
            }
            else
                i++;
        }
        i = 0;
        while (i < allSelectableUnits.Count)
        {
            if (allSelectableUnits[i] == null)
            {
                allSelectableUnits.RemoveAt(i);
            }
            else
                i++;
        }
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
                item.Mutation(CellType.ACIDIC_CELL);
            }
        }

        if (Input.GetKeyDown(KeyCode.X)) // If the player presses X
        {
            foreach (StemCell item in selectedUnits) // For each of the player's selected units
            {
                item.Mutation(CellType.HEAT_CELL);
            }
        }

        if (Input.GetKeyDown(KeyCode.V)) // If the player presses V
        {
            foreach (StemCell item in selectedUnits)
            {
                item.Mutation(CellType.ALKALI_CELL);
            }
        }

        if (Input.GetKeyDown(KeyCode.Z)) // If the player presses Z
        {
            foreach (StemCell item in selectedUnits)
            {
                item.Mutation(CellType.COLD_CELL);
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
        if (Input.GetMouseButtonDown(1))
        {
            UnitMove();
        }
    }



    public static void Swap<T> (ref T lhs, ref T rhs) {
		T temp = lhs;
		lhs = rhs;
		rhs = temp;
	}
}
