using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class QuestPopUp : MonoBehaviour {

    // First Level:  Level Goal#Search this cave and destory#all the enemies!!!#Tips: Some proteins will regenerate.#Beware of traps!
    // Second Level: Level Goal#Search this cave and destory#all the enemies!!!#Beware of patrols!
    // Third Level:  Level Goal#Destroy all the enemies!!!#This time beware all the traps and patrols!

    public float delayTime;
    public string questText;
    private GUIContent textContent;
    private float currentTimeScale;
    private bool isPaused = false;

    void Awake() {
        if (GetComponent<PauseMenu>()) {
            GetComponent<PauseMenu>().enabled = false;
        }
        if (questText.Length < 1 || delayTime < 1) {
            Debug.LogError(ToString() + "Quest information/setting has NOT yet been set up!!");
            Destroy(gameObject);
        }
        questText = questText.Replace("#", "\n");
        textContent = new GUIContent(questText);
        currentTimeScale = Time.timeScale;

    }

    void Start() {
        StartCoroutine(DelayShowQuest(delayTime));

    }

    // Coroutine function
    IEnumerator DelayShowQuest(float delay) {
        yield return new WaitForSeconds(delay);
        isPaused = true;

    }

    void OnGUI() {
        if (isPaused) {
            Vector2 centerPos = new Vector2(Screen.currentResolution.width / 4, Screen.currentResolution.height / 4);
            Vector2 boxContentSize = GUI.skin.box.CalcSize(textContent);
            GUI.Box(
                new Rect(
                    centerPos.x,
                    centerPos.y,
                    boxContentSize.x,
                    boxContentSize.y
                ),
                textContent.text
            );
            if (GUI.Button(new Rect(centerPos.x, centerPos.y + boxContentSize.y + 5, boxContentSize.x, boxContentSize.y), "Countine")) {
                Time.timeScale = currentTimeScale;
                isPaused = false;
                GetComponent<PauseMenu>().enabled = true;

            }
            Time.timeScale = 0f;
        }
    }
}
