using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GamePlayer : MonoBehaviour
{
    public string Id;
    public string Username;

    public TextMeshPro txt;

    public void SetTxt()
    {
        txt.text = Username;
    }

    public void SetUp(int i)
    {
        transform.GetChild(i).gameObject.SetActive(true);
    }
}
