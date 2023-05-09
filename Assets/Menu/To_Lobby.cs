using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class To_Lobby : MonoBehaviour
{
    public void press(){
        SceneManager.LoadScene("Lobby");
    }
}
