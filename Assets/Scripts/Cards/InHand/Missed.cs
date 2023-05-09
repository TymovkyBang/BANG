using UnityEngine;
public class Missed : Card
{
    protected override void init()
    {
        this.numberOfCards = 12; // 12
    }

    public override bool use(){
		Debug.Log(GameManager.localPlayer.beingShotAt);
		if (!GameManager.localPlayer.beingShotAt) return false;
        GameManager.localPlayer.beingShotAt = false;
		this.numberOfCardsCurrently--;
		return true;

	}
}
