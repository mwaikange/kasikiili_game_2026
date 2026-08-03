using Firebase.Messaging;
using UnityEngine;

public class Fcm : MonoBehaviour
{
	private void Start()
	{
		FirebaseMessaging.TokenReceived += delegate(object sender, TokenReceivedEventArgs e)
		{
			Debug.Log("Token Received: " + e.Token);
			if (PlayerPrefs.GetString("fcm_token") == "")
			{
				PlayerPrefs.SetString("fcm_token", e.Token);
			}
		};
		FirebaseMessaging.MessageReceived += delegate(object sender, MessageReceivedEventArgs e)
		{
			Debug.Log("Message Received: " + e.Message);
		};
		FirebaseMessaging.SubscribeAsync("/topics/FusionPrix");
	}
}
