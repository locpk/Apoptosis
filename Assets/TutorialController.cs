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

    public GameObject prefabProtein;
    GameObject spawnedProtein;

    // Use this for initialization
    void Start()
    {
        Task welcome = new Task();
        welcome.Text = "Welcome to Apoptosis! This tutorial will teach you the controls and basic actions that can be taken in the game. Press any key to continue.";
        welcome.Initialize = EmptyInitialization;
        welcome.IsComplete = AnyKeyCondition;
        Tasklist.Add(welcome);
        Task unitSelect = new Task();
        unitSelect.Text = "Left-click one of your units or left-click and drag over any of your units to select them.";
        unitSelect.Initialize = EmptyInitialization;
        unitSelect.IsComplete = UnitSelectionCondition;
        Tasklist.Add(unitSelect);
        Task targetSelection = new Task();
        targetSelection.Text = "Enemy units and proteins can be targeted. Right-click one or Right-click and drag over many to set them as a target.";
        targetSelection.Initialize = TargetSelectionInitialization;
        targetSelection.IsComplete = TargetSelectionCondition;
        Tasklist.Add(targetSelection);
        Task proteinConsumption = new Task();
        proteinConsumption.Text = "Targeting a protein will tell your selected units to consume it. Consume the protein.";
        proteinConsumption.Initialize = EmptyInitialization;
        proteinConsumption.IsComplete = ProteinConsumptionCondition;
        Tasklist.Add(proteinConsumption);
        Task stemLesson = new Task();
        stemLesson.Text = "The Stem Cell you've been controlling is the basis of your army. It's important not to lose all of these units. Let's Start by making more. Press any key to continue.";
        stemLesson.Initialize = EmptyInitialization;
        stemLesson.IsComplete = AnyKeyCondition;
        Tasklist.Add(stemLesson);
        Task mitosis = new Task();
        mitosis.Text = "Press 'D' to perform mitosis on all selected units. This will divide any selected cells into 2. Make at least 5 cells.";
        mitosis.Initialize = EmptyInitialization;
        mitosis.IsComplete = MitosisCondition;
        Tasklist.Add(mitosis);
        Task replenish = new Task();
        replenish.Text = "Protein acts as your cells health. Every cell that divides splits their protein with the new cell. All those divides has left your cells starved. Eat these proteins to replenish.";
        replenish.Initialize = ReplenishInitialization;
        replenish.IsComplete = ReplenishCondition;
        Tasklist.Add(replenish);
        Task mutateHeat = new Task();
        mutateHeat.Text = "Now that we have a few cells, let's gain some diversity. Select one of your stem cells and press 'X' to mutate it into a faster heat cell. Remember to leave at least one stem cell.";
        mutateHeat.Initialize = EmptyInitialization;
        mutateHeat.IsComplete = MutateHeatCondition;
        Tasklist.Add(mutateHeat);
        Task mutateCold = new Task();
        mutateCold.Text = "Great! Now select another stem cell and press 'Z' to mutate it into a tougher cold cell. Remember to leave at least one stem cell.";
        mutateCold.Initialize = EmptyInitialization;
        mutateCold.IsComplete = MutateColdCondition;
        Tasklist.Add(mutateCold);
        Task mutateAlkali = new Task();
        mutateAlkali.Text = "Stem cells must be incubated in an alkali zone to mutate into an alkali cell. Move a stem cell there and press 'V' to mutate. Remember to leave at least one stem cell.";
        mutateAlkali.Initialize = EmptyInitialization;
        mutateAlkali.IsComplete = MutateAlkaliCondition;
        Tasklist.Add(mutateAlkali);
        Task mutateAcidic = new Task();
        mutateAcidic.Text = "Stem cells must be incubated in an acidic zone to mutate into an acidic cell. Move a stem cell there and press 'C' to mutate. Remember to leave at least one stem cell.";
        mutateAcidic.Initialize = EmptyInitialization;
        mutateAcidic.IsComplete = MutateAcidicCondition;
        Tasklist.Add(mutateAcidic);

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

    bool AnyKeyCondition()
    {
        return Input.anyKeyDown && !Input.GetMouseButtonDown(0);
    }

    bool UnitSelectionCondition()
    {
        return PlayerControls.selectedUnits.Count > 0 && Input.GetMouseButtonUp(0);
    }

    void TargetSelectionInitialization()
    {
        Vector3 spawnPos = Camera.main.transform.position;
        spawnPos.y = 0;
        spawnedProtein = GameObject.Instantiate(prefabProtein, spawnPos, Quaternion.Euler(90, 0, 0)) as GameObject;
        PlayerControls.allSelectableTargets.Add(spawnedProtein);
    }

    bool TargetSelectionCondition()
    {
        return PlayerControls.selectedTargets.Count > 0 && Input.GetMouseButtonUp(1);
    }

    bool ProteinConsumptionCondition()
    {
        return spawnedProtein == null;
    }

    bool MitosisCondition()
    {
        return PlayerControls.allSelectableUnits.Count > 4;
    }

    void ReplenishInitialization()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPos = Camera.main.transform.position;
            spawnPos.x = 4 * i;
            spawnPos.y = 0;
            spawnedProtein = GameObject.Instantiate(prefabProtein, spawnPos, Quaternion.Euler(90, 0, 0)) as GameObject;
            PlayerControls.allSelectableTargets.Add(spawnedProtein);
        }
    }

    bool ReplenishCondition()
    {
        return PlayerControls.allSelectableTargets.Count < 1;
    }

    bool MutateHeatCondition()
    {
        return PlayerControls.NumHeatCells > 0;
    }

    bool MutateColdCondition()
    {
        return PlayerControls.NumColdCells > 0;
    }

    bool MutateAlkaliCondition()
    {
        return PlayerControls.NumAlkaliCells > 0;
    }

    bool MutateAcidicCondition()
    {
        return PlayerControls.NumAcidicCells > 0;
    }

    void EmptyInitialization(){}
}
