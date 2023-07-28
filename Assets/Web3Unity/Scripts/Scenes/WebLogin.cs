using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;
using System.Numerics;
using UnityEngine.UI;

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

    public Button loginButton; // login with MM button
    public Text loginButtonText; // text what's inside that button
    public Color loginButtonTextColor = Color.white; // color picker if no NFT

    private bool ownNFT;
    
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
        // NFT contract
        string contract = "0x1aCB10DBD319DA52D941DFEC478f1aA2D118D7F7";

        // read actual balanceOf from the contract using rpc which is in the network.js 
        int balance = await ERC721.BalanceOf(contract, account);
        ownNFT = balance > 0; // set ownNFT to True or False
        print(ownNFT);
        print(balance);
        
        PlayerPrefs.SetInt("OwnNFT", ownNFT ? 1 : 0); // save is holder
        PlayerPrefs.SetString("Account", account); // save account for next scene
        SetConnectAccount(""); // reset login message

        if (balance > 0){ // if balace more 0
            // load game scene if own nft
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else{ // if balance lower 0
            loginButtonText.color = loginButtonTextColor; // red text on button
            loginButtonText.text = "You don't own the NFT";
            loginButton.interactable = false; // uninteractible login button
        }
    }

    public void OnSkip()
    {
        // burner account for skipped sign in screen
        PlayerPrefs.SetString("Account", "");
        // move to next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
#endif
