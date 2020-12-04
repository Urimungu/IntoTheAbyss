using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement{

    /// <summary>
    /// Moves the palyer
    /// </summary>
    /// <param name="rb">Rigidbody attached to the player.</param>
    /// <param name="hor">Horizontal input.</param>
    /// <param name="ver">Vertical Input</param>
    /// <param name="speed">The velocity of movement</param>
    public static void MovePlayer(Rigidbody rb, float hor, float ver, float speed){
        //Calculates the movement
        var newVel = ((rb.transform.forward * ver) + (rb.transform.right * hor)).normalized * speed;
        newVel.y = rb.velocity.y;

        //Sets movement into the passed Rigidbody
        rb.velocity = newVel;
    }

}
