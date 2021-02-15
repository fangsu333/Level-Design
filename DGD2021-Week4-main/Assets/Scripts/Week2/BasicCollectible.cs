using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCollectible : MonoBehaviour
{
    /// <summary>
    /// holds the number of points added to the score
    /// 
    /// the only method within it is OnTriggerEnter, and if it collides with the Player we update the score
    /// </summary>
    public int points;

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
        //if the player hits this trigger
        if (other.tag == "Player")
        {
            //we find the object that has the GameStateController and Update the score according to the number of points set in this object
            GameObject.FindObjectOfType<GameStateController>().UpdateScore(points);

            //and delete this gameObject
            Destroy(this.gameObject);
        }
    }

}
