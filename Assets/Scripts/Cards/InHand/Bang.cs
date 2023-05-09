
using UnityEngine;

public class Bang : Card, Targetable
{
    protected override void init()
    {
        this.numberOfCards = 25; // Maximální po?et karet v kolob?hu hry // 25
	}

    public override bool use(){ // Vrací bool podle toho, jestli se poda?ilo kartu použít ( p?. BANG se dá použít jen jednou za kolo, takže když to zkusíš vícekrát, vrátí se false )
        if (GameManager.localPlayer.usedBang) return false;
		if (GameManager.localPlayer.onCooldown)
		{
			GameManager.console.log("You are on cooldown!", "blue");
			return false;
		}
		target(GameManager.selectedPlayer.ID);
		GameManager.cooldown();
		if (!GameManager.localPlayer.hasVolcanic) GameManager.localPlayer.usedBang = true;
        this.numberOfCardsCurrently--; // Po?et karet v kolob?hu hry ( ?ím víc karet, tím v?tší ?íslo, jelikož se karta použila, odhodila se do balí?ku a m?že se použít znovu )
        return true;
	}
	public void target(ulong targetID) // Tahle karta rozši?uje interface 'Targetable', takže když ji použiješ, musíš tam dát i ID hrá?e, na kterýho to chceš použít.
	{
		GameManager.localNetwork.shootServerRpc(GameManager.localNetwork.OwnerClientId, (int)targetID, name); // Pošle na server request, aby dostal targetID damage. První argument je ID hráče, kterej posílá ten request
    }
}
