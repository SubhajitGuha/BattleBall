using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    [SerializeField] private TMP_Text m_whoWinsText;
    [SerializeField] private Button m_mainMenuButton;
    [SerializeField] private Button m_playTieBreakerButton;

    public void Activate(string winnerName)
    {
        gameObject.SetActive(true);
        m_whoWinsText.text = winnerName + " WINS";
        if(winnerName == "ATTACKER")
            m_whoWinsText.color = Color.white;
        else
            m_whoWinsText.color= Color.red;

        m_mainMenuButton.gameObject.SetActive(true);
    }
    public void Activate()
    {
        gameObject.SetActive(true);
        m_whoWinsText.text = "DRAW";
        if(m_playTieBreakerButton == null)
        {
            return;
        }
        m_playTieBreakerButton.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
