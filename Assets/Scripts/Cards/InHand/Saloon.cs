using UnityEngine;
public class Saloon : Card
{
    protected override void init()
    {
        this.numberOfCards = 1;
    }

    public override bool use(){
        GameManager.localNetwork.saloonServerRpc(GameManager.localNetwork.ID);
		this.numberOfCardsCurrently--;
		return true;
	}
}
