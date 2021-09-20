using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleScript : MonoBehaviour
{
    public bool redPaddle, leftPaddle, topPaddle;

    float paddleHeight, scissorInput;

    float paddleRange = 3.65f;
    float scissorMin = 0;
    float scissorMax = 1023;

    public Color red, blue;
    bool closedScissors = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D collider = this.gameObject.GetComponent<BoxCollider2D>();

        // Determines whether this paddle belongs to Player 1 or 2
        if (leftPaddle) scissorInput = BladeScript.playerInput1;
        else scissorInput = BladeScript.playerInput2;

        // Determines whether the ball is the right color to hit the paddle
        if (BallScript.redBall) {
            if (redPaddle) collider.isTrigger = false;
            else collider.isTrigger = true;
        } else {
            if (redPaddle) collider.isTrigger = true;
            else collider.isTrigger = false;
        }

        // Keyboard controls for paddles
        if (Input.GetKey(KeyCode.UpArrow)) scissorInput += 7f;
        if (Input.GetKey(KeyCode.DownArrow)) scissorInput -= 7f;

        // Checks the scissor input values are within the proper range
        if (scissorInput > scissorMax) scissorInput = scissorMax;
        if (scissorInput < scissorMin) scissorInput = scissorMin;

        // Remaps input values to the range of paddle movement
        paddleHeight = Remap(scissorInput, scissorMin, scissorMax, 0, paddleRange);

        // Checks to see if paddles have reached the middle
        float middleMargin = paddleRange / 10;
        if (paddleHeight < middleMargin && paddleHeight > -middleMargin) {
            if (!closedScissors) {
                reColor();
                closedScissors = true;
            }
        } else {
            closedScissors = false;
        }

        // Moves the paddles
        Vector2 pos;
        if (leftPaddle) {
            if (topPaddle) pos = new Vector2(this.transform.position.x, paddleHeight);
            else pos = new Vector2(this.transform.position.x, -paddleHeight);
        } else {
            if (topPaddle) pos = new Vector2(this.transform.position.x, paddleHeight);
            else pos = new Vector2(this.transform.position.x, -paddleHeight);
        }
        transform.position = pos;
    }

    void reColor() {
        if (redPaddle) {
            this.gameObject.GetComponent<SpriteRenderer>().color = blue;
            redPaddle = false;
        } else {
            this.gameObject.GetComponent<SpriteRenderer>().color = red;
            redPaddle = true;
        }
    }

    public static float Remap (float z, float x1, float x2, float y1, float y2) {
        var m = (y2 - y1) / (x2 - x1);
        var c = y1 - m * x1;
        
        return m * z + c;
    }
}
