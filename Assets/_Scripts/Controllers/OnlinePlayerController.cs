using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class OnlinePlayerController: MonoBehaviour
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

    public void TargetSelection(Vector2 origin)
    {
        if (Input.mousePosition.x >= origin.x)
        {
            GUISelectRect.xMax = Input.mousePosition.x;
        }
        else
        {
            GUISelectRect.xMin = Input.mousePosition.x;
        }

        if (-Input.mousePosition.y + Screen.height >= origin.y)
        { GUISelectRect.yMax = -Input.mousePosition.y + Screen.height; }
        else
        { GUISelectRect.yMin = -Input.mousePosition.y + Screen.height; }

        selectedTargets.Clear();
        foreach (GameObject item in allSelectableTargets)
        {
            Vector3 itemPos = Camera.main.WorldToScreenPoint(item.transform.position);
            itemPos.y = -itemPos.y + Screen.height;
            if (GUISelectRect.Contains(itemPos))
            {
                selectedTargets.Add(item);
            }
        }
    }

    public void UnitMove()
    {
        if (selectedUnits.Count <= 0)
        {
            return;
        }
        // Modified by using raycast
        RaycastHit hitInfo;
        Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(screenRay, out hitInfo, 1000.0f, terrainLayer))
        {
            EventManager.Move(hitInfo.point);
            GameObject.Instantiate(movePin, hitInfo.point, Quaternion.Euler(90.0f,0.0f,0.0f));
        }

    }


    public void UnitAttackMove()
    {
        // Modified by using raycast
        RaycastHit hitInfo;
        Ray screenRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(screenRay, out hitInfo, 1000.0f, terrainLayer))
        {
            EventManager.AttackMove(hitInfo.point);
        }
    }

    public void UnitAttack()
    {
        EventManager.Attack(selectedTargets[0]);

    }


    public void UnitSplit()
    {
        EventManager.Split();
        CheckSelectedUnits();
    }

    public void UnitEvolve(int cellNum)
    {
        switch (cellNum)
        {
            case 0: //turn into heat cell
                EventManager.Evolve(CellType.HEAT_CELL);
                break;
            case 1: //turn into cold cell
                EventManager.Evolve(CellType.COLD_CELL);
                break;
            case 2: //turn into acidic cell
                EventManager.Evolve(CellType.ACIDIC_CELL);
                break;
            case 3: //turn into alkali cell
                EventManager.Evolve(CellType.ALKALI_CELL);
                break;
            default:
                break;
        }
        CheckSelectedUnits();
    }

    public void UnitHarvest()
    {
        EventManager.Consume(selectedTargets[0]);
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
                CheckSelectedUnits();
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
            if (!isOverUI)
            {
                if (Input.GetMouseButton(0))
                {
                    GUI.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);
                }
                else
                {
                    GUI.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
                }

                GUI.DrawTexture(GUISelectRect, selector, ScaleMode.StretchToFill, true);
            }

        }
        GUI.color = new Color(0.0f, 1.0f, 0.0f,1.0f);
        foreach (BaseCell item in selectedUnits)
        {
            if (item)
            {
                Vector3 drawLoc = Camera.main.WorldToScreenPoint(item.transform.position);
                float left = drawLoc.x - (float)4;
                float top = -(drawLoc.y - (float)4) + Screen.height;
                Rect location = new Rect(left, top, (float)8, (float)8);
                GUI.DrawTexture(location, selector);
            }
        }

        if (Time.timeScale > 0.0f)
        {
            GUI.BeginGroup(new Rect(Screen.width * 0.5f - 320, 15, 900, 100));
            GUI.Box(new Rect(0, 0, 75, 60), "Stem Cells: ");
            GUI.Label(new Rect(35, 35, 50, 50), NumStemCells.ToString());

            GUI.Box(new Rect(80, 0, 75, 60), "Heat Cells: ");
            GUI.Label(new Rect(115, 35, 50, 50), NumHeatCells.ToString());

            GUI.Box(new Rect(160, 0, 75, 60), "Cold Cells: ");
            GUI.Label(new Rect(195, 35, 50, 50), NumColdCells.ToString());

            GUI.Box(new Rect(240, 0, 75, 60), "Acidic Cells: ");
            GUI.Label(new Rect(275, 35, 50, 50), NumAcidicCells.ToString());

            GUI.Box(new Rect(320, 0, 75, 60), "Alkali Cells: ");
            GUI.Label(new Rect(355, 35, 50, 50), NumAlkaliCells.ToString());

            GUI.Box(new Rect(400, 0, 75, 60), "Nerve Cells: ");
            GUI.Label(new Rect(435, 35, 50, 50), NumNerveCells.ToString());

            GUI.Box(new Rect(480, 0, 75, 60), "Tier 2\nHeat Cells: ");
            GUI.Label(new Rect(515, 35, 50, 50), NumTierTwoHeat.ToString());

            GUI.Box(new Rect(560, 0, 75, 60), "Tier 2\nCold Cells: ");
            GUI.Label(new Rect(595, 35, 50, 50), NumTierTwoCold.ToString());

            GUI.Box(new Rect(720, 0, 75, 60), "Cap: ");
            GUI.Label(new Rect(755, 35, 50, 50), cap.ToString());
            GUI.EndGroup();
        }

       

    }

    public void FixedUpdate()
    {
        





    }

    public void UnitStop()
    {
        selectedTargets.Clear();
        EventManager.Stop();
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

    public void CheckSelectedUnits()
    {
        NumStemCells = 0;
        NumHeatCells = 0;
        NumColdCells = 0;
        NumAcidicCells = 0;
        NumAlkaliCells = 0;
        NumTierTwoCold = 0;
        NumTierTwoHeat = 0;

        foreach (var item in selectedUnits)
        {
            switch (item.celltype)
            {
                case CellType.STEM_CELL:
                    NumStemCells++;
                    break;
                case CellType.HEAT_CELL:
                    NumHeatCells++;
                    break;
                case CellType.COLD_CELL:
                    NumColdCells++;
                    break;
                case CellType.ACIDIC_CELL:
                    NumAcidicCells++;
                    break;
                case CellType.ALKALI_CELL:
                    NumAlkaliCells++;
                    break;
                case CellType.COLD_CELL_TIRE2:
                    NumTierTwoCold++;
                    break;
                case CellType.HEAT_CELL_TIRE2:
                    NumTierTwoHeat++;
                    break;
                default:
                    break;
            }
        }
    }

}
