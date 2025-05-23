using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ArRayCast : MonoBehaviour
{
    const float k_PrefabHalfSize = 0.025f;

    private Camera m_camera;
    private ARRaycastManager m_raycastManager;
    private ARPlaneManager m_planeManager;
    [SerializeField]private GameObject m_prefabToPlace;
    private float m_scalefactor = 1.0f;
    private Vector3 m_initialScale;
    private float m_initialDist;

    private GameObject m_spawnObject;
    private void Awake()
    {
        m_camera = Camera.main;// FindFirstObjectByType<Camera>();
        m_spawnObject = null;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_raycastManager = FindFirstObjectByType<ARRaycastManager>();
        m_planeManager = FindFirstObjectByType<ARPlaneManager>();
        m_spawnObject = GameObject.FindGameObjectWithTag("ArenaRoot");
    }

    void scale(float val)
    {
        var scale = m_spawnObject.transform.localScale = m_initialScale * val * m_scalefactor;
        m_spawnObject.transform.localScale = new Vector3(
            Mathf.Clamp(scale.x, 0.01f, 1.5f),
            Mathf.Clamp(scale.y, 0.01f, 1.5f),
            Mathf.Clamp(scale.z, 0.01f, 1.5f));
    }
    // Update is called once per frame
    void Update()
    {
        //scale with pinch zoom logic
        if (m_spawnObject != null)
        {
#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            if (touchZero.phase == TouchPhase.Ended ||
                touchZero.phase == TouchPhase.Canceled ||
                touchOne.phase == TouchPhase.Ended ||
                touchOne.phase == TouchPhase.Canceled)
            {
                return;
            }
            if (touchZero.phase == TouchPhase.Began ||
                touchOne.phase == TouchPhase.Began)
            {
                m_initialDist = Vector2.Distance(touchZero.position, touchOne.position);
                m_initialScale = m_spawnObject.transform.localScale;
            }
            else //touch moved
            {
                var curDistance = Vector2.Distance(touchZero.position, touchOne.position);

                if(Mathf.Approximately(m_initialDist,0))
                {
                    return ;
                }
                    
                float factor = curDistance / m_initialDist;
                
                scale(factor);
            }
        }
#endif
        }
        if (EventSystem.current == null) 
            return;
        if (Input.GetMouseButtonDown(0))
        { 
            // Debug.Log("Placement Method =>  Meshing");
            TouchToRayPlaneDetection(Input.mousePosition);
        }
#if UNITY_IOS || UNITY_ANDROID

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Touch touch = Input.GetTouch(0);
                
                PointerEventData pointerData = new PointerEventData(EventSystem.current);
                pointerData.position = touch.position;

                List<RaycastResult> results = new List<RaycastResult>();

                EventSystem.current.RaycastAll(pointerData, results);

                if (results.Count > 0) {
                    // We hit a UI element
                    Debug.Log("We hit an UI Element");
                    return;
                }


            TouchToRayPlaneDetection(touch.position);
               
            }
#endif
    }


    //Shoot ray against AR planes
    void TouchToRayPlaneDetection(Vector3 touch)
    {
        Ray ray = m_camera.ScreenPointToRay(touch);
        List<ARRaycastHit> hits = new();

        m_raycastManager.Raycast(touch, hits, TrackableType.Planes);
        if (hits.Count > 0)
        {
            if(m_spawnObject == null)
            {
                m_spawnObject = Instantiate(m_prefabToPlace);
                Debug.Log("m_spawn is null");
                //m_spawnObject.transform.localScale = m_spawnObject.transform.lossyScale.Multiply(new Vector3(m_scale, m_scale, m_scale));
            }
            if(hits[0].trackable.gameObject.activeInHierarchy)
            {
                //m_spawnObject.SetActive(true);
                var forward = hits[0].pose.rotation * Vector3.up;
                var offset = forward * k_PrefabHalfSize;
                m_spawnObject.transform.position = hits[0].pose.position + offset;
            }
        }
    }
}
