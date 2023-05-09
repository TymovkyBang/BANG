using UnityEngine;
public class CatBalou : Card, Targetable
{
    protected override void init()
    {
        this.numberOfCards = 4; // 4
    }

    public override bool use(){
        Debug.Log("Your cards are mine now.");
		target(GameManager.selectedPlayer.ID);
		this.numberOfCardsCurrently--;
		return true;
	}

	public void target(ulong targetID)
	{
        GameManager.localNetwork.catBalouServerRpc(GameManager.localNetwork.ID, GameManager.selectedPlayer.ID) ;
    }
}
