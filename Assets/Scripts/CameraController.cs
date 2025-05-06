using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float m_minOrthographicSize = 20.0f;
    [SerializeField] private float m_maxOrthographicSize = 25.0f;
    [SerializeField] private float m_zoomSpeed = 2.0f;

    private Camera m_Camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_Camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Camera.orthographic)
        {
            if (Input.mouseScrollDelta.x != 0.0 || Input.mouseScrollDelta.y != 0.0)
            {
                m_Camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * m_zoomSpeed;
                m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize, m_minOrthographicSize, m_maxOrthographicSize);
            }
            //for touch input
            if(Input.touchCount == 2)
            {
                Touch touchZero = Input.GetTouch(0);
                Touch touchOne = Input.GetTouch(1);

                Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                float prevMag = (touchZeroPrevPos -  touchOnePrevPos).magnitude;
                float curMag = (touchZero.position - touchOne.position).magnitude;

                float diff = curMag - prevMag;

                m_Camera.orthographicSize -= diff * m_zoomSpeed;
            }
        }
        //else
        //{
        //    m_Camera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * m_zoomSpeed;
        //}
    }
}
