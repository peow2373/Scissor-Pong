using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallScript : MonoBehaviour
{
    float minVelocity = 2.5f;
    float maxVelocity = 5;

    public static bool redBall = false;
    public Color red, blue;

    public GameObject outline;
    private SpriteRenderer sr;

    public static int p1Score, p2Score;
    public Text score;

    public AudioSource bounce, metalBounce, scoredPoint;

    float speedUp = 1.5f;
    float slowDown = 0.9f;
    float maxSpeed = 10f;

    public static bool roundOver = true;
    bool ballResetting = false;

    // Start is called before the first frame update
    void Start()
    {
        sr = this.gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine("Launch");
    }

    // Update is called once per frame
    void Update()
    {
        // Makes the outline disappear and reappear with the ball
        if (!sr.enabled) outline.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        else outline.gameObject.GetComponent<SpriteRenderer>().enabled = true;

        // Checks to make sure the ball isn't going too fast
        Vector2 currentVelocity = this.gameObject.GetComponent<Rigidbody2D>().velocity;

        if (currentVelocity.x >= maxSpeed) this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(currentVelocity.x * slowDown, currentVelocity.y);
        else if (currentVelocity.y >= maxSpeed) this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(currentVelocity.x, currentVelocity.y * slowDown);

        // Update player scores
        score.text = p1Score.ToString() + "   " + p2Score.ToString();
    }

    void OnCollisionEnter2D(Collision2D other) {
        if (!roundOver) {
            // Is the wall the ball hit Red?
            if (other.collider.tag == "Red") {
                this.gameObject.GetComponent<SpriteRenderer>().color = red;
                redBall = true;

                bounce.pitch = Random.Range(0.95f,1.15f);
                bounce.Play();
            }

            // Is the wall the ball hit Blue?
            if (other.collider.tag == "Blue") {
                this.gameObject.GetComponent<SpriteRenderer>().color = blue;
                redBall = false;

                bounce.pitch = Random.Range(0.95f,1.15f);
                bounce.Play();
            }

            if (other.collider.tag == "Scissors") {
                Vector2 currentVelocity = this.gameObject.GetComponent<Rigidbody2D>().velocity;

                // Increase the speed of the ball
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(currentVelocity.x * speedUp, currentVelocity.y * speedUp);

                metalBounce.pitch = Random.Range(0.95f,1.15f);
                metalBounce.Play();
            }

        }

        // Did the ball go out of bounds?
        if (other.collider.tag == "Out of Bounds") {
            sr.enabled = false;
            if (!ballResetting) {
                StartCoroutine("Launch");
                ballResetting = true;
            }
        }


        Vector2 velocityCheck = this.gameObject.GetComponent<Rigidbody2D>().velocity;

        // Ensures the ball is going above the minimum Y speed
        if (velocityCheck.y >= 0) {
            if (velocityCheck.y < minVelocity) {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityCheck.x, minVelocity);
            }
        } else {
            if (velocityCheck.y > -minVelocity) {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityCheck.x, -minVelocity);
            }
        }

        // Ensures the ball is going above the minimum X speed
        if (velocityCheck.x >= 0) {
            if (velocityCheck.x < minVelocity) {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(minVelocity, velocityCheck.y);
            }
        } else {
            if (velocityCheck.x > -minVelocity) {
                this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-minVelocity, velocityCheck.y);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!roundOver) {
            // Did the ball hit a player's goal?
            if (other.GetComponent<BoxCollider2D>().tag == "P1 Goal") p2Score++;
            else if (other.GetComponent<BoxCollider2D>().tag == "P2 Goal") p1Score++;

            if (other.GetComponent<BoxCollider2D>().tag == "P1 Goal" || other.GetComponent<BoxCollider2D>().tag == "P2 Goal") {
                scoredPoint.pitch = Random.Range(0.95f,1.25f);
                scoredPoint.Play();

                roundOver = true;
                
                //StartCoroutine("Evaporate");
            }
        }
    }

    IEnumerator Launch() {
        sr.enabled = false;

        yield return new WaitForSeconds(1);

        roundOver = false;
        ballResetting = false;

        // Reset the ball
        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        this.transform.position = new Vector3(0,0,0);

        float velocityX = Random.Range(minVelocity, maxVelocity);
        float velocityY = Random.Range(minVelocity, maxVelocity);

        // Determines in which direction the ball launches
        float xDirection = Random.Range(-1,1);
        float yDirection = Random.Range(-1,1);
        if (xDirection < 0) velocityX *= -1;
        if (yDirection < 0) velocityY *= -1;

        // Determines which color the ball starts as
        float color = Random.Range(-1,1);
        if (color < 0) {
            sr.color = red;
            redBall = true;
        } else {
            sr.color = blue;
            redBall = false;
        }

        sr.enabled = true;
        yield return new WaitForSeconds(0.75f);

        this.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(velocityX, velocityY);
    }

    IEnumerator Evaporate() {
        yield return new WaitForSeconds(2);

        if (roundOver != false) {
            for (int i = 0; i < 25; i++) {
                float scaleFactor = transform.localScale.x / 25 * (25 - i);
                transform.localScale = new Vector3 (scaleFactor, scaleFactor, 1);
                yield return new WaitForSeconds(0.25f);
            }

            StartCoroutine("Launch");
        }
    }
}
