using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class BallScript : MonoBehaviour
{
    public static BallScript instance;

    public float Speed = 5f; // You can adjust this value in the Unity editor to control the Speed of the ball
    public float DirSpeed = 20f;

    private Rigidbody2D rb;
    Vector2 direction;
    public GameObject circleRadius;
    public float cloneCooldown = 1f; // Cooldown in seconds
    public float nextCloneTime = 0f; // When the next clone can be created
    public TMP_Text ballCount;
    public TMP_Text moneyCount;
    public TMP_Text costOne;
    public TMP_Text costTwo;
    public TMP_Text costThree;
    public TMP_Text countOne;
    public TMP_Text countTwo;
    public TMP_Text countThree;
    static int upgradeOneCost = 50;
    static int upgradeOneCount = 0;
    static int upgradeTwoCost = 250;
    static int upgradeTwoCount = 0;
    static int upgradeThreeCost = 1000;
    static int upgradeThreeCount = 0;
    static int balls = 1;
    static int money = 0;
    public int maxBalls = 10;
    static int moneyGain = 1;
    static float radius = 0.5f;

    public bool isImmune = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();


        Vector2 direction = new Vector2(UnityEngine.Random.Range(-5f, 5f), UnityEngine.Random.Range(-5f, 5f)).normalized;
        rb.AddForce(direction * DirSpeed, ForceMode2D.Impulse);

    }

    IEnumerator StartImmunity()
    {
        isImmune = true;
        yield return new WaitForSeconds(0.2f);
        isImmune = false;
    }

    void SpawnBall()
    {
        Vector2 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, Camera.main.nearClipPlane));

        GameObject newBall = Instantiate(gameObject, spawnPosition, transform.rotation);
        newBall.name = gameObject.name;
        newBall.GetComponent<BallScript>().StartCoroutine("StartImmunity");
        nextCloneTime = Time.time + cloneCooldown; // Set the next clone time

        // Change the color of the new ball
        SpriteRenderer sr = newBall.GetComponent<SpriteRenderer>();
        sr.color = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value);

        // Enable BallScript and CircleCollider2D
        BallScript ballScript = newBall.GetComponent<BallScript>();
        CircleCollider2D circleCollider = newBall.GetComponent<CircleCollider2D>();
        if (ballScript != null)
        {
            ballScript.enabled = true;
        }
        if (circleCollider != null)
        {
            circleCollider.enabled = true;
        }

        balls += 1;
        ballCount.text = "Balls: " + balls.ToString();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("wall"))
        {
            direction.y = -direction.y;
            direction.x = -direction.x;


            if (Time.time >= nextCloneTime && balls < maxBalls)
            {
                SpawnBall();

            }
        }
    }

        public void MoneyUpgrade()
    {
        if (money >= upgradeOneCost)
        {
            money -= upgradeOneCost;
            upgradeOneCount += 1;
            upgradeOneCost = (int)(50 * Math.Pow(1.30, upgradeOneCount));
            moneyGain += 1;
            costOne.text = "$" + upgradeOneCost;
            moneyCount.text = "$" + money.ToString();
            countOne.text = upgradeOneCount.ToString();
            
        }
    }

    public void RadiusUpgrade()
    {
        if (money >= upgradeTwoCost)
        {
            money -= upgradeTwoCost;
            upgradeTwoCount += 1;
            upgradeTwoCost = (int)(250 * Math.Pow(1.30, upgradeTwoCount));
            circleRadius.transform.localScale += new Vector3(0.1f, 0.1f, 0);
            radius += 0.1f;
            costTwo.text = "$" + upgradeTwoCost;
            moneyCount.text = "$" + money.ToString();
            countTwo.text = upgradeTwoCount.ToString();
            

        }
    }

    public void MaxBallsUpgrade()
    {
        if (money > upgradeThreeCost)
        {
            money -= upgradeThreeCost;
            upgradeThreeCount += 1;
            upgradeThreeCost = (int)(1000 * Math.Pow(1.30, upgradeThreeCount));
            maxBalls += 10;
            costThree.text = "$" + upgradeThreeCost;
            moneyCount.text = "$" + money.ToString();
            countThree.text = upgradeThreeCount.ToString();
        }
    }

    void Update()
    {
        if (!PauseMenu.isPaused)
        {
            
        
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hitCollider = Physics2D.OverlapCircle(mousePosition, radius);

            if (hitCollider != null && hitCollider.gameObject == gameObject && !isImmune)
            {
                Destroy(gameObject);
                money += moneyGain;
                moneyCount.text = "$" + money.ToString();
                balls -= 1;
                ballCount.text = "Balls: " + balls.ToString();
            }
            if (balls == 0)
            {
                SpawnBall();
            }
        }
    }

}