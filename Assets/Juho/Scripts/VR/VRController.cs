using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Photon.Pun;

public class VRController : MonoBehaviour
{

    public float m_Sensitivity = 0.1f;
    public float m_MaxSpeed = 1.0f;

    public SteamVR_Action_Boolean m_MovePress = null;
    public SteamVR_Action_Vector2 m_moveValue = null;

    //rotate to right stick
    public SteamVR_Action_Vector2 m_rotateValue = null;

    private float m_Speed = 0.0f;

    private CharacterController m_CharacterController = null;
    private Transform m_CameraRig = null;
    private Transform m_Head = null;

    //p3

    public float m_Gravity = 30.0f;
    public float m_RotateIncrement = 90;

    public SteamVR_Action_Boolean m_RotatePress = null;

    //[SerializeField] GameObject playerAvatar;
    //[SerializeField] GameObject cameraParent;
    int i = 0;
    //Jump
    private float m_Gravity_storer;
    public float m_JumpForce = -1000f;

    //Pointer
    public GameObject m_CanvasPointer;


    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
        m_CanvasPointer.SetActive(true);
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_CameraRig = SteamVR_Render.Top().origin;
        m_Head = SteamVR_Render.Top().head;
    }

    // Update is called once per frame
    private void Update()
    {
        //if (playerAvatar == null)
        //{
        //    playerAvatar = GameObject.FindGameObjectWithTag("PlayerAvatar");
        //}

        //if (cameraParent == null)
        //{
        //    cameraParent = GameObject.FindGameObjectWithTag("CameraParent");
        //}

        //if (playerAvatar != null && cameraParent != null && i == 0)
        //{
        //    playerAvatar.transform.SetParent(cameraParent.transform);
        //    i++;
        //}

        //HandleHead();
        HandleHeight();
        CalculateMovement();
        SnapRotation();        
    }

    /*
    private void HandleHead()
    {
        //Store current
        Vector3 oldPosition = m_CameraRig.position;
        Quaternion oldRotation = m_CameraRig.rotation;

        //Rotation
        transform.eulerAngles = new Vector3(0.0f, m_Head.rotation.eulerAngles.y, 0.0f);

        //Restore
        m_CameraRig.position = oldPosition;
        m_CameraRig.rotation = oldRotation;
    }
    */

    private void HandleHeight()
    {
        //Get the head in local space
        float headHeigth = Mathf.Clamp(m_Head.localPosition.y, 1, 2);
        m_CharacterController.height = headHeigth;

        //Cut in half
        Vector3 newCenter = Vector3.zero;
        newCenter.y = m_CharacterController.height / 2;
        newCenter.y += m_CharacterController.skinWidth;

        //Move capsule in local space
        newCenter.x = m_Head.localPosition.x;
        newCenter.z = m_Head.localPosition.z;

        //Apply
        m_CharacterController.center = newCenter;
    }

    private void CalculateMovement()
    {
        //Figure out movement orientation
        Quaternion orientation = CalculateOrientation();
        Vector3 movement = Vector3.zero;

        //If not moving
        //if (m_MovePress.GetStateUp(SteamVR_Input_Sources.Any))
        if (m_moveValue.axis.magnitude == 0)
            m_Speed = 0;

        //If player is grounded
        if (m_CharacterController.isGrounded)
        {
            //Add, clamp
            m_Speed += m_moveValue.axis.magnitude * m_Sensitivity;
            m_Speed = Mathf.Clamp(m_Speed, -m_MaxSpeed, m_MaxSpeed);

            //Orientation
            movement += orientation * (m_Speed * Vector3.forward);
        }

        //Gravity
        movement.y -= m_Gravity * Time.deltaTime;

        //Apply
        m_CharacterController.Move(movement * Time.deltaTime);
    }

    private Quaternion CalculateOrientation()
    {
        float rotation = Mathf.Atan2(m_moveValue.axis.x, m_moveValue.axis.y);
        rotation *= Mathf.Rad2Deg;

        Vector3 orientationEuler = new Vector3(0, m_Head.eulerAngles.y + rotation, 0);
        return Quaternion.Euler(orientationEuler);
    }

    private void SnapRotation()
    {
        float snapValue = 0.0f;

        if (m_RotatePress.GetStateDown(SteamVR_Input_Sources.LeftHand))
            snapValue = -Mathf.Abs(m_RotateIncrement);

        if (m_RotatePress.GetStateDown(SteamVR_Input_Sources.RightHand))
            snapValue = Mathf.Abs(m_RotateIncrement);

        transform.RotateAround(m_Head.position, Vector3.up, snapValue);
    }

    public void Jump()
    {
        Debug.Log("Player on jumppad");

        m_Gravity_storer = m_Gravity;
        m_Gravity = m_JumpForce;

        Invoke("StopJumping", 1.5f);
    }

    private void StopJumping()
    {
        m_Gravity = m_Gravity_storer;
    }
    

}
