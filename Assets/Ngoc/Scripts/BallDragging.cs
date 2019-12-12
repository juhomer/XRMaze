using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallDragging : MonoBehaviourPun
{
    [SerializeField] float maxStretch = 3.0f;
    [SerializeField] LineRenderer leftLine;
    [SerializeField] LineRenderer rightLine;

    [SerializeField] Transform LeftEdge;
    [SerializeField] Transform RightEdge;

    SpringJoint springJoint;
    Rigidbody rBBall;
    Vector3 prevVelocity;


    [SerializeField] GameObject ballHoldPoint;
    [SerializeField] ActivateSlingDetector activateSlingDetectorScript;

    // Instruction message reference
    [SerializeField] GameObject instructionMessageBox; 
    Text instructionMessageText;

    // ball support
    [SerializeField] GameObject BallPrefab;
    GameObject ball;
    Timer ballSelfDestructionTimer;
    Timer cooldownTimer;
    int shootedTime = 0;
    int maxShootTime = 3;

    PhotonView pV;
    SlingAudio slingAudio;

    private void Awake()
    {
        springJoint = GetComponent<SpringJoint>();
        pV = GetComponent<PhotonView>();
        instructionMessageBox = GameObject.FindGameObjectWithTag("InstructionMessageBox");
    }

    // Start is called before the first frame update
    void Start()
    {
        slingAudio = GameObject.FindGameObjectWithTag("AudioSling").GetComponent<SlingAudio>();
        RespawnBall();
        LineRendererSetup();

        instructionMessageText = instructionMessageBox.transform.GetChild(0).GetComponent<Text>();
        instructionMessageBox.SetActive(false);

        // timer for destroy the ball
        ballSelfDestructionTimer = gameObject.AddComponent<Timer>();
        ballSelfDestructionTimer.Duration = 3f;
        ballSelfDestructionTimer.AddTimerFinishedEventListener(HandleBallSelfDestructionTimerFinishedEventListener);

        // timer for cooldown
        cooldownTimer = gameObject.AddComponent<Timer>();
        cooldownTimer.Duration = 0.5f;
        cooldownTimer.AddTimerFinishedEventListener(HandleCooldownTimerFinishedEventListener);

        //audioSling = GameObject.FindGameObjectWithTag("AudioSling");
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K)) {
            slingAudio.PlaySlingLoadSound();
        }
        if (springJoint.connectedBody != null)
        {
            if(!rBBall.isKinematic)
            {

                if (prevVelocity.sqrMagnitude > rBBall.velocity.sqrMagnitude)
                {
                    springJoint.connectedBody = null;
                    rBBall.velocity = prevVelocity;

                    ballSelfDestructionTimer.Run();
                }
                prevVelocity = rBBall.velocity;

            }    
            LineRendererUpdate();
        }

        if (activateSlingDetectorScript.isSlingActivated)
        {
            if (ballHoldPoint == null)
            {
                // Find point on mobile device to attach the ball into
                ballHoldPoint = GameObject.FindGameObjectWithTag("BallHoldPoint");
            }

            // Track a single touch to control the slingshot and modify instruction message
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // Handle shooting action based on TouchPhase
                switch (touch.phase)
                {
                    case TouchPhase.Began:

                        //pV.RPC("CallPlaySlingLoadSoundMethod", RpcTarget.All);
                        slingAudio.PlaySlingLoadSound();
                        // change message
                        instructionMessageText.text = "Release to shoot.";

                        // attach the ball to the ballHoldPoint
                        ball.transform.position = ballHoldPoint.transform.position;
                        ball.transform.SetParent(ballHoldPoint.transform);
                        break;

                    case TouchPhase.Ended:
                        //pV.RPC("CallPlaySlingShotSoundMethod", RpcTarget.All);
                        slingAudio.PlaySlingShotSound();
                        // realse the ball
                        rBBall.isKinematic = false;
                        ball.transform.parent = null;
                        shootedTime++;

                        // hide the message
                        instructionMessageBox.SetActive(false);

                        leftLine.enabled = false;
                        rightLine.enabled = false;
                        break;
                }
            }
            else
            {
                instructionMessageBox.SetActive(true);
                instructionMessageText.text = "Tap anywhere and hold to drag the ball.";
            }
        }
        else
        {
            instructionMessageBox.SetActive(false);
        }
    }

    void LineRendererSetup()
    {
        leftLine.SetPosition(0, leftLine.transform.position);
        rightLine.SetPosition(0, rightLine.transform.position);
    }

    void LineRendererUpdate()
    {
        leftLine.SetPosition(1, LeftEdge.position);
        rightLine.SetPosition(1, RightEdge.position);
    }

    private void RespawnBall()
    {
        ball = PhotonNetwork.Instantiate(this.BallPrefab.name, this.gameObject.transform.position, this.gameObject.transform.rotation);
        ball.transform.parent = this.gameObject.transform;

        if (ball != null)
        {
            rBBall = ball.GetComponent<Rigidbody>();

            springJoint.connectedBody = ball.GetComponent<Rigidbody>();

            LeftEdge = ball.transform.GetChild(0).transform;
            RightEdge = ball.transform.GetChild(1).transform;

        }
    }

    private void HandleBallSelfDestructionTimerFinishedEventListener()
    {
        PhotonNetwork.Destroy(ball);
        prevVelocity = Vector3.zero;
        
        if(shootedTime >= maxShootTime)
        {
            Destroy(this.transform.parent.gameObject);
        }

        cooldownTimer.Run();
    }

    private void HandleCooldownTimerFinishedEventListener()
    {
        leftLine.enabled = true;
        rightLine.enabled = true;

        RespawnBall();
    }

    /*[PunRPC]
    void CallPlaySlingLoadSoundMethod()
    {
        audioSling.GetComponent<SlingAudio>().PlaySlingLoadSound();
    }

    [PunRPC]
    void CallPlaySlingShotSoundMethod()
    {
        audioSling.GetComponent<SlingAudio>().PlaySlingShotSound();
    }*/
}
