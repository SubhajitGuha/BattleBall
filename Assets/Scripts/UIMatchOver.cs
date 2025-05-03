using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIMatchOver : MonoBehaviour
{
    [SerializeField]private TMP_Text m_whoWinsText;
    [SerializeField]private TMP_Text m_attackerWinCount;
    [SerializeField] private TMP_Text m_defenderWinCount;

    public void Activate(string winnerName, int attackerWinCount, int defenderWinCount)
    {
        gameObject.SetActive(true);
        m_whoWinsText.text = winnerName + " WINS";
        m_attackerWinCount.text = "ATTACKER SCORE : " + attackerWinCount.ToString();
        m_defenderWinCount.text = "DEFENDER SCORE : " + defenderWinCount.ToString();
    }
    public void Activate(int attackerWinCount, int defenderWinCount)
    {
        gameObject.SetActive(true);
        m_whoWinsText.text = "DRAW";
        m_attackerWinCount.text = "ATTACKER SCORE : " + attackerWinCount.ToString();
        m_defenderWinCount.text = "DEFENDER SCORE : " + defenderWinCount.ToString();
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
