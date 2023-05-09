using UnityEngine;

public class Beer : Card
{
    protected override void init()
    {
        this.numberOfCards = 6;
    }

    public override bool use(){
        if (GameManager.localPlayer.Health >= GameManager.localPlayer.MaxHealth)
        {
            GameManager.console.log("You are full health!", "blue");
            return false;
        }
        GameManager.localPlayer.Health = 1;
		this.numberOfCardsCurrently--;
        return true;
	}
}
