using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;



public class PlayerController : MonoBehaviour
{
    private bool isOverUI = false;


    public void TurnOnOverUI() { isOverUI = true; }
    public void TurnOffOverUI() { isOverUI = false; }


    public bool isTouch = true;
    private int terrainLayer;

    public const int MAX_CAP = 20;
    public static int cap = 0;
    public GameObject movePin;
    public GameObject attackPin;
    public GameObject moveLine;



    public int NumStemCells = 0;
    public int NumHeatCells = 0;
    public int NumColdCells = 0;
    public int NumAcidicCells = 0;
    public int NumAlkaliCells = 0;
    public int NumNerveCells = 0;
    public int NumTierTwoCold = 0;
    public int NumTierTwoHeat = 0;
    public int NumEnemiesLeft = 0;


    public List<BaseCell> allSelectableUnits;
    public List<BaseCell> selectedUnits;
    public List<GameObject> allSelectableTargets;
    public List<GameObject> selectedTargets;
    //    List<BaseCell>[] groups;
    public Texture selector;

    float fps;
    float initTouchTime;

    Rect GUISelectRect;

    Vector2 origin = new Vector2();

    void Awake()
    {

        if (Input.touchSupported)
        {
            isTouch = true;
        }
        else
        {
            isTouch = false;
        }

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
                if (PhotonNetwork.connected && item.GetPhotonView().isMine || !bCell.isAIPossessed && bCell.isMine) // If the cell belongs to this player
                {
                    allSelectableUnits.Add(item.GetComponent<BaseCell>()); // Add the cell to the players controllable units
                }
                else if (PhotonNetwork.connected && !item.GetPhotonView().isMine || bCell.isAIPossessed && !bCell.isMine) // If the cell belongs to this player
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

    public void AddNewProtein(Protein _in)
    {
        allSelectableTargets.Add(_in.gameObject);
        selectedTargets.Add(_in.gameObject);
        CheckSelectedUnits();
    }

    public void RemoveDeadCell(BaseCell _in)
    {
        _in.isSelected = false;
        allSelectableUnits.Remove(_in);
        selectedUnits.Remove(_in);
    }
    public void DeselectCell(BaseCell _in)
    {
        _in.isSelected = false;
        selectedUnits.Remove(_in);
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

        foreach (BaseCell item in selectedUnits)
        {
            item.isSelected = false;
        }
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


    public void TouchUnitSelection(Vector2 origin)
    {
        if (Input.GetTouch(0).position.x >= origin.x)
        {
            GUISelectRect.xMax = Input.GetTouch(0).position.x;
        }
        else
        {
            GUISelectRect.xMin = Input.GetTouch(0).position.x;
        }

        if (-Input.GetTouch(0).position.y + Screen.height >= origin.y)
        { GUISelectRect.yMax = -Input.GetTouch(0).position.y + Screen.height; }
        else
        { GUISelectRect.yMin = -Input.GetTouch(0).position.y + Screen.height; }

        foreach (BaseCell item in selectedUnits)
        {
            item.isSelected = false;
        }
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

    public void TouchTargetSelection(Vector2 origin)
    {
        Vector2 oneTouchPos = Input.GetTouch(0).position;
        if (oneTouchPos.x >= origin.x)
        {
            GUISelectRect.xMax = oneTouchPos.x;
        }
        else
        {
            GUISelectRect.xMin = oneTouchPos.x;
        }

        if (-oneTouchPos.y + Screen.height >= origin.y)
        { GUISelectRect.yMax = -oneTouchPos.y + Screen.height; }
        else
        { GUISelectRect.yMin = -oneTouchPos.y + Screen.height; }

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
            GameObject.Instantiate(movePin, hitInfo.point, Quaternion.Euler(90.0f, 0.0f, 0.0f));
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
        CheckEnemiesLeft();

    }


    public void UnitSplit()
    {
        EventManager.Split();

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
        GUI.Label(Rect.MinMaxRect(0, 0, Screen.width, Screen.height), fps.ToString());
        if (GUISelectRect.height != 0 && GUISelectRect.width != 0)
        {
            if (!isOverUI)
            {
                if (Input.touchCount == 1)
                {
                    GUI.color = new Color(0.0f, 0.0f, 1.0f, 0.5f);
                }
                else if (Input.touchCount == 2)
                {
                    GUI.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
                }

                GUI.DrawTexture(GUISelectRect, selector, ScaleMode.StretchToFill, true);
            }

        }
        GUI.color = new Color(0.0f, 1.0f, 0.0f, 1.0f);
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
            GUI.BeginGroup(new Rect(Screen.width * 0.5f - 400, 15, Screen.width * 0.5f + 500, 100));
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

            GUI.Box(new Rect(640, 0, 75, 60), "Enemies\nLeft: ");
            GUI.Label(new Rect(675, 35, 50, 50), NumEnemiesLeft.ToString());

            GUI.Box(new Rect(720, 0, 75, 60), "Cap: ");
            GUI.Label(new Rect(755, 35, 50, 50), cap.ToString());
            GUI.EndGroup();
        }



    }

    public void FixedUpdate()
    {
        CheckSelectedUnits();
        CheckEnemiesLeft();
        //Debug.Log(allSelectableUnits.Count);

    }

    public void UnitStop()
    {
        selectedTargets.Clear();
        EventManager.Stop();
    }

    public void UnitMerge()
    {
        EventManager.Merge();
    }

    public void UnitRevert()
    {
        EventManager.Revert();
    }


    void TouchUpdate()
    {
        if (!isOverUI && Time.timeScale > 0.0f)
        {
            if (Input.touchCount == 1 && selectedUnits.Count == 0)
            {
                Touch oneTouch = Input.GetTouch(0);
                switch (oneTouch.phase)
                {
                    case TouchPhase.Began:


                        GUISelectRect.xMax = oneTouch.position.x;
                        GUISelectRect.yMax = oneTouch.position.y;
                        GUISelectRect.xMin = oneTouch.position.x;
                        GUISelectRect.yMin = -oneTouch.position.y + Screen.height;

                        origin = oneTouch.position;
                        origin.y = -origin.y + Screen.height;
                        break;
                    case TouchPhase.Canceled:
                        break;
                    case TouchPhase.Ended:
                        TouchUnitSelection(origin);
                        break;
                    case TouchPhase.Moved:
                        TouchUnitSelection(origin);
                        break;
                    case TouchPhase.Stationary:
                        TouchUnitSelection(origin);
                        break;
                    default:
                        break;
                }
            }

            if (Input.touchCount == 1 && selectedUnits.Count > 0)
            {
                GUISelectRect.xMax = GUISelectRect.xMin;
                GUISelectRect.yMax = GUISelectRect.yMin;
                Vector3 initPos = -Vector3.one;
                Vector3 lastPos = -Vector3.one;
                Touch touch = Input.GetTouch(0);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        initTouchTime = Time.time;



                        RaycastHit hitInfo;
                        Ray screenRay = Camera.main.ScreenPointToRay(touch.position);

                        if (Physics.Raycast(screenRay, out hitInfo, 1000.0f, terrainLayer))
                        {
                            if (hitInfo.collider.tag == "Unit" && hitInfo.collider.GetComponent<BaseCell>().isSelected)
                            {
                                initPos = hitInfo.point;
                            }

                        }

                        break;
                    case TouchPhase.Canceled:
                        break;
                    case TouchPhase.Ended:
                        if (Time.time - initTouchTime <= 0.5f)
                        {
                            initTouchTime = 0.0f;
                            selectedUnits.Clear();
                            break;
                        }


                        RaycastHit hitInfo1;
                        Ray screenRay1 = Camera.main.ScreenPointToRay(touch.position);

                        if (Physics.Raycast(screenRay1, out hitInfo1, 1000.0f, terrainLayer))
                        {
                                lastPos = hitInfo1.point;
                        }


                        break;
                    case TouchPhase.Moved:
                        RaycastHit hitInfo2;
                        Ray screenRay2 = Camera.main.ScreenPointToRay(touch.position);

                        if (Physics.Raycast(screenRay2, out hitInfo2, 1000.0f, terrainLayer))
                        {
                                lastPos = hitInfo2.point;
                        }
                        Instantiate(movePin, lastPos, Quaternion.identity);
                        break;
                    case TouchPhase.Stationary:
                        break;
                    default:
                        break;
                }
                if (lastPos != initPos)
                {
                    EventManager.Move(lastPos);
                    initPos = lastPos = -Vector3.one;
                }
            }
        }
    }

    void MouseKeyBoardUpdate()
    {

        //Vector3 topleft = new Vector3(GUISelectRect.xMin, GUISelectRect.yMin, Camera.main.transform.position.z);
        //Vector3 bottomright = new Vector3(GUISelectRect.xMax, GUISelectRect.yMin, Camera.main.transform.position.z);

        if (Input.GetKeyDown(KeyCode.D)) // If the player presses D
        {
            UnitSplit();

        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) // If the player presses 1
        {
            UnitRevert();

        }

        if (Input.GetKeyDown(KeyCode.Q)) // If the player presses Q
        {
            UnitMerge();

        }

        if (Input.GetKeyDown(KeyCode.S)) // If the player presses S
        {
            UnitStop();
        }

        if (Input.GetKeyDown(KeyCode.C)) // If the player presses C
        {
            EventManager.Evolve(CellType.ACIDIC_CELL);


        }

        if (Input.GetKeyDown(KeyCode.V)) // If the player presses V
        {
            EventManager.Evolve(CellType.ALKALI_CELL);

        }

        if (Input.GetKeyDown(KeyCode.X)) // If the player presses X
        {
            EventManager.Evolve(CellType.HEAT_CELL);


        }

        if (Input.GetKeyDown(KeyCode.Z)) // If the player presses Z
        {
            EventManager.Evolve(CellType.COLD_CELL);


        }

        if (!isOverUI && Time.timeScale > 0.0f)
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
    // Update is called once per frame
    void Update()
    {
        fps = 1.0f / Time.deltaTime;
        cap = allSelectableUnits.Count;
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


        if (isTouch)
        {
            TouchUpdate();
        }
        else
        {
            MouseKeyBoardUpdate();
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

        NumStemCells = selectedUnits.FindAll(item => item.celltype == CellType.STEM_CELL).Count;
        NumHeatCells = selectedUnits.FindAll(item => item.celltype == CellType.HEAT_CELL).Count;
        NumColdCells = selectedUnits.FindAll(item => item.celltype == CellType.COLD_CELL).Count;
        NumAcidicCells = selectedUnits.FindAll(item => item.celltype == CellType.ACIDIC_CELL).Count;
        NumAlkaliCells = selectedUnits.FindAll(item => item.celltype == CellType.ALKALI_CELL).Count;
        NumTierTwoCold = selectedUnits.FindAll(item => item.celltype == CellType.COLD_CELL_TIRE2).Count;
        NumTierTwoHeat = selectedUnits.FindAll(item => item.celltype == CellType.HEAT_CELL_TIRE2).Count;
        NumNerveCells = selectedUnits.FindAll(item => item.celltype == CellType.NERVE_CELL).Count;
    }

    public void CheckEnemiesLeft()
    {
        NumEnemiesLeft = 0;


        List<BaseCell> enemies = new List<BaseCell>();

        foreach (GameObject item in allSelectableTargets)
        {
            if (item.tag == "Unit")
            {
                enemies.Add(item.GetComponent<BaseCell>());
            }
        }


        NumEnemiesLeft += enemies.FindAll(item => item.celltype == CellType.STEM_CELL).Count;
        NumEnemiesLeft += enemies.FindAll(item => item.celltype == CellType.HEAT_CELL).Count;
        NumEnemiesLeft += enemies.FindAll(item => item.celltype == CellType.COLD_CELL).Count;
        NumEnemiesLeft += enemies.FindAll(item => item.celltype == CellType.ACIDIC_CELL).Count;
        NumEnemiesLeft += enemies.FindAll(item => item.celltype == CellType.ALKALI_CELL).Count;
        NumEnemiesLeft += enemies.FindAll(item => item.celltype == CellType.HEAT_CELL_TIRE2).Count;
        NumEnemiesLeft += enemies.FindAll(item => item.celltype == CellType.COLD_CELL_TIRE2).Count;
        NumEnemiesLeft += enemies.FindAll(item => item.celltype == CellType.NERVE_CELL).Count;
    }
}
