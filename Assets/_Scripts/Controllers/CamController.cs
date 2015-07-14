using UnityEngine;
using System.Collections;

public class CamController : MonoBehaviour {

    public enum CameraMode {
        GameView,
        FocusView,
    };

    public float minX , maxX, minY, maxY;
    public float scrollSpeed;
    public float scrollPercentage;
    public float minZoom;
    public float maxZoom;
    public GameObject quad;
    public GameObject mainCamera;
    public Camera minimapCamera;

    public float zoomValue;
//    private float realtimeTimer;
    private Vector3 smoothFocusTarget;
    private Vector3 smoothTargetPosition;
    private Quaternion smoothTargetRotation;
    private CameraMode mode = CameraMode.GameView;
    
    // Froze mouse when start
    private bool m_frozeMouse = true;
    private float m_zoomingTime;
	// Use this for initialization
	void Start () {
        smoothFocusTarget = transform.position;
        zoomValue = Mathf.Clamp(zoomValue, minZoom, maxZoom);
        GetComponentInChildren<Camera>().orthographicSize = zoomValue;
        StartCoroutine(ZoomOutWhenStart(2.0f, 2.0f));
	}

    IEnumerator ZoomOutWhenStart(float firstDelayed, float secondDelayed) {
        yield return new WaitForSeconds(firstDelayed);
        m_zoomingTime = Time.time + secondDelayed;

        yield return new WaitForSeconds(secondDelayed);

        m_frozeMouse = false;
    } 
	
	// Update is called once per frame
	void Update () {

        float deltaTime = Time.deltaTime;

        if (mode == CameraMode.GameView) {
            Camera camera = GetComponentInChildren<Camera>();

            float camZoom = camera.orthographicSize;

            if (!m_frozeMouse) {
                //Scroll zooming
                zoomValue -= Input.mouseScrollDelta.y;
                zoomValue = Mathf.Clamp(zoomValue, minZoom, maxZoom);
                camZoom = Mathf.Lerp(camZoom, zoomValue, deltaTime * 10.0f);
            } else {
                if (m_zoomingTime > Time.time) {
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
            if (screenRect.Contains(Input.mousePosition) /*&& !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()*/)
            {
                // to go up 
                if (Input.GetKey(KeyCode.UpArrow) || viewPoint.y > 1.0f - scrollPercentage)
                {
                    nodePos.z = 1.0f;
                    isScrolled = true;
                }
                // to go down
                if (Input.GetKey(KeyCode.DownArrow) || viewPoint.y < scrollPercentage)
                {
                    nodePos.z = -1.0f;
                    isScrolled = true;
                }
                // to go left
                if (Input.GetKey(KeyCode.LeftArrow) || viewPoint.x < scrollPercentage)
                {
                    nodePos.x = -1.0f;
                    isScrolled = true;
                }
                // to go right
                if (Input.GetKey(KeyCode.RightArrow) || viewPoint.x > 1.0f - scrollPercentage)
                {
                    nodePos.x = 1.0f;
                    isScrolled = true;
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
            

        } else if (mode == CameraMode.FocusView) {
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

    public void smoothMoveTo(Vector3 _des) {
        smoothFocusTarget = new Vector3(_des.x, 100, _des.z);
    }

    public void SwitchToFocusView(Transform focusOn) {
        mode = CameraMode.FocusView;
        smoothTargetPosition = focusOn.position;
        smoothTargetRotation = focusOn.rotation;
    }

    public void SwitchToGameplayView() {
        mode = CameraMode.GameView;
    }
}
