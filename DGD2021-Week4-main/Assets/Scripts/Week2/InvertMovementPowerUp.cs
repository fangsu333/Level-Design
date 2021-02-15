using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertMovementPowerUp : MonoBehaviour
{

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
        if (other.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerBehaviour>().InvertMovement(duration);

            Destroy(this.gameObject);
        }
    }

}
