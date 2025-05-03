using UnityEngine;
using UnityEngine.SceneManagement;

//Main Menu Game Manager
public class MmGamemanager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PlayerPrefs.SetInt(MyUtils.ATTACKER_WIN_COUNT, 0);
        PlayerPrefs.SetInt(MyUtils.DEFENDER_WIN_COUNT, 0);
        PlayerPrefs.SetInt(MyUtils.MATCH_COUNT, 0);
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void Quit()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
