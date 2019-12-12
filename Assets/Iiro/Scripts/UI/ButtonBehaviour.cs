using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonBehaviour : MonoBehaviour
{
    [FormerlySerializedAs ("photonLobbyMobile")] [SerializeField] private PhotonLobbyMobile photonLobby;
    
    public enum State
    {
        Offline,
        Play,
        Cancel,
    }

    private Button attachedButton;

    [SerializeField] private Color offlineColor;
    [SerializeField] private Color playColor;
    [SerializeField] private Color cancelColor;

    [SerializeField] private string offlineText = "Offline";
    [SerializeField] private string playText = "Play";
    [SerializeField] private string cancelText = "Cancel";

    
    private State buttonState = State.Offline;

    private void Awake ()
    {
        attachedButton = GetComponent<Button> ();
    }

    private void Start ()
    {
        //attachedButton = GetComponent<Button> ();
        EvaluateButtonState ();
    }

    //Call this to change button behaviour
    public void ChangeButtonState (State newButtonState)
    {
        buttonState = newButtonState;
        EvaluateButtonState ();
    }
    
    // Depending on enum value, we change the button behaviour to match the possibilities.
    // To Change the button behaviour, call ChangeButtonState Method which will change the enum value and reevaluate the behaviour.
    private void EvaluateButtonState ()
    {
        switch (buttonState)
        {
            case State.Offline:
                ChangeButtonState (offlineColor, offlineText, ButtonOfflineBehaviour);
                break;
            case State.Play:
                ChangeButtonState (playColor, playText, ButtonPlayBehaviour);
                break;
            case State.Cancel:
                ChangeButtonState (cancelColor, cancelText, ButtonCancelBehaviour);
                break;
            default:
                throw new ArgumentOutOfRangeException ();
        }
    }

    //Here we modify our button to match the functionality depending on state
    private void ChangeButtonState (Color newColor, string newText, UnityAction buttonBehaviour)
    {
        ColorBlock colorBlock = attachedButton.colors;
        colorBlock.normalColor = newColor;
        colorBlock.highlightedColor = newColor;
        attachedButton.colors = colorBlock;
        GetComponentInChildren<TMP_Text> ().text = newText;
        attachedButton.onClick.RemoveAllListeners ();
        attachedButton.onClick.AddListener (buttonBehaviour);
    }

    private void ButtonOfflineBehaviour ()
    {
        //Add functionality here!
    }

    private void ButtonPlayBehaviour ()
    {
        //Add functionality here!
        photonLobby.OnPlayButtonClicked ();
    }

    private void ButtonCancelBehaviour ()
    {
        //Add functionality here!
        photonLobby.OnCancelButtonClicked ();
    }
}