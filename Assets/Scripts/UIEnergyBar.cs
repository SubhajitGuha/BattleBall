using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIEnergyBar : MonoBehaviour
{
    private Slider m_progress;

    public void Activate(float normalizedEnergyAmount)
    {
        m_progress.value = normalizedEnergyAmount; //fill the progress bar
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_progress = GetComponent<Slider>();
        if(m_progress == null )
        {
            Debug.LogError( string.Format("Add a slider component to {}", gameObject.ToString()));
        }
        m_progress.value = 0; //at game start energy is 0.0
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
