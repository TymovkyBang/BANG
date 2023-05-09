using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bullet : MonoBehaviour
{   
    [SerializeField] int index;
    public void Awake() {
        this.Index = this.index;
        GameManager.healthManager.addHealthBullet(this);
		this.gameObject.SetActive(false);
	}

    public int Index{
        get {
            return this.index;
        }
        set {
            this.index = value; 
            gameObject.name = "Bullet " + this.index.ToString();
        }
    }

    public void show(bool state){
        gameObject.SetActive(state);
    }
}
