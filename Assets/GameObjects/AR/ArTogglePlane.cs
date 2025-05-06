using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ArTogglePlane : MonoBehaviour
{
    private ARPlaneManager m_planeManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_planeManager = FindFirstObjectByType<ARPlaneManager>();

    }

    public void ToggleARPlaneVisibility()
    {
        m_planeManager.enabled = !m_planeManager.enabled;

        if (m_planeManager.enabled)
        {
            SetAllPlanesActive(true);
        }
        else
        {
            SetAllPlanesActive(false);
        }

    }

    void SetAllPlanesActive(bool value)
    {
        foreach (var plane in m_planeManager.trackables)
            plane.gameObject.SetActive(value);
    }
    // Update is called once per frame
    void Update()
    {
    }
}
