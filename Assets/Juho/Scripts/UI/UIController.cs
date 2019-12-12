using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{

    public Canvas winCanvas;
    public VRController vrController;
    public Camera canvasPointerCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            vrController = other.gameObject.GetComponent<VRController>();
            winCanvas.gameObject.SetActive(true);
            //canvasPointerCamera = other.transform.Find("PR_Pointer").gameObject.GetComponent<Camera>();
            canvasPointerCamera = vrController.m_CanvasPointer.GetComponent<Camera>();
            winCanvas.worldCamera = canvasPointerCamera;
            //vrController = GameObject.Find("VRController").GetComponent<VRController>();
            vrController.m_CanvasPointer.SetActive(true);
        }
    }

    public void GoToMainMenu()
    {
        Debug.Log("To main menu");
        SceneManager.LoadScene(0);
    }
}
