using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

#if UNITY_WEBGL
public class WebLogin : MonoBehaviour
{
    ProjectConfigScriptableObject projectConfigSO = null;

    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);

    private int expirationTime;
    private string account;

    [Header("Button links")]
    public Button loginButton; // login with MM button
    public Text loginButtonText; // text what's inside that button
    public Color loginButtonTextColor = Color.white; // color picker if no NFT 

    private SoundManager soundManager; // SoundManager
    private bool ownNFT;

    void Start() 
    {
        projectConfigSO = (ProjectConfigScriptableObject)Resources.Load("ProjectConfigData", typeof(ScriptableObject));
        PlayerPrefs.SetString("ProjectID", projectConfigSO.ProjectId);
        PlayerPrefs.SetString("ChainID", projectConfigSO.ChainId);
        PlayerPrefs.SetString("Chain", projectConfigSO.Chain);
        PlayerPrefs.SetString("Network", projectConfigSO.Network);
        PlayerPrefs.SetString("RPC", projectConfigSO.Rpc);

        soundManager = SoundManager.Instance; // SoundManager
    }

    public void OnLogin()
    {
        soundManager.playSoundMM();
        Web3Connect();
        OnConnected();
    }

    async private void OnConnected()
    {
        account = ConnectAccount();
        while (account == "") {
            await new WaitForSeconds(1f);
            account = ConnectAccount();
        };

        CheckBalanceAndLoadScene(account);
    }

    async public void CheckBalanceAndLoadScene(string account)
    {
        // NFT contract
        string contract = "0x1aCB10DBD319DA52D941DFEC478f1aA2D118D7F7";
        string chain = "exosama";
        string network = "mainnet";
        string rpc = "https://rpc.exosama.com";

        int balance = await ERC721.BalanceOf(chain, network, contract, account, rpc);
        ownNFT = balance > 0; // set ownNFT to True or False
        print(ownNFT);
        print(balance);
        
        print("point3");
        PlayerPrefs.SetInt("OwnNFT", ownNFT ? 1 : 0); // save is holder
        PlayerPrefs.SetString("Account", account); // save account for next scene
        SetConnectAccount(""); // reset login message

        if (balance > 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else {
            loginButtonText.color = loginButtonTextColor; // red text on button
            loginButtonText.text = "You don't own the NFT";
            loginButton.interactable = false; // uninteractible login button
        }
    }

    public void OnSkip()
    {
        soundManager.playSoundSkip(); // skip button sound
        PlayerPrefs.SetString("Account", ""); // saving empty account string
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
#endif
