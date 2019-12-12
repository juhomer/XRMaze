using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : MonoBehaviour
{

    private VRController m_VRController;

    // Start is called before the first frame update
    void Start()
    {
        m_VRController = GameObject.Find("VRController(Clone)").GetComponent<VRController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {

            m_VRController.Jump();
            Destroy(this.gameObject);

        }
    }
}
