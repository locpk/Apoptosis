using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class CamController : MonoBehaviour
{

    public enum CameraMode
    {
        GameView,
        FocusView,
    };

    public float minX, maxX, minY, maxY;
    public float scrollSpeed;
    public float touchScrollSpeed;
    public float scrollPercentage;
    public float minZoom;
    public float maxZoom;
    public GameObject quad;
    public GameObject mainCamera;
    public Camera minimapCamera;

    // for cursor stuff
    public Texture2D cursor_Right;
    public Texture2D cursor_Left;
    public Texture2D cursor_Up;
    public Texture2D cursor_Down;
    public Texture2D cursor_Normal;

    // for screen adges
    private Image shader_right;
    private Image shader_left;
    private Image shader_top;
    private Image shader_bottom;

    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;


    public PlayerController PlayerControls;
    public List<GameObject> allSelectableUnits;

    public float zoomValue;
    //    private float realtimeTimer;
    private Vector3 smoothFocusTarget;
    //private Vector3 smoothTargetPosition;
    //private Quaternion smoothTargetRotation;
    private CameraMode mode = CameraMode.GameView;

    private bool isOverUI = false;

    public void TurnOnOverUI() { isOverUI = true; }
    public void TurnOffOverUI() { isOverUI = false; }

    // Froze mouse when start
    private bool m_frozeMouse = true;
    private float m_zoomingTime;
    private bool tablet_mode = false;
    // Use this for initialization
    void Start()
    {
        if (PhotonNetwork.connected)
        {
            switch (PhotonNetwork.player.ID)
            {
                case 1:
                    smoothFocusTarget = GameObject.Find("Player - 1").transform.position;
                    break;
                case 2:
                    smoothFocusTarget = GameObject.Find("Player - 2").transform.position;
                    break;
                default:
                    break;
            }
        }
        else
        {
            smoothFocusTarget = transform.position;
        }

        zoomValue = Mathf.Clamp(zoomValue, minZoom, maxZoom);
        GetComponentInChildren<Camera>().orthographicSize = zoomValue;
        StartCoroutine(ZoomOutWhenStart(2.0f, 2.0f));

        shader_right = GameObject.FindGameObjectWithTag("Shader_Right").GetComponent<Image>();
        shader_left = GameObject.FindGameObjectWithTag("Shader_Left").GetComponent<Image>();
        shader_top = GameObject.FindGameObjectWithTag("Shader_Top").GetComponent<Image>();
        shader_bottom = GameObject.FindGameObjectWithTag("Shader_Bottom").GetComponent<Image>();
        shader_top.enabled = false;
        shader_bottom.enabled = false;
        shader_left.enabled = false;
        shader_right.enabled = false;

        // sets the cursor if not in tablet
        if (Input.touchSupported)
        {
            tablet_mode = true;
        }
    }

    IEnumerator ZoomOutWhenStart(float firstDelayed, float secondDelayed)
    {
        yield return new WaitForSeconds(firstDelayed);
        m_zoomingTime = Time.time + secondDelayed;

        yield return new WaitForSeconds(secondDelayed);

        m_frozeMouse = false;
    }

    void TouchUpdate()
    {
        if (Input.touchCount == 1)
        {
            //get the position of the click
            RaycastHit hitPosition;
            Ray ray = minimapCamera.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out hitPosition))
            {
                //move the camera to that position
                smoothMoveTo(hitPosition.point);
            }
        }


        if (Input.touchCount == 2)
        {
            Touch touchOne = Input.GetTouch(0);
            Touch touchTwo = Input.GetTouch(1);

            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            Vector2 touchTwoPrevPos = touchTwo.position - touchTwo.deltaPosition;
            float prevTouchDeltaMag = (touchOnePrevPos - touchTwoPrevPos).magnitude;
            float touchDeltaMag = (touchOne.position - touchTwo.position).magnitude;
            float deltaMagDiff = (prevTouchDeltaMag - touchDeltaMag) * 0.1f;
            Camera camera = GetComponentInChildren<Camera>();
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, camera.orthographicSize + deltaMagDiff / Time.deltaTime, Time.deltaTime);
            camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, minZoom, maxZoom);


        }


        if (Input.touchCount >= 2)
        {
            Touch oneTouch = Input.GetTouch(0);
            float fingerID = oneTouch.fingerId;
            if (oneTouch.phase == TouchPhase.Began)
            {
                fingerID = oneTouch.fingerId;
            }
            else if (oneTouch.phase == TouchPhase.Moved)
            {
                if (fingerID == oneTouch.fingerId)
                {
                    float speed = oneTouch.deltaPosition.magnitude / Time.deltaTime;
                    Vector2 dirV2 = oneTouch.deltaPosition;
                    Vector3 dirV3 = new Vector3(dirV2.x, transform.position.y, dirV2.y);
                    smoothMoveTo(transform.position + -dirV3.normalized * speed);
                }
            }
        }

        transform.position = Vector3.Lerp(transform.position, smoothFocusTarget, Time.deltaTime);


    }

    void MouseKeyboardUpdate()
    {
        float deltaTime = Time.deltaTime;

        if (mode == CameraMode.GameView)
        {
            Camera camera = GetComponentInChildren<Camera>();

            float camZoom = camera.orthographicSize;

            if (!m_frozeMouse)
            {
                //Scroll zooming
                zoomValue -= Input.mouseScrollDelta.y;
                zoomValue = Mathf.Clamp(zoomValue, minZoom, maxZoom);
                camZoom = Mathf.Lerp(camZoom, zoomValue, deltaTime * 10.0f);
            }
            else
            {
                if (m_zoomingTime > Time.time)
                {
                    zoomValue = Mathf.Lerp(zoomValue, maxZoom, deltaTime * 2.0f);
                    camZoom = zoomValue;
                }
            }

            camera.orthographicSize = camZoom;


            // smooth movement
            transform.position = Vector3.Lerp(transform.position, smoothFocusTarget, deltaTime * 2.5f);
            // scoller
            Vector3 viewPoint = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector3 nodePos = Vector3.zero;

            bool isScrolled = false;
            Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);

            //resets the cersor to normal if not in edge
            if (tablet_mode)
            {
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
            }
            else
                Cursor.SetCursor(cursor_Normal, Vector2.zero, cursorMode);
            shader_top.enabled = false;
            shader_bottom.enabled = false;
            shader_left.enabled = false;
            shader_right.enabled = false;

            if (screenRect.Contains(Input.mousePosition) && !minimapCamera.pixelRect.Contains(Input.mousePosition))
            {
                // to go up 
                if ((Input.GetKey(KeyCode.UpArrow) || viewPoint.y > 1.0f - scrollPercentage))
                {
                    nodePos.z = 1.0f;
                    isScrolled = true;
                    Cursor.SetCursor(cursor_Up, hotSpot, cursorMode);
                    shader_top.enabled = true;
                }

                // to go down
                if (Input.GetKey(KeyCode.DownArrow) || viewPoint.y < scrollPercentage)
                {
                    nodePos.z = -1.0f;
                    isScrolled = true;
                    Cursor.SetCursor(cursor_Down, hotSpot, cursorMode);
                    shader_bottom.enabled = true;
                }

                // to go left
                if (Input.GetKey(KeyCode.LeftArrow) || viewPoint.x < scrollPercentage)
                {
                    nodePos.x = -1.0f;
                    isScrolled = true;
                    Cursor.SetCursor(cursor_Left, hotSpot, cursorMode);
                    shader_left.enabled = true;
                }

                // to go right
                if ((Input.GetKey(KeyCode.RightArrow) || viewPoint.x > 1.0f - scrollPercentage) && !isOverUI)
                {
                    nodePos.x = 1.0f;
                    isScrolled = true;
                    Cursor.SetCursor(cursor_Right, hotSpot, cursorMode);
                    shader_right.enabled = true;
                }
            }




            Vector3 boundPos = transform.position + nodePos.normalized * deltaTime * scrollSpeed;

            if (boundPos.x < minX)
                boundPos.x = minX;
            if (boundPos.x > maxX)
                boundPos.x = maxX;
            if (boundPos.z < minY)
                boundPos.z = minY;
            if (boundPos.z > maxY)
                boundPos.z = maxY;

            transform.position = boundPos;
            if (isScrolled)
                smoothFocusTarget = transform.position;


        }
        else if (mode == CameraMode.FocusView)
        {
            //Camera camera = GetComponentInChildren<Camera>();
            //if (camera) {
            //    camera.transform.position = Vector3.Lerp(camera.transform.position, smoothTargetPosition, deltaTime * 2.5f);
            //    camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, smoothTargetRotation, deltaTime * 2.5f);
            //}
        }

        if (Input.GetMouseButtonDown(0)) // if the player clicks on the minimap
        {

            //get the position of the click
            RaycastHit hitPosition;
            Ray ray = minimapCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitPosition))
            {
                //move the camera to that position
                smoothMoveTo(hitPosition.point);

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.0f)
        {
            if (Input.touchSupported)
            {
                TouchUpdate();
            }
            else
            {

                MouseKeyboardUpdate();

            }
        }

    }

    void FixedUpdate()
    {
        //if (Input.GetMouseButtonDown(0)) // if the player clicks on the minimap
        //{
        //    allSelectableUnits = PlayerControls.GetSelectedUnits();
        //    foreach (GameObject item in allSelectableUnits)
        //    {
        //        item.gameObject.GetComponent<BaseCell>().isSelected = true;
        //    }
        //}
    }

    public void smoothMoveTo(Vector3 _des)
    {
        smoothFocusTarget = new Vector3(_des.x, 100, _des.z);
    }

    //public void SwitchToFocusView(Transform focusOn) {
    //    mode = CameraMode.FocusView;
    //    smoothTargetPosition = focusOn.position;
    //    smoothTargetRotation = focusOn.rotation;
    //}

    public void SwitchToGameplayView()
    {
        mode = CameraMode.GameView;
    }
}
