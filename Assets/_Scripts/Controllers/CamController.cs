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

    private float zoomValue = 0.0f;
    private float realtimeTimer;
    private Vector3 smoothFocusTarget;
    private Vector3 smoothTargetPosition;
    private Quaternion smoothTargetRotation;
    private CameraMode mode = CameraMode.GameView;


	// Use this for initialization
	void Start () {
        smoothTargetPosition = transform.position;
        smoothTargetRotation = transform.rotation;
        realtimeTimer = Time.realtimeSinceStartup;
	}
	
	// Update is called once per frame
	void Update () {
        
        float deltaTime = Time.realtimeSinceStartup - realtimeTimer;
        realtimeTimer = Time.realtimeSinceStartup;

        if (mode == CameraMode.GameView) {
            //Scroll zooming
            zoomValue -= Input.mouseScrollDelta.y;
            zoomValue = Mathf.Clamp(zoomValue, minZoom, maxZoom);

            Camera camera = GetComponentInChildren<Camera>();
            if (camera) {
                camera.orthographicSize = zoomValue;
            }

        
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
            Camera camera = GetComponentInChildren<Camera>();
            if (camera) {
                camera.transform.position = Vector3.Lerp(camera.transform.position, smoothTargetPosition, deltaTime * 2.5f);
                camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, smoothTargetRotation, deltaTime * 2.5f);
            }
        }
	}

    public void smoothMoveTo(Vector3 _des) {
        smoothFocusTarget = _des;
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
