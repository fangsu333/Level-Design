using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    //reference to that component within GameController object
    private GameStateController gameController;

    //reference to the SimpleController object within this gameObject
    private SimpleController controls;

    //how long the player will be invincible for after losing a life
    public float invincibilityTime;

    // Start is called before the first frame update
    void Start()
    {
        //saves the reference to the gameController...
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameStateController>();

        //...and to SimpleControls
        controls = GetComponent<SimpleController>();
        
    }


    


    // Update is called once per frame
    void Update()
    {
        
        
    }


    //You have seen coroutines with Abel

    //https://docs.unity3d.com/Manual/Coroutines.html
    /// <summary>
    /// Works as a timer according to the duration parameter passed onto it
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    IEnumerator Invincibility(float duration)
    {

        //See "GameStateController" -> but it simply deactivates the colliders from objects under AllObstacles        
        gameController.MakeInvincible();

        //works effectively as a counter, every frame reducing the elapsed time from duration until duration is over
        while (duration > 0)
        {
            duration -= Time.deltaTime;

            yield return null;
        }

        //See "GameStateController" -> but it simply activates the colliders from objects under AllObstacles
        gameController.MakeVulnerable();
    }



    IEnumerator SpeedPowerUp(float ns, float d)
    {
        //saves the movePower used before the effect is applied
        float originalMovePower = controls.m_MovePower;

        //applies the temporary factor
        controls.m_MovePower *= ns;

        //works effectively as a counter, every frame reducing the elapsed time from duration until duration is over
        while (d > 0)
        {
            d -= Time.deltaTime;

            yield return null;
        }

        //restores movePower to the previous value
        controls.m_MovePower = originalMovePower;

    }


    IEnumerator InvertHAxis(float d)
    {
        //inverts the movement axis
        controls.invertMovement = !controls.invertMovement;

        //works effectively as a counter, every frame reducing the elapsed time from duration until duration is over
        while (d > 0)
        {
            d -= Time.deltaTime;

            yield return null;
        }

        //inverts the movement axis again (restoring it)
        controls.invertMovement = !controls.invertMovement;
    }

    //works with the coroutine above the keep the duration working    
    public void InvertMovement(float duration)
    {
        
        StartCoroutine(InvertHAxis(duration));
    }


    //works with the coroutine above the keep the duration working
    public void SpeedUp(float newSpeed, float duration)
    {
        StartCoroutine(SpeedPowerUp(newSpeed, duration));
    }


    private void OnCollisionEnter(Collision collision)
    {
        //if the player collides with anything that is not the floor
        if (collision.gameObject.tag != "Floor")
        {
            //we remove a life
            gameController.UpdateLives(-1);

            //and we make it invincible for some time (set as the variable invincibilityTime, so if it is 2f,it stays 2secs invincible)
            StartCoroutine("Invincibility", invincibilityTime);
        }        
        
                
    }

    
}
