using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

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
    public List<GameObject> allSelectableUnits;


    public GameObject prefabProtein;
    GameObject spawnedProtein;

    public GameObject TouchButton;
    public GameObject HeatButton;

    // Use this for initialization
    void Start()
    {

        if (Input.touchSupported)
        {
            Task welcome = new Task();
            welcome.Text = "Welcome to Apoptosis! This tutorial will teach you the controls and basic actions that can be taken in the game. Press Skip to continue.";
            welcome.Initialize = EmptyInitialization;
            welcome.IsComplete = AnyKeyCondition;
            Tasklist.Add(welcome);
            Task unitButtonSelect = new Task();
            unitButtonSelect.Text = "Touch the Big Circle Button to turn it on.";
            unitButtonSelect.Initialize = EmptyInitialization;
            unitButtonSelect.IsComplete = UnitSelectionButton;
            Tasklist.Add(unitButtonSelect);
            Task unitSelect = new Task();
            unitSelect.Text = "One finger touch your units or one finger touch and drag over any of your units to select them.";
            unitSelect.Initialize = EmptyInitialization;
            unitSelect.IsComplete = UnitSelectionCondition;
            Tasklist.Add(unitSelect);
            Task targetButtonSelect = new Task();
            targetButtonSelect.Text = "If the Button is on, touch the Big Circle Button to turn it off.";
            targetButtonSelect.Initialize = EmptyInitialization;
            targetButtonSelect.IsComplete = UnitSelectionButton;
            Tasklist.Add(unitButtonSelect);
            Task targetSelection = new Task();
            targetSelection.Text = "Now enemy units and proteins can be targeted. One finger touch or one finger touch and drag over many to set them as a target.";
            targetSelection.Initialize = TargetSelectionInitialization;
            targetSelection.IsComplete = TargetSelectionCondition;
            Tasklist.Add(targetSelection);
            Task proteinConsumption = new Task();
            proteinConsumption.Text = "Targeting a protein will tell your selected units to consume it. Consume the protein.";
            proteinConsumption.Initialize = EmptyInitialization;
            proteinConsumption.IsComplete = ProteinConsumptionCondition;
            Tasklist.Add(proteinConsumption);
            Task stemLesson = new Task();
            stemLesson.Text = "The Stem Cell you've been controlling is the basis of your army. It's important not to lose all of these units. Let's Start by making more. Press Skip to continue.";
            stemLesson.Initialize = EmptyInitialization;
            stemLesson.IsComplete = AnyKeyCondition;
            Tasklist.Add(stemLesson);
            Task mitosis = new Task();
            mitosis.Text = "Press Split Button to perform mitosis on all selected units. This will divide any selected cells into 2. Make at least 5 cells.";
            mitosis.Initialize = EmptyInitialization;
            mitosis.IsComplete = MitosisCondition;
            Tasklist.Add(mitosis);
            Task replenish = new Task();
            replenish.Text = "Protein acts as your cells health. Every cell that divides splits their protein with the new cell. All those divides has left your cells starved. Eat these proteins to replenish.";
            replenish.Initialize = ReplenishInitialization;
            replenish.IsComplete = ReplenishCondition;
            Tasklist.Add(replenish);
            Task mutateButton = new Task();
            mutateButton.Text = "Now that we have a few cells, let's gain some diversity. Select one of your stem cells and press the Evolve Button.";
            mutateButton.Initialize = EmptyInitialization;
            mutateButton.IsComplete = EvolveButtonPressed;
            Tasklist.Add(mutateButton);
            Task mutateHeat = new Task();
            mutateHeat.Text = "Now press the Flame Button to mutate your cell into a faster heat cell. Remember to leave at least one stem cell.";
            mutateHeat.Initialize = EmptyInitialization;
            mutateHeat.IsComplete = MutateHeatCondition;
            Tasklist.Add(mutateHeat);
            Task checkEvolveButton = new Task();
            checkEvolveButton.Text = "Great! Now select another stem cell and press the Evolve Button.";
            checkEvolveButton.Initialize = EmptyInitialization;
            checkEvolveButton.IsComplete = EvolveButtonPressed;
            Tasklist.Add(checkEvolveButton);
            Task mutateCold = new Task();
            mutateCold.Text = "Now press the Snow Flake to mutate it into a tougher cold cell. Remember to leave at least one stem cell.";
            mutateCold.Initialize = EmptyInitialization;
            mutateCold.IsComplete = MutateColdCondition;
            Tasklist.Add(mutateCold);
            Task checkAlkaliArea = new Task();
            checkAlkaliArea.Text = "Stem cells must be incubated in an alkali zone to mutate into an alkali cell. Move a stem cell there now and press the Evolve Button.";
            checkAlkaliArea.Initialize = EmptyInitialization;
            checkAlkaliArea.IsComplete = EvolveButtonPressed;
            Tasklist.Add(checkAlkaliArea);
            Task mutateAlkali = new Task();
            mutateAlkali.Text = "Press the Bubble Button to mutate your cell into an alkali cell. Remember to leave at least one stem cell.";
            mutateAlkali.Initialize = EmptyInitialization;
            mutateAlkali.IsComplete = MutateAlkaliCondition;
            Tasklist.Add(mutateAlkali);
            Task checkAcidicArea = new Task();
            checkAcidicArea.Text = "Stem cells must be incubated in an acidic zone to mutate into an acidic cell. Move a stem cell there now and press the Evolve Button.";
            checkAcidicArea.Initialize = EmptyInitialization;
            checkAcidicArea.IsComplete = EvolveButtonPressed;
            Tasklist.Add(checkAcidicArea);
            Task mutateAcidic = new Task();
            mutateAcidic.Text = "Press the Skull&Crossbones to mutate your cell into an acidic cell. Remember to leave at least one stem cell.";
            mutateAcidic.Initialize = EmptyInitialization;
            mutateAcidic.IsComplete = MutateAcidicCondition;
            Tasklist.Add(mutateAcidic);
            Task cancerLesson = new Task();
            cancerLesson.Text = "Stem cells can divide indefinitely with no repurcussions. Heat and cold cells can divide too, but the more they divide the higher the chance they have of becoming cancerous. Press Skip to continue.";
            cancerLesson.Initialize = EmptyInitialization;
            cancerLesson.IsComplete = AnyKeyCondition;
            Tasklist.Add(cancerLesson);
            Task cancerLesson2 = new Task();
            cancerLesson2.Text = "Cancer cells cannot be controlled and will attack everything. Select at least one stem, heat and cold cell then press 'D' to divide each one. Make at least 2 of each.";
            cancerLesson2.Initialize = EmptyInitialization;
            cancerLesson2.IsComplete = CancerLesson2Condition;
            Tasklist.Add(cancerLesson2);
            Task evolutionLesson = new Task();
            evolutionLesson.Text = "Some cells can be combined to evolve into a more powerfull cell. Stem cells cannot be merged. Press Skip to continue.";
            evolutionLesson.Initialize = EmptyInitialization;
            evolutionLesson.IsComplete = AnyKeyCondition;
            Tasklist.Add(evolutionLesson);
            Task evolutionHeat = new Task();
            evolutionHeat.Text = "2 Heat cells can merge when they are in a heat area. Select two heat cells, move them to a heat area and press the Merge Button to combine them into an evolved heat cell.";
            evolutionHeat.Initialize = EmptyInitialization;
            evolutionHeat.IsComplete = EvolutionHeatCondition;
            Tasklist.Add(evolutionHeat);
            Task evolutionCold = new Task();
            evolutionCold.Text = "2 Cold cells can merge when they are in a cold area. Select two cold cells, move them to a cold area and press the Merge Button to combine them into an evolved cold cell.";
            evolutionCold.Initialize = EmptyInitialization;
            evolutionCold.IsComplete = EvolutionColdCondition;
            Tasklist.Add(evolutionCold);
            Task evolutionNerve = new Task();
            evolutionNerve.Text = "An acidic and an alkali cell can merge. Select one acidic and one alkali and press the Merge to combine them into a nerve cell.";
            evolutionNerve.Initialize = EmptyInitialization;
            evolutionNerve.IsComplete = EvolutionNerveCondition;
            Tasklist.Add(evolutionNerve);

            Task tutorialComplete = new Task();
            tutorialComplete.Text = "This completes the tutorial for Apoptosis. Good luck in your future games! Press Skip to return to the main menu.";
            tutorialComplete.Initialize = EmptyInitialization;
            tutorialComplete.IsComplete = AnyKeyCondition;
            Tasklist.Add(tutorialComplete);
        }
        else
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
            Task proteinDegeneration = new Task();
            proteinDegeneration.Text = "Your health degrades over time in other Game modes.";
            proteinDegeneration.Initialize = EmptyInitialization;
            proteinDegeneration.IsComplete = AnyKeyCondition;
            Tasklist.Add(proteinDegeneration);
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
            Task cancerLesson = new Task();
            cancerLesson.Text = "Stem cells can divide indefinitely with no repurcussions. Heat and cold cells can divide too, but the more they divide the higher the chance they have of becoming cancerous. Press any key to continue.";
            cancerLesson.Initialize = EmptyInitialization;
            cancerLesson.IsComplete = AnyKeyCondition;
            Tasklist.Add(cancerLesson);
            Task cancerLesson2 = new Task();
            cancerLesson2.Text = "Cancer cells cannot be controlled and will attack everything. Select at least one stem, heat and cold cell then press 'D' to divide each one. Make at least 2 of each.";
            cancerLesson2.Initialize = EmptyInitialization;
            cancerLesson2.IsComplete = CancerLesson2Condition;
            Tasklist.Add(cancerLesson2);
            Task evolutionLesson = new Task();
            evolutionLesson.Text = "Some cells can be combined to evolve into a more powerfull cell. Stem cells cannot be merged. Press any key to continue.";
            evolutionLesson.Initialize = EmptyInitialization;
            evolutionLesson.IsComplete = AnyKeyCondition;
            Tasklist.Add(evolutionLesson);
            Task evolutionHeat = new Task();
            evolutionHeat.Text = "2 Heat cells can merge when they are in a heat area. Select two heat cells, move them to a heat area and press 'Q' to combine them into an evolved heat cell.";
            evolutionHeat.Initialize = EmptyInitialization;
            evolutionHeat.IsComplete = EvolutionHeatCondition;
            Tasklist.Add(evolutionHeat);
            Task evolutionCold = new Task();
            evolutionCold.Text = "2 Cold cells can merge when they are in a cold area. Select two cold cells, move them to a cold area and press 'Q' to combine them into an evolved cold cell.";
            evolutionCold.Initialize = EmptyInitialization;
            evolutionCold.IsComplete = EvolutionColdCondition;
            Tasklist.Add(evolutionCold);
            Task evolutionNerve = new Task();
            evolutionNerve.Text = "An acidic and an alkali cell can merge. Select one acidic and one alkali and press 'Q' to combine them into a nerve cell.";
            evolutionNerve.Initialize = EmptyInitialization;
            evolutionNerve.IsComplete = EvolutionNerveCondition;
            Tasklist.Add(evolutionNerve);

            Task tutorialComplete = new Task();
            tutorialComplete.Text = "This completes the tutorial for Apoptosis. Good luck in your future games! Press any key to return to the main menu.";
            tutorialComplete.Initialize = EmptyInitialization;
            tutorialComplete.IsComplete = AnyKeyCondition;
            Tasklist.Add(tutorialComplete);
        }
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

    void FixedUpdate()
    {
        allSelectableUnits = PlayerControls.GetAllSelectableUnits();
        foreach (GameObject item in allSelectableUnits)
        {
            item.GetComponent<BaseCell>().isDepleting = false;
            item.GetComponent<BaseCell>().currentProtein = 400;
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
            Application.LoadLevel("MainMenu");
        }
    }

    bool AnyKeyCondition()
    {
        return Input.anyKeyDown && !Input.GetMouseButtonDown(0);
    }

    bool UnitSelectionButton()
    {
        return TouchButton.GetActive();
    }

    bool EvolveButtonPressed()
    {
        return HeatButton.GetActive();
    }

    bool TargetSelectionButton()
    {
        if (TouchButton.GetActive())
        {
            return false;
        }
        else
        {
            return true;
        }
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

    bool CancerLesson2Condition()
    {
        return PlayerControls.NumStemCells > 1
            && PlayerControls.NumHeatCells > 1
            && PlayerControls.NumColdCells > 1;
    }

    bool EvolutionHeatCondition()
    {
        return PlayerControls.NumTierTwoHeat > 0;
    }

    bool EvolutionColdCondition()
    {
        return PlayerControls.NumTierTwoCold > 0;
    }

    bool EvolutionNerveCondition()
    {
        return true; // dependent on nerve merge
        //return PlayerControls.NumNerveCells > 0;
    }

    void EmptyInitialization() { }
}
