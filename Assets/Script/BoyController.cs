using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoyController : MonoBehaviour
{

    public enum GameState
    {
        Running,
        Waiting,
        Scuding,
        Painting,
        GameOver,

    }
    public GameState currentState;
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private Rigidbody rbBoy;
    [SerializeField] private Camera cam;
    [SerializeField] private float lerpValue = 5f;
    [SerializeField] private float newXPos;
    private float firstXPos = 0f, differenceXPos = 0f;
    bool onRotating = false;
    public GameObject paintingObject;
    private GameManager gameManager;
    public GameObject paintingPercent;

    void Start()
    {
        currentState = GameState.Waiting;

        paintingPercent.SetActive(false);

        gameManager = FindObjectOfType<GameManager>();
        paintingObject.GetComponent<PaintingController>().enabled = false;
        currentState = GameState.Waiting;
        newXPos = transform.position.x;
    }

    void Update()
    {
        if (transform.localPosition.z >= 62)
        {
            currentState = GameState.Painting;
        }
        if (transform.position.y <= -2)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        if (onRotating)
        {

            newXPos = Mathf.MoveTowards(newXPos, newXPos - 1, 0.18f);
        }
    }
    void FixedUpdate()
    {
        switch (currentState)
        {
            case GameState.Waiting:
                for (int i = 0; i < gameManager.character.Count; i++)
                {
                    if (gameManager.character[i].tag == "Opponent")
                        gameManager.character[i].GetComponent<OpponentController>().enabled = false;
                }
                if (Input.GetMouseButtonDown(0))
                {
                    for (int i = 0; i < gameManager.character.Count; i++)
                    {
                        if (gameManager.character[i].tag == "Opponent")
                            gameManager.character[i].GetComponent<OpponentController>().enabled = true;
                    }
                    currentState = GameState.Running;
                }

                break;
            case GameState.Running:
                if (Input.GetMouseButtonDown(0))
                {

                    firstXPos = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
                }
                if (Input.GetMouseButton(0))
                {
                    BoyMovement();
                    rbBoy.MovePosition(new Vector3(Mathf.Lerp(transform.position.x, newXPos, lerpValue * Time.fixedDeltaTime), transform.position.y, transform.position.z + runSpeed * Time.fixedDeltaTime));
                }
                break;

            case GameState.Painting:

                paintingObject.GetComponent<PaintingController>().enabled = true;

                paintingPercent.SetActive(true);

                if (Input.GetMouseButtonDown(0))
                {
                    firstXPos = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
                }
                if (Input.GetMouseButton(0))
                {
                    BoyMovement();
                    rbBoy.MovePosition(new Vector3(Mathf.Lerp(transform.position.x, newXPos, lerpValue * Time.fixedDeltaTime), transform.position.y, 68f));
                }



                break;
            default: break;
        }
    }
    void BoyMovement()
    {
        differenceXPos = (Camera.main.ScreenToViewportPoint(Input.mousePosition).x - firstXPos) * 10;
        newXPos += differenceXPos;
        differenceXPos = 0f;
        firstXPos = Camera.main.ScreenToViewportPoint(Input.mousePosition).x;
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "HorizontalObs" || other.gameObject.tag == "StaticObs")
        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (other.gameObject.tag == "RotatingPlatform")
        {

            onRotating = true;
        }
        else if (other.gameObject.tag == "HalfDonut")
        {
            currentState = GameState.Scuding;
        }
        else if (other.gameObject.tag == "Rotater")
        {
            gameManager.rotaterIsRot = false;
        }
    }
    void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "RotatingPlatform")
        {

            onRotating = true;
        }
    }
    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.tag == "RotatingPlatform")
        {

            onRotating = false;
        }
        else if (other.gameObject.tag == "Rotater")
        {
            gameManager.rotaterIsRot = true;
        }
        else if (other.gameObject.tag == "HalfDonut")
        {
            currentState = GameState.Running;
        }
    }
}
