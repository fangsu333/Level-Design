using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    //bdp: reference to the player (object that is kept on the centre of the screen, the one the camera is focused on
    public GameObject player;

    //bdp: the offset is the distance between the camera and the focused object (player), so we keep following it at a constant distance
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        //bdp: the scene is set by hand (I went into Unity and positioned the player and the camera by hand, making it as I wanted to look like)
        //bpd: in this first frame I just calculate the offset, so we keep the same distance and angle between camera and player all the time
        offset = transform.position - player.transform.position;
    }

    // LateUpdate, like Update, is called once per frame, but after Update happened
    void LateUpdate()
    {
        //bdp: here we update the position of the camera taking into account the position of the player AND the offset calculated earlier
        //bdp: this means the camera will follow the player all the time
        transform.position = player.transform.position + offset;
    }

    //bdp: this works "ok" for a basic camera for a prototype. If this were a "proper" game, we would probably need to worry about culling the camera
    //bdp: so it does not "cross" other objects or walls from the environment. But this is a problem for the "final" version of the game, now we
    //bdp: are more worried with gameplay and fun...
}
