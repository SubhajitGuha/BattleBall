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
            m_Camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * m_zoomSpeed;
            m_Camera.orthographicSize = Mathf.Clamp(m_Camera.orthographicSize, m_minOrthographicSize, m_maxOrthographicSize);
        }
        else
        {
            m_Camera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * m_zoomSpeed;
        }
    }
}
