using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Car : MonoBehaviour
{
    [SerializeField] private float _defautSpeed = 10f;
    [SerializeField] private float _boostSpeed = 30f;
    private float _speed;
    [SerializeField] private float _turnSpeed = 250f;

    [SerializeField] private float _packageDestroyDelay = 0.25f;

    [SerializeField] private Sprite[] _carSprites;
    private SpriteRenderer _spriteRenderer;
    private Color packageColor = Color.black;

    [SerializeField] private Text infoText;
    [SerializeField] private Text timeText;

    private float gameTime;
    private bool gameComplete = false;
    private bool gameStarted = false;

    private float _verticalAxis = 0;
    private float _horizontalAxis = 0;

    private bool _hasPackage = false;
    private int deliveredPackages;

    private int carSpriteIdx = 0;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if ( _spriteRenderer == null ) { Debug.LogError("_spriteRenderer is NULL in Car script"); }

        _speed = _defautSpeed;

        infoText.text = "Collect Square Packages and deliver to same color Hexagon Customers\n" +
            "'Tab' to change cars. 'wasd' to move/ turn. 'Spacebar' to reload game";
    }

    // Update is called once per frame
    void Update()
    {

        //select car type with tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            carSpriteIdx++;
            if (carSpriteIdx >= _carSprites.Length)
            {
                carSpriteIdx = 0;
            }
            _spriteRenderer.sprite = _carSprites[carSpriteIdx];
        } 
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("MainGame");
        }
        else if ( (Input.GetAxis("Horizontal") + Input.GetAxis("Vertical")) != 0 )
        {
            gameStarted = true;
        }

        if (gameStarted)
        { 
            gameTime += Time.deltaTime;
            timeText.text = "Time:\n" + gameTime.ToString("0.00") + " s";

            _verticalAxis = Input.GetAxis("Vertical");
            _horizontalAxis = Input.GetAxis("Horizontal");

            this.transform.Translate(0, _verticalAxis * _speed * Time.deltaTime, 0, transform);
            this.transform.Rotate(0, 0, -_horizontalAxis * _turnSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Package":
                if (_hasPackage)
                {
                    Debug.Log("Already have a package, cannot pickup another: " + collision.gameObject.name);
                    infoText.text = "You already have a package (square) and cannot pick up another.";
                }
                else
                {
                    Debug.Log("Picked up Package: " + collision.gameObject.name);
                    infoText.text = "Picked up package (square), now deliver it to the same color customer (hexagon).";
                    _hasPackage = true;
                    packageColor = collision.GetComponent<SpriteRenderer>().color;
                    _spriteRenderer.color = collision.GetComponent<SpriteRenderer>().color;
                    Destroy(collision.gameObject, _packageDestroyDelay);      
                }
                break;
            case "Customer":
                if (_hasPackage)       
                {
                    if (collision.GetComponent<SpriteRenderer>().color == packageColor)
                    {
                        Debug.Log("Delivered package to Customer: " + collision.gameObject.name);
                        infoText.text = "Good Job! Delivered package to customer.";
                        _hasPackage = false;
                        deliveredPackages++;
                        if (deliveredPackages == 4)
                        {
                            infoText.text = "Good Job! All Packages delivered! Press Spacebar to start a new game.";
                            _speed = 0;
                            gameComplete = true;
                        }
                        _spriteRenderer.color = Color.white;
                        Destroy(collision.gameObject, _packageDestroyDelay);
                    } else
                    {
                        Debug.Log("Wrong color customer, cannot deliver package to: " + collision.gameObject.name);
                        infoText.text = "You must deliver the package (square) to the same color customer (hexagon).";
                    }
                    

                } else
                {
                    Debug.Log("Dont have a package, cannot diliver to customer: " + collision.gameObject.name);
                    infoText.text = "You must have a package in order to deliver it to a customer.";
                }
                break;
            case "Boost":
                Debug.Log("Car got a boost: " + collision.gameObject.name);
                infoText.text = "You got a speed boost, dont hit anything or the boost will go away!";
                _speed = _boostSpeed;
                //Destroy(collision.gameObject, _packageDestroyDelay);
                break;
            default:
                Debug.Log("Car entered trigger: " + collision.gameObject.name);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Car collided with: " + collision.gameObject);
        if (_speed == _boostSpeed)
        { 
            infoText.text = "Collision! Boost removed.";
            _speed = _defautSpeed;
        }
        
    }
}
