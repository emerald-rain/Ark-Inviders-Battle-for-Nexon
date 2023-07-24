using System.Numerics;
using UnityEngine;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;

public class ERC1155BalanceOfExample : MonoBehaviour
{
    async void Start()
    {
        string contract = "0x1aCB10DBD319DA52D941DFEC478f1aA2D118D7F7";
        string account = "0x3C096BA01bD406bC8567FC1c59b5Cd94D4b32C05";
        string tokenId = "667";

        BigInteger balanceOf = await ERC1155.BalanceOf(contract, account, tokenId);
        print(balanceOf);
    }
}
