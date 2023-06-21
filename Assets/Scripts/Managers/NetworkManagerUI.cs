using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode.Transports.UTP;
using System.Net;
using System.Net.Sockets;

public class NetworkManagerUI : MonoBehaviour
{
	[SerializeField] private Button clientBtn;
	[SerializeField] private Button hostBtn;
	[SerializeField] private Button startBtn;
	[SerializeField] private TextMeshProUGUI IPText;

	[SerializeField] private TMP_InputField ip;
	private UnityTransport transport;

	private string ipAddress;
	private bool pcAssigned;

		
	private void Start()
	{
		ipAddress = "0.0.0.0";
		this.SetIpAddress();


		IPText.gameObject.SetActive(false);
		startBtn.gameObject.SetActive(false);
		clientBtn.onClick.AddListener(() => // CLIENT
		{
			ipAddress = ip.text;
			SetIpAddress();
			NetworkManager.Singleton.StartClient();

			if (GameManager.localNetwork.numberOfPlayers.Value > 7)
            {
				NetworkManager.Singleton.Shutdown();
            }

			gameObject.SetActive(false);

		});
		hostBtn.onClick.AddListener(() => // HOST
		{
			NetworkManager.Singleton.StartHost();
			GetLocalIPAddress();
			ip.gameObject.SetActive(false);
			IPText.gameObject.SetActive(true);
			startBtn.gameObject.SetActive(true);
		});
		startBtn.onClick.AddListener(() => // START GAME
		{
			if (GameManager.localNetwork.numberOfPlayers.Value < 4)
			{
				Debug.Log("Not enought players in the session!");
				return;
			}
			GameManager.localNetwork.startGameServerRpc();
			gameObject.SetActive(false);
		});

	}

	public void SetIpAddress()
	{
		transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
		transport.ConnectionData.Address = ipAddress;
	}
	public string GetLocalIPAddress()
	{
		var host = Dns.GetHostEntry(Dns.GetHostName());
		foreach (var ip in host.AddressList)
		{
			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				IPText.text = ip.ToString();
				ipAddress = ip.ToString();
				return ip.ToString();
			}
		}
		throw new System.Exception("No network adapters with an IPv4 address in the system!");
	}
}
