using UnityEngine;
public class Stagecoach : Card
{
    protected override void init()
    {
        this.numberOfCards = 2;
    }

    public override bool use(){
        GameManager.localPlayer.CardsToDraw += 2;
		this.numberOfCardsCurrently--;
		return true;

	}
}
