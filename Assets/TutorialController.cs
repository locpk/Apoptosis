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

}

public class TutorialController : MonoBehaviour
{

    int currentTask = 0;
    public GameObject taskCanvas;
    public UnityEngine.UI.Text currentTaskText;
    public UnityEngine.UI.Button showTaskButton;
    System.Collections.Generic.List<Task> Tasklist = new System.Collections.Generic.List<Task>();

    // Use this for initialization
    void Start()
    {
        Task welcome = new Task();
        welcome.Text = "Hello World";
        welcome.IsComplete = WelcomeCondition;
        Tasklist.Add(welcome);
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
        return false;
    }
}
