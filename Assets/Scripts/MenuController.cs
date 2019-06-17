namespace VRTK.Examples
{
    using UnityEngine;


    //USE: attach to EACH controller in VRTK_Scripts object
    public class MenuController : MonoBehaviour
    {
        private bool isPressed = false; //ensures DoPressed only activates once
        private void Start()
        { 
            if (GetComponent<VRTK_ControllerEvents>() == null)
            {
                VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerEvents_ListenerExample", "VRTK_ControllerEvents", "the same"));
                return;
            }

            GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed); 
            GetComponent<VRTK_ControllerEvents>().TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);

        }

        private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
        {
            if(!isPressed) MenuManager.instance.MenuButtonPressed(this.gameObject); //passes the controller it is attached to as a parameter
            isPressed = true; 
        }

        private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
        {
            isPressed = false;
            MenuManager.instance.MenuButtonUnpressed();
        }
        
    }
}