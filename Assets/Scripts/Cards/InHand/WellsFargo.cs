using UnityEngine;
public class WellsFargo : Card
{
    protected override void init()
    {
        this.numberOfCards = 1;
    }

    public override bool use(){
		GameManager.localPlayer.CardsToDraw += 3;
		this.numberOfCardsCurrently--;
		return true;

	}
}
