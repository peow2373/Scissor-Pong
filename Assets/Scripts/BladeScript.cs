using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BladeScript : MonoBehaviour
{
    public bool leftScissors, topBlade, redBlade;

    public static float playerInput1, playerInput2;

    float inputMin;
    float inputMax;

    float scissorAngle, scissorInput;

    float scissorMin = -3;
    float scissorMax = 50;

    float p1Min = 125;
    float p1Max = 300;

    float p2Min = 0;
    float p2Max = 1023;

    public Color red, blue;
    bool closedScissors = true;

    public AudioSource leftSnip, rightSnip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D collider = this.gameObject.GetComponentInChildren<PolygonCollider2D>();

        // Determines whether this paddle belongs to Player 1 or 2
        if (leftScissors) {
            scissorInput = playerInput1;
            inputMin = p1Min;
            inputMax = p1Max;
        } else {
            scissorInput = playerInput2;
            inputMin = p2Min;
            inputMax = p2Max;
        }

        // Determines whether the ball is the right color to hit the paddle
        if (BallScript.redBall) {
            if (redBlade) collider.isTrigger = false;
            else collider.isTrigger = true;
        } else {
            if (redBlade) collider.isTrigger = true;
            else collider.isTrigger = false;
        }

        // Keyboard controls for paddles
        if (Input.GetKey(KeyCode.UpArrow)) scissorInput += 10f;
        if (Input.GetKey(KeyCode.DownArrow)) scissorInput -= 10f;

        // Checks the scissor input values are within the proper range
        if (scissorInput > inputMax) scissorInput = inputMax;
        if (scissorInput < inputMin) scissorInput = inputMin;

        
        // Remaps input values to the range of paddle movement
        scissorAngle = Remap(scissorInput, inputMin, inputMax, scissorMin, scissorMax);

        // Checks to see if paddles have reached the middle
        if (scissorAngle <= 0) {
            if (!closedScissors) {
                reColor();
                closedScissors = true;

                if (leftScissors) leftSnip.Play();
                else rightSnip.Play();
            }
        } else {
            closedScissors = false;
        }

        // Moves the paddles
        Vector3 rotation = transform.rotation.eulerAngles;
        if (topBlade) rotation.z = scissorAngle;
        else rotation.z = -scissorAngle;
        transform.rotation = Quaternion.Euler(rotation);
    }

    void reColor() {
        SpriteRenderer[] sprites = this.gameObject.GetComponentsInChildren<SpriteRenderer>();

        if (redBlade) {
            foreach(SpriteRenderer sprite in sprites) sprite.color = blue;
            redBlade = false;
        } else {
            foreach(SpriteRenderer sprite in sprites) sprite.color = red;
            redBlade = true;
        }
    }

    public static float Remap (float z, float x1, float x2, float y1, float y2) {
        var m = (y2 - y1) / (x2 - x1);
        var c = y1 - m * x1;
        
        return m * z + c;
    }
}
