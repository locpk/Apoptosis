using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFadeInOut : MonoBehaviour
{
    public Image FadeImg;
    public float fadeSpeed = 1.5f;
    public bool sceneStarting = true;

     public float timer = 1.0f;
     public Texture2D cursor_Normal;
     public CursorMode cursorMode = CursorMode.Auto;
     private bool tablet_mode = false;

       
    void Awake()
    {
        FadeImg.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
       
        if (Input.touchSupported)
        {
            tablet_mode = true;
        }

        //resets the cersor to normal if not in edge
        if (tablet_mode)
        {
           
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
        }
        else
            Cursor.SetCursor(cursor_Normal, Vector2.zero, cursorMode);
    }

    void Update()
    {
        // If the scene is starting...
        if (sceneStarting)
            // ... call the StartScene function.
            StartScene();


         timer -= Time.deltaTime;

        if (timer <= 0.0f || Input.anyKeyDown)
        {
            EndScene(1);
        }
    }


    void FadeToClear()
    {
        // Lerp the colour of the image between itself and transparent.
        FadeImg.color = Color.Lerp(FadeImg.color, Color.clear, fadeSpeed * Time.deltaTime);
    }


    void FadeToBlack()
    {
        // Lerp the colour of the image between itself and black.
        FadeImg.color = Color.Lerp(FadeImg.color, Color.black, fadeSpeed * Time.deltaTime);
    }


    void StartScene()
    {
        // Fade the texture to clear.
        FadeToClear();

        // If the texture is almost clear...
        if (FadeImg.color.a <= 0.05f)
        {
            // ... set the colour to clear and disable the RawImage.
            FadeImg.color = Color.clear;
            FadeImg.enabled = false;

            // The scene is no longer starting.
            sceneStarting = false;
        }
    }


    public void EndScene(int SceneNumber)
    {
        // Make sure the RawImage is enabled.
        FadeImg.enabled = true;

        // Start fading towards black.
        FadeToBlack();

        // If the screen is almost black...
        if (FadeImg.color.a >= 0.95f)
            // ... reload the level
            Application.LoadLevel(SceneNumber);
    }
}   