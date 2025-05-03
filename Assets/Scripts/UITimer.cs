using TMPro;
using UnityEngine;

public class UITimer : MonoBehaviour
{
    [SerializeField] private TMP_Text m_timerText;

    public void Activate(float remainingTime)
    {
        int minutes = Mathf.Max(Mathf.FloorToInt(remainingTime / 60), 0);
        int seconds = Mathf.Max(Mathf.FloorToInt(remainingTime % 60), 0);

        m_timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
