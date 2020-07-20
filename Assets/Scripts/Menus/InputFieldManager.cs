using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

/// <summary>
/// Created by Kimberly Burke
/// Used by the login and signup canvas to activate the submit button when all forms are filled
/// </summary>
public class InputFieldManager : MonoBehaviour
{
    [SerializeField] InputField[] inputs;
    [SerializeField] Button submit;
    [SerializeField] Toggle agreement;
    [SerializeField] GameObject passPopup;
    [SerializeField] GameObject confirmPopup;
    [SerializeField] GameObject offlinePopup;

    private static bool agreed = true;
    private static bool viewPass = false;
    private static bool viewConf = false;

    public void Awake()
    {
        agreement.isOn = true;
        agreed = true;
        SignupConnection();
    }

    /// <summary>
    /// Checks if fields are all filled in order to activate the submit button.
    /// </summary>
    public void CheckFields()
    {
        bool interact = true;
        bool strongPass = CheckPassword();
        for (int i = 0; i < inputs.Length; i++)
        {
            if (inputs[i].text == "")
            {
                interact = false;
            }
        }

        if (!agreed || !strongPass)
        {
            interact = false;
        }

        submit.interactable = interact;
    }

    /// <summary>
    /// Checks the password is valid. (8-20 chars with at least one number, one capital and one symbol)
    /// </summary>
    public bool CheckPassword()
    {
        string password = inputs[1].text;
        GameObject checkmark = inputs[1].transform.GetChild(3).gameObject;
        bool hasNum = Regex.IsMatch(password, @"\d");
        bool hasCap = Regex.IsMatch(password, @"[A-Z]");

        if (password.Length >= 8 && password.Length <= 20)
        {
            passPopup.transform.GetChild(0).GetComponent<Text>().color = Color.green;
        } else
        {
            passPopup.transform.GetChild(0).GetComponent<Text>().color = Color.white;
        }

        if (hasNum)
        {
            passPopup.transform.GetChild(1).GetComponent<Text>().color = Color.green;
        }
        else
        {
            passPopup.transform.GetChild(1).GetComponent<Text>().color = Color.white;
        }

        if (hasCap)
        {
            passPopup.transform.GetChild(2).GetComponent<Text>().color = Color.green;
        }
        else
        {
            passPopup.transform.GetChild(2).GetComponent<Text>().color = Color.white;
        }

        if (password.Length >= 8 && password.Length <= 20 && hasNum && hasCap)
        {
            checkmark.SetActive(true);
            return true;
        }
        checkmark.SetActive(false);
        return false;
    }

    public void ClearFields()
    {
        foreach (InputField field in inputs)
        {
            field.text = "";
        }
        PasswordMatch();

    }

    public void ToggleAgreement()
    {
        agreed = agreement.isOn;
        CheckFields();
    }

    public void ViewPassword(string field)
    {
        if (field == "password")
        {
            if (viewPass)
            {
                inputs[1].GetComponent<InputField>().contentType = InputField.ContentType.Password;
                inputs[1].GetComponent<InputField>().ForceLabelUpdate();
                viewPass = false;
            } else
            {
                inputs[1].GetComponent<InputField>().contentType = InputField.ContentType.Standard;
                inputs[1].GetComponent<InputField>().ForceLabelUpdate();
                viewPass = true;
            }
        } else if (field == "confirm")
        {
            if (viewConf)
            {
                inputs[2].GetComponent<InputField>().contentType = InputField.ContentType.Password;
                inputs[2].GetComponent<InputField>().ForceLabelUpdate();
                viewPass = false;
            }
            else
            {
                inputs[2].GetComponent<InputField>().contentType = InputField.ContentType.Standard;
                inputs[2].GetComponent<InputField>().ForceLabelUpdate();
                viewPass = true;
            }
        }
    }

    public void PasswordMatch()
    {
        string password = inputs[1].text;
        string confirm = inputs[2].text;
        GameObject checkmark = inputs[2].transform.GetChild(3).gameObject;

        if (password == confirm && password != "")
        {
            confirmPopup.SetActive(false);
            checkmark.SetActive(true);           
            
        } else
        {
            confirmPopup.SetActive(true);
            checkmark.SetActive(false);
        }
    }

    public void ValidEmail()
    {
        string email = inputs[3].text;
        GameObject checkmark = inputs[3].transform.GetChild(3).gameObject;
        // NOTE: requires updated Regex expression to check validity of email
        checkmark.SetActive(email.Contains("@") && email.Contains("."));
    }

    public void ValidUsername()
    {
        string username = inputs[0].text;
        GameObject checkmark = inputs[0].transform.GetChild(3).gameObject;
        if (username.Length < 3)
        {
            checkmark.SetActive(false);
        } else
        {
            checkmark.SetActive(true);
        }

    }

    public void SignupConnection()
    {
        DBManager.Instance.ConnectionTest();
        if (GameSaveUtility.OfflineMode == 1)
            offlinePopup.SetActive(true);
    }

    public void ConfirmOffline()
    {
        GameSaveUtility.SaveID(-2); // set player as OFFLINE account (ID = -2);
        GameSaveUtility.SavePlayerName("OFFLINE");
        GameSaveUtility.SetConnectionMode(2);
    }
}
