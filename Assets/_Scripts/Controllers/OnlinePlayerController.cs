using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class OnlinePlayerController: PlayerController
{
    private bool isOverUI = false;

    public void TurnOnOverUI() { isOverUI = true; }
    public void TurnOffOverUI() { isOverUI = false; }



    private int terrainLayer;

    public const int MAX_CAP = 20;
    public static int cap = 0;
    public GameObject movePin;
    public GameObject attackPin;

    public int NumStemCells = 0;
    public int NumHeatCells = 0;
    public int NumColdCells = 0;
    public int NumAcidicCells = 0;
    public int NumAlkaliCells = 0;
    public int NumNerveCells = 0;
    public int NumTierTwoCold = 0;
    public int NumTierTwoHeat = 0;


    public List<BaseCell> allSelectableUnits;
    public List<BaseCell> selectedUnits;
    public List<GameObject> allSelectableTargets;
    public List<GameObject> selectedTargets;
    //    List<BaseCell>[] groups;
    public Texture selector;

    Rect GUISelectRect;

    Vector2 origin = new Vector2();

    void Awake()
    {
        // Initialize variables
        selectedTargets.Clear();
        //        groups = new List<BaseCell>[10];
        allSelectableUnits = new List<BaseCell>();
        selectedUnits = new List<BaseCell>();
        terrainLayer = 1 << LayerMask.NameToLayer("Terrain");  // Layer masking for raycast clicking
        // ----------

        GameObject[] tmpArr = GameObject.FindGameObjectsWithTag("Unit"); // Get every cell in the game
        foreach (GameObject item in tmpArr) // Iterate through all the cells
        {
            BaseCell bCell = item.GetComponent<BaseCell>(); // Upcast each cell to a base cell
            if (!bCell.isAIPossessed && bCell.isMine) // If the cell belongs to this player
            {
                allSelectableUnits.Add(item.GetComponent<BaseCell>()); // Add the cell to the players controllable units
            }
            else if (bCell.isAIPossessed && !bCell.isMine) // If the cell belongs to this player
            {
                allSelectableTargets.Add(item); // Add the cell to the players controllable units
            }
        }

        tmpArr = GameObject.FindGameObjectsWithTag("Protein"); // Get every cell in the game
        foreach (GameObject item in tmpArr) // Iterate through all the cells
        {
            allSelectableTargets.Add(item); // Add the cell to the players controllable units
        }
    }

    [PunRPC]
    public void AddNewCell(BaseCell _in)
    {
        if (!_in.gameObject.GetPhotonView().isMine)
        {
            allSelectableTargets.Add(_in.gameObject);
        }
        else
        {
            _in.isSelected = true;
            allSelectableUnits.Add(_in);
            selectedUnits.Add(_in);
            CheckSelectedUnits();
        }
    }

    [PunRPC]
    public void RemoveDeadCell(BaseCell _in)
    {
        _in.isSelected = false;
        allSelectableUnits.Remove(_in);
        selectedUnits.Remove(_in);
        CheckSelectedUnits();
    }

    [PunRPC]
    public void AddNewProtein(Protein _in)
    {
        allSelectableTargets.Add(_in.gameObject);
        selectedTargets.Add(_in.gameObject);
        CheckSelectedUnits();
    }

    [PunRPC]
    public void RemoveConsumedProtein(Protein _in)
    {
        allSelectableTargets.Remove(_in.gameObject);
        selectedTargets.Remove(_in.gameObject);
        CheckSelectedUnits();
    }

    public void RemoveTarget(GameObject _in)
    {
        allSelectableTargets.Remove(_in);
        selectedTargets.Remove(_in);
    }

    public List<GameObject> GetAllSelectableUnits()
    {
        List<GameObject> allSelectableObjects = new List<GameObject>(); // Initialize a list of GameObjects
        foreach (BaseCell item in allSelectableUnits) // For each of the player's controllable cells
        {
            allSelectableObjects.Add(item.gameObject); // Add the cell's GameObject to the list
        }
        return allSelectableObjects; // Return the list
    }

    public List<GameObject> GetAllSelectableTargets()
    {
        List<GameObject> allSelectableObjects = new List<GameObject>(); // Initialize a list of GameObjects
        foreach (GameObject item in allSelectableTargets) // For each of the player's controllable cells
        {
            allSelectableObjects.Add(item.gameObject); // Add the cell's GameObject to the list
        }
        return allSelectableObjects; // Return the list
    }

    public void UnitSelection(Vector2 origin)
    {
        // Initialize variables
        selectedTargets.Clear();
        //        groups = new List<BaseCell>[10];
        allSelectableUnits = new List<BaseCell>();
        selectedUnits = new List<BaseCell>();
        terrainLayer = 1 << LayerMask.NameToLayer("Terrain");  // Layer masking for raycast clicking
        // ----------

        List<GameObject> tmpArr = GameObjectManager.FindAllUnits(); // Get every cell in the game
        foreach (GameObject item in tmpArr) // Iterate through all the cells
        {
            BaseCell bCell = item.GetComponent<BaseCell>(); // Upcast each cell to a base cell
            if (bCell)
            {
                if (!bCell.isAIPossessed && bCell.isMine) // If the cell belongs to this player
                {
                    allSelectableUnits.Add(item.GetComponent<BaseCell>()); // Add the cell to the players controllable units
                }
            }

        }

        tmpArr.Clear();
        tmpArr = GameObjectManager.FindAllUnits(); // Get every cell in the game
        foreach (GameObject item in tmpArr) // Iterate through all the cells
        {
            BaseCell bCell = item.GetComponent<BaseCell>(); // Upcast each cell to a base cell
            if (bCell)
            {
                if (bCell.isAIPossessed && !bCell.isMine) // If the cell belongs to this player
                {
                    allSelectableTargets.Add(item); // Add the cell to the players controllable units
                }
            }
        }

        tmpArr.Clear();
        tmpArr = GameObject.FindGameObjectsWithTag("Protein").ToList<GameObject>(); // Get every cell in the game
        foreach (GameObject item in tmpArr) // Iterate through all the cells
        {
            allSelectableTargets.Add(item); // Add the cell to the players controllable units
        }

    }

    // Update is called once per frame
    void Update()
    {
        selectedUnits.RemoveAll(item => item == null);
        selectedTargets.RemoveAll(item => item == null);
        allSelectableTargets.RemoveAll(item => item == null);

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
        //Vector3 topleft = new Vector3(GUISelectRect.xMin, GUISelectRect.yMin, Camera.main.transform.position.z);
        //Vector3 bottomright = new Vector3(GUISelectRect.xMax, GUISelectRect.yMin, Camera.main.transform.position.z);

        if (Input.GetKeyDown(KeyCode.D)) // If the player presses D
        {
            UnitSplit();
            CheckSelectedUnits();
        }

        if (Input.GetKeyDown(KeyCode.S)) // If the player presses S
        {
            UnitStop();
        }

        if (Input.GetKeyDown(KeyCode.C)) // If the player presses C
        {
            EventManager.Evolve(CellType.ACIDIC_CELL);
            CheckSelectedUnits();

        }

        if (Input.GetKeyDown(KeyCode.V)) // If the player presses V
        {
            EventManager.Evolve(CellType.ALKALI_CELL);
            CheckSelectedUnits();
        }

        if (Input.GetKeyDown(KeyCode.X)) // If the player presses X
        {
            EventManager.Evolve(CellType.HEAT_CELL);
            CheckSelectedUnits();

        }

        if (Input.GetKeyDown(KeyCode.Z)) // If the player presses Z
        {
            EventManager.Evolve(CellType.COLD_CELL);

            CheckSelectedUnits();
        }

        if (!isOverUI)
        {
            if (Input.GetMouseButtonDown(0)) // If the player left-clicks
            {
                GUISelectRect.xMin = Input.mousePosition.x;
                GUISelectRect.yMin = Input.mousePosition.y;
                GUISelectRect.xMax = Input.mousePosition.x;
                GUISelectRect.yMax = Input.mousePosition.y;

                GUISelectRect.xMin = Input.mousePosition.x;
                GUISelectRect.yMin = -Input.mousePosition.y + Screen.height;
                origin = Input.mousePosition;
                origin.y = -origin.y + Screen.height;
                CheckSelectedUnits();
            }
            else if (Input.GetMouseButtonUp(0)) // When the player releases left-click
            {
                GUISelectRect.yMax = GUISelectRect.yMin;
                GUISelectRect.xMax = GUISelectRect.xMin;
                if (selectedUnits.Count == 0)
                {
                    RaycastHit hitInfo;
                    Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(screenRay, out hitInfo, 1000.0f))
                    {
                        BaseCell hitCell = hitInfo.collider.gameObject.GetComponent<BaseCell>();
                        if (allSelectableUnits.Contains(hitCell))
                        {
                            hitInfo.collider.gameObject.GetComponent<BaseCell>().isSelected = true;
                            selectedUnits.Add(hitInfo.collider.gameObject.GetComponent<BaseCell>());
                        }
                    }
                }
                CheckSelectedUnits();
            }
            else if (Input.GetMouseButton(0)) // If the player has left-click held down
            {

                UnitSelection(origin);

            }

            if (Input.GetMouseButtonDown(1)) // If the player right-clicks
            {
                GUISelectRect.xMin = Input.mousePosition.x;
                GUISelectRect.yMin = Input.mousePosition.y;
                GUISelectRect.xMax = Input.mousePosition.x;
                GUISelectRect.yMax = Input.mousePosition.y;

                GUISelectRect.xMin = Input.mousePosition.x;
                GUISelectRect.yMin = -Input.mousePosition.y + Screen.height;
                origin = Input.mousePosition;
                origin.y = -origin.y + Screen.height;
            }
            else if (Input.GetMouseButtonUp(1)) // When the player releases right-click
            {

                GUISelectRect.yMax = GUISelectRect.yMin;
                GUISelectRect.xMax = GUISelectRect.xMin;
                if (selectedTargets.Count == 0)
                {
                    RaycastHit hitInfo;
                    Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                    if (Physics.Raycast(screenRay, out hitInfo, 1000.0f))
                    {
                        GameObject hitObject = hitInfo.collider.gameObject;
                        if (allSelectableTargets.Contains(hitObject))
                        {
                            selectedTargets.Add(hitObject);
                        }
                    }


                }
                if (selectedTargets.Count > 0)
                {
                    foreach (BaseCell item in selectedUnits)
                    {
                        item.SetTargets(selectedTargets);
                        item.SetPrimaryTarget(selectedTargets[0]);
                    }
                    if (selectedTargets[0].tag == "Protein")
                    {
                        UnitHarvest();
                    }
                    else
                        UnitAttack();
                }
                else
                    UnitMove();

            }
            else if (Input.GetMouseButton(1)) // If the player has right-click held down
            {
                TargetSelection(origin);
            }
        }

        if (Input.GetMouseButton(2))
        {
            UnitAttackMove();
        }
    }

}
