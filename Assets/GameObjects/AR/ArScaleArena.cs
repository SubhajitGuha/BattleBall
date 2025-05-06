using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class ArScaleArena : MonoBehaviour
{

    private Camera m_mainCam;
    private GameObject m_selectedObject;
    [SerializeField]private GameObject m_Arena;
    private float m_scalefactor = 1.0f;
    private Vector3 m_initialScale;
    //Shoot ray from the touch position 
    void TouchToRayCasting(Vector3 touch)
    {
        Ray ray = m_mainCam.ScreenPointToRay(touch);
        Debug.DrawRay(ray.origin, ray.direction);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100000.0f))
        {
            m_selectedObject = hit.collider.gameObject;
            m_selectedObject = m_selectedObject.transform.root.gameObject; //get the root of the hit object
            //Debug.Log(m_selectedObject.transform.root.ToString());
        }
        //Else, deselect all objects
        else
        {
            m_selectedObject = null;
        }
    }

    private void Awake()
    {
        m_mainCam = Camera.main;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(m_Arena == null)
        {
            Debug.Log(string.Format("attach arena to {0}", gameObject.ToString()));
        }
    }

    void scale(float val)
    {
        var scale = m_selectedObject.transform.localScale = m_initialScale * val * m_scalefactor;
        m_selectedObject.transform.localScale = new Vector3(
            Mathf.Clamp(scale.x, 0.01f, 1.5f),
            Mathf.Clamp(scale.y, 0.01f, 1.5f),
            Mathf.Clamp(scale.z, 0.01f, 1.5f));
    }
    // Update is called once per frame
    void Update()
    {

#if UNITY_EDITOR
        //If not manipulating objects transform
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("UI Hit was recognized");
                return;
            }

            TouchToRayCasting(Input.mousePosition);
        }


#endif
#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount > 0 && Input.touchCount < 2 &&
                Input.GetTouch(0).phase == TouchPhase.Began)
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
                TouchToRayCasting(touch.position);
            }

#endif
        if(m_selectedObject == null)
            return;

        if (m_Arena.tag != m_selectedObject.tag) 
            return;
        
#if UNITY_IOS || UNITY_ANDROID
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            if(touchOne.phase == TouchPhase.Began || touchZero.phase == TouchPhase.Began)
            {
                m_initialScale = m_selectedObject.transform.lossyScale;
            }
            else //touch moved
            {
                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                float curMag = (touchZero.position - touchOne.position).magnitude;

                float factor = curMag / prevMag;
                if (factor < 0.01f)
                    return;
                scale(factor);
            }
        }
#endif
    }

}
