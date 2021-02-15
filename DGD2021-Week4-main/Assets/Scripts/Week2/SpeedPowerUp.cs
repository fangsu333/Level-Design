using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    //the factor that will be used to modify player speed
    public float speedModifier;

    //the duration of the event (see PlayerBehaviour to check how it works) 
    public float duration;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        //if the player collides with this object we call SpeedUp function passing the factor and how long it will last
        //and destroy this gameObject
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerBehaviour>().SpeedUp(speedModifier, duration);

            Destroy(this.gameObject);
        }
    }
}
