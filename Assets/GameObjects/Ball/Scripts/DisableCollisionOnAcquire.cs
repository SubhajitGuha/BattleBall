using UnityEngine;

public class DisableCollisionOnAcquire : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if the ball is not owned by any attacker then enable the attacker
        if(gameObject.transform.parent == null)
        {
            gameObject.GetComponent<Collider>().enabled = true;
        }
        else if(gameObject.transform.parent.GetComponent<AttackerStateManager>() != null)
        {
            gameObject.GetComponent<Collider>().enabled = false;
        }
    }
}
