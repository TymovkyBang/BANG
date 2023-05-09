using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
	[SerializeField] private Button clientBtn;
	[SerializeField] private Button hostBtn;
	[SerializeField] private Button startBtn;
		
	private void Start()
	{
		startBtn.gameObject.SetActive(false);
		clientBtn.onClick.AddListener(() =>
		{
			NetworkManager.Singleton.StartClient();
			gameObject.SetActive(false);

		});
		hostBtn.onClick.AddListener(() =>
		{
			NetworkManager.Singleton.StartHost();

			startBtn.gameObject.SetActive(true);
		});
		startBtn.onClick.AddListener(() =>
		{
			GameManager.localNetwork.startGameServerRpc();
			gameObject.SetActive(false);
		});

	}
}
