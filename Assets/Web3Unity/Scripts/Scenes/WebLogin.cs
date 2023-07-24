using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;
using System.Numerics;

#if UNITY_WEBGL
public class WebLogin : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);

    private int expirationTime;
    private string account;
    ProjectConfigScriptableObject projectConfigSO = null;
    
    void Start()
    {
        // loads the data saved from the editor config
        projectConfigSO = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));
        PlayerPrefs.SetString("ProjectID", projectConfigSO.ProjectID);
        PlayerPrefs.SetString("ChainID", projectConfigSO.ChainID);
        PlayerPrefs.SetString("Chain", projectConfigSO.Chain);
        PlayerPrefs.SetString("Network", projectConfigSO.Network);
        PlayerPrefs.SetString("RPC", projectConfigSO.RPC);
    }

    public void OnLogin()
    {
        Web3Connect();
        OnConnected();
    }

    async private void OnConnected()
    {
        account = ConnectAccount();
        while (account == "")
        {
            await new WaitForSeconds(1f);
            account = ConnectAccount();
        };

        // Perform the balance check after login
        CheckBalanceAndLoadScene(account);
    }

    private async void CheckBalanceAndLoadScene(string account)
    {
        string contract = "0x1aCB10DBD319DA52D941DFEC478f1aA2D118D7F7";

        int balance = await ERC721.BalanceOf(contract, account);
        print(balance);

        // save account for next scene
        PlayerPrefs.SetString("Account", account);

        // reset login message
        SetConnectAccount("");

        // Check the balance and load the appropriate scene
        if (balance > 0)
        {
            // Load next scene if the balance is greater than 0
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            // Reload the current scene if the balance is 0
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void OnSkip()
    {
        // burner account for skipped sign in screen
        PlayerPrefs.SetString("Account", "");
        // move to next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
    }
}
#endif
