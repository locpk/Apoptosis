using UnityEngine;
using System.Collections;

class Task
{
    string text;

    public string Text
    {
        get { return text; }
        set { text = value; }
    }

    public delegate bool condition();

    condition isComplete;

    internal condition IsComplete
    {
        get { return isComplete; }
        set { isComplete = value; }
    }

    public delegate void initializeTask();

    initializeTask initialize;

    internal initializeTask Initialize
    {
        get { return initialize; }
        set { initialize = value; }
    }
}

public class TutorialController : MonoBehaviour
{

    int currentTask = 0;
    public GameObject taskCanvas;
    public UnityEngine.UI.Text currentTaskText;
    public UnityEngine.UI.Button showTaskButton;
    System.Collections.Generic.List<Task> Tasklist = new System.Collections.Generic.List<Task>();

    public PlayerController PlayerControls;

    public GameObject spawnedProtein;

    // Use this for initialization
    void Start()
    {
        Task welcome = new Task();
        welcome.Text = "Welcome to Apoptosis! This tutorial will teach you the controls and basic actions that can be taken in the game. Press any key to continue.";
        welcome.Initialize = EmptyInitialization;
        welcome.IsComplete = WelcomeCondition;
        Tasklist.Add(welcome);
        Task unitSelect = new Task();
        unitSelect.Text = "Left-click one of your units or left-click and drag over any of your units to select them.";
        unitSelect.Initialize = EmptyInitialization;
        unitSelect.IsComplete = UnitSelectionCondition;
        Tasklist.Add(unitSelect);
        Task targetSelection = new Task();
        targetSelection.Text = "Enemy units and proteins can be targeted. Right-click one or Right-click and drag over many to set them as a target.";
        targetSelection.Initialize = UnitSelectionInitialization;
        targetSelection.IsComplete = TargetSelectionCondition;
        Tasklist.Add(targetSelection);
        Task proteinConsumption = new Task();
        proteinConsumption.Text = "Targeting a protein will tell your selected units to consume it. Consume the protein.";
        proteinConsumption.Initialize = EmptyInitialization;
        proteinConsumption.IsComplete = ProteinConsumptionCondition;
        Tasklist.Add(proteinConsumption);

        currentTaskText.text = Tasklist[0].Text;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTask < Tasklist.Count && Tasklist[currentTask].IsComplete())
        {
            NextTask();
        }
    }

    public void HideTask()
    {
        taskCanvas.gameObject.SetActive(false);
        showTaskButton.gameObject.SetActive(true);
    }

    public void ShowTask()
    {
        taskCanvas.gameObject.SetActive(true);
        showTaskButton.gameObject.SetActive(false);
    }

    public void NextTask()
    {
        currentTask++;
        if (currentTask < Tasklist.Count)
        {
            currentTaskText.text = Tasklist[currentTask].Text;
            Tasklist[currentTask].Initialize();
            ShowTask();
        }
        else
        {
            taskCanvas.gameObject.SetActive(false);
            showTaskButton.gameObject.SetActive(false);
            print("Should Be Complete");
        }
    }

    bool WelcomeCondition()
    {
        return Input.anyKeyDown && !Input.GetMouseButtonDown(0);
    }

    void UnitSelectionInitialization()
    {
        Vector3 spawnPos = Camera.main.transform.position;
        spawnPos.y = 0;
        GameObject.Instantiate(spawnedProtein, spawnPos, Quaternion.Euler(90, 0 ,0));
    }

    bool UnitSelectionCondition()
    {
        return PlayerControls.selectedUnits.Count > 0 && Input.GetMouseButtonUp(0);
    }

    bool TargetSelectionCondition()
    {
        return PlayerControls.selectedUnits.Count > 0 && PlayerControls.selectedTargets.Count > 0 && Input.GetMouseButtonUp(1);
    }

    bool ProteinConsumptionCondition()
    {
        return false;
    }

    void EmptyInitialization(){}
}
