using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginMenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] InputField[] inputs;
    [SerializeField] Button submit;
    [SerializeField] GameObject offlinePopup;

    private bool viewPass = false;

    private void Awake()
    {
        LoginConnection();
    }

    /// <summary>
    /// Checks if fields are all filled in order to activate the submit button.
    /// </summary>
    public void CheckFields()
    {
        bool interact = true;
        foreach (InputField field in inputs)
        {
            if (field.text == "")
            {
                interact = false;
            }
        }
        submit.interactable = interact;
    }

    public void ClearFields()
    {
        foreach (InputField field in inputs)
        {
            field.text = "";
        }
    }

    public void ViewPassword()
    {

        if (viewPass)
        {
            inputs[1].GetComponent<InputField>().contentType = InputField.ContentType.Password;
            inputs[1].GetComponent<InputField>().ForceLabelUpdate();
            viewPass = false;
        }
        else
        {
            inputs[1].GetComponent<InputField>().contentType = InputField.ContentType.Standard;
            inputs[1].GetComponent<InputField>().ForceLabelUpdate();
            viewPass = true;
        }
    }

    public void LoginConnection()
    {
        DBManager.Instance.ConnectionTest();
        if (GameSaveUtility.OfflineMode == 1)
        {
            GameSaveUtility.SaveID(-2); // set player as OFFLINE account (ID = -2);
            GameSaveUtility.SavePlayerName("OFFLINE");
            offlinePopup.SetActive(true);
        }
    }

    public void ConfirmOffline()
    {
        GameSaveUtility.SaveID(-2); // set player as OFFLINE account (ID = -2);
        GameSaveUtility.SavePlayerName("OFFLINE");
        GameSaveUtility.SetConnectionMode(2);
    }
}

