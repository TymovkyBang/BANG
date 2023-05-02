
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    [SerializeField] protected int index;
    public new string name;
    public string description;
    public Sprite artwork;
    public Sprite background;

    void Start() {
        GameManager.cardManager.addCard(this);
    }

    public int Index{
        get {
            return this.index;
        }
        set {
            this.index = value; 
        }
    }

    public abstract void use();

}
