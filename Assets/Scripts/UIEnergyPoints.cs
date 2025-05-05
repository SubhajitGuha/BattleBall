using UnityEngine;
using System.Collections.Generic;

//this class creats "numEnergyPoints" number of ui images which will act as energy blocks
public class UIEnergyPoints : MonoBehaviour
{
    [SerializeField] private GameObject m_energyPointImage; //Ui Prefab that will be instanciated multiple times
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int numEnergyPoints = GameManager.GetNumEnergyPoints();
        RectTransform rectTransform = m_energyPointImage.GetComponent<RectTransform>();
        float anchorPos = 0.0f;
        Rect rect = rectTransform.rect;
        rect.width = rect.width / numEnergyPoints;
        
        //create n copies of "m_energyPointImage" this will act as energy point block
        for (int i=0;i<numEnergyPoints;i++)
        {
            GameObject newObj = Instantiate(m_energyPointImage);
            newObj.transform.SetParent(transform, false);

            RectTransform newRectTransform = newObj.GetComponent<RectTransform>();
            newRectTransform.sizeDelta = new Vector2(rect.width, rect.height);
            newRectTransform.anchoredPosition = new Vector2(anchorPos, 0.0f); //modify the anchor position
            anchorPos += rect.width;
        }
        //rectTransform.transform.position = 
    }
    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
