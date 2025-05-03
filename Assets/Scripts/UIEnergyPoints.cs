using UnityEngine;
using System.Collections.Generic;

//this class creats "numEnergyPoints" number of ui images which will act as energy blocks
public class UIEnergyPoints : MonoBehaviour
{
    [SerializeField] private GameObject m_energyPointImage;
    private List<GameObject> m_uiObjects = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        int numEnergyPoints = GameManager.GetNumEnergyPoints();
        RectTransform rectTransform = m_energyPointImage.GetComponent<RectTransform>();
        Vector3 pos = rectTransform.position;
        Rect rect = rectTransform.rect;
        rect.width = rect.width / numEnergyPoints;
        rectTransform.sizeDelta = new Vector2 (rect.width, rect.height);
        Vector2 size = rectTransform.sizeDelta / 2.0f;

        //create n copies of "m_energyPointImage" this will act as energy point block
        for (int i=1;i<numEnergyPoints;i++)
        {
            GameObject newObj = Instantiate(m_energyPointImage, new Vector3(
                pos.x + rect.width,
                pos.y,
                pos.z
                ), Quaternion.identity, transform);
            pos = newObj.transform.position;
        }
        //rectTransform.transform.position = 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
