using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GamePlayer : MonoBehaviour
{
    public string Id { get; set; }
    public string Username { get; set; }

    public TMP_Text usertext;
    

    private void Start()
    {
        usertext = GetComponent<TMP_Text>();
        
    }

}