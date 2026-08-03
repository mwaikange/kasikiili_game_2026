using System.Collections;
using System.Collections.Generic;
using Commands;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ViewModel;

public class Leaderboard : MonoBehaviour
{
	[SerializeField]
	private GameObject menuAnchor;

	[SerializeField]
	private Loading loading;

	[SerializeField]
	private ErrorManager errorManager;

	[SerializeField]
	private Canvas canvas;

	[SerializeField]
	private Camera camera;

	[SerializeField]
	private GameObject table;

	[SerializeField]
	private GameObject wheel;

	[SerializeField]
	private RouletteManager rouletteManager;

	[SerializeField]
	private TableManager tableManager;

	[SerializeField]
	private RouletteCmdFactory rouletteCmdFactory;

	[SerializeField]
	private AudioManager audioManager;

	[Header("LEADERBOARD")]
	[SerializeField]
	private GameObject leaderBoard;

	[SerializeField]
	private TextMeshProUGUI activeReferrals;

	[SerializeField]
	private TextMeshProUGUI playerPosition;

	[SerializeField]
	private Transform leaderboardContent;

	[SerializeField]
	private Transform player;

	[SerializeField]
	private TextMeshProUGUI currentActiveReferrals;

	[SerializeField]
	private Transform candidateRef;

	[SerializeField]
	private Sprite leaderboardUp;

	[SerializeField]
	private Sprite leaderboardDown;

	[SerializeField]
	private Transform candidateCollection;

	[SerializeField]
	private ParticleSystem firework;

	[SerializeField]
	private AudioSource playerUpgradedAudio;

	[SerializeField]
	private Texture2D leaderboardPromotion;

	private bool isPlayerUpgrated;

	private float candidateRefHeight;

	[Header("LEADERBOARD INFO")]
	[SerializeField]
	private GameObject leaderBoardInfo;

	[SerializeField]
	private RectTransform leaderBoardInfoContent;

	[SerializeField]
	private TextMeshProUGUI prizeDistributionTotalLabel;

	[SerializeField]
	private List<TextMeshProUGUI> prizeDistributionLabel;

	[SerializeField]
	private List<TextMeshProUGUI> prizeDistributionAmount;

	private void Awake()
	{
		leaderBoard.SetActive(value: false);
		leaderBoardInfo.SetActive(value: false);
		UpdateLeaderboardInfoContentSize();
		candidateRefHeight = candidateRef.GetComponent<RectTransform>().sizeDelta.y;
		firework.Stop();
		firework.gameObject.SetActive(value: false);
	}

	private void HandleCanvasSettings(bool isReset)
	{
		if (isReset)
		{
			canvas.renderMode = RenderMode.ScreenSpaceOverlay;
			table.SetActive(value: true);
			wheel.SetActive(value: true);
		}
		else
		{
			canvas.renderMode = RenderMode.ScreenSpaceCamera;
			canvas.worldCamera = camera;
			table.SetActive(value: false);
			wheel.SetActive(value: false);
		}
	}

	public void OpenLeaderboard()
	{
		loading.Enable();
		firework.Stop();
		firework.gameObject.SetActive(value: false);
		playerUpgradedAudio.Stop();
		Singleton.Instance.apiManager.CallLeaderboardAPI(delegate(bool status, string error)
		{
			if (status)
			{
				isPlayerUpgrated = false;
				LoadLeaderboardData();
				loading.Disable();
				menuAnchor.SetActive(value: false);
				leaderBoard.SetActive(value: true);
				HandleCanvasSettings(isReset: false);
				ShowUpgradedAnimation();
			}
			else
			{
				HandleCanvasSettings(isReset: true);
				loading.Disable();
				errorManager.Show(error);
			}
		});
	}

	public void CloseLeaderBoard()
	{
		HandleCanvasSettings(isReset: true);
		rouletteManager.tableActive.Value = true;
		rouletteManager.gameActive.Value = true;
		firework.Stop();
		firework.gameObject.SetActive(value: false);
		playerUpgradedAudio.Stop();
		leaderBoard.SetActive(value: false);
		if (tableManager.cashManager.currentCredit.Value >= rouletteManager.gameLimit)
		{
			rouletteCmdFactory.TurnRouletteState(rouletteManager, audioManager, RouletteState.Cashout).Execute();
		}
		else
		{
			rouletteCmdFactory.TurnRouletteState(rouletteManager, audioManager, RouletteState.Game).Execute();
		}
	}

	public void RefreshLeaderboard()
	{
		loading.Enable();
		firework.Stop();
		firework.gameObject.SetActive(value: false);
		playerUpgradedAudio.Stop();
		Singleton.Instance.apiManager.CallLeaderboardAPI(delegate(bool status, string error)
		{
			if (status)
			{
				isPlayerUpgrated = false;
				LoadLeaderboardData();
				loading.Disable();
				ShowUpgradedAnimation();
			}
			else
			{
				loading.Disable();
				errorManager.Show(error);
			}
		});
	}

	private void LoadLeaderboardData()
	{
		activeReferrals.text = Singleton.Instance.dataManager.leaderboard.data.active_referrals;
		playerPosition.text = Singleton.Instance.dataManager.leaderboard.data.player_position;
		currentActiveReferrals.text = Singleton.Instance.dataManager.leaderboard.data.active_referrals;
		UpdateCandidatesCount();
		UpdateCandidateCollectionSize();
		UpdateLeaderboardSize();
		LoadCandidateData();
		LoadPlayerData();
	}

	private void UpdateCandidatesCount()
	{
		if (candidateCollection.childCount > Singleton.Instance.dataManager.leaderboard.data.candidates.Count)
		{
			for (int i = 0; i < candidateCollection.childCount; i++)
			{
				if (i < Singleton.Instance.dataManager.leaderboard.data.candidates.Count)
				{
					candidateCollection.GetChild(i).gameObject.SetActive(value: true);
				}
				else
				{
					candidateCollection.GetChild(i).gameObject.SetActive(value: false);
				}
			}
		}
		else if (Singleton.Instance.dataManager.leaderboard.data.candidates.Count > candidateCollection.childCount)
		{
			for (int j = 0; j < Singleton.Instance.dataManager.leaderboard.data.candidates.Count; j++)
			{
				if (j > candidateCollection.childCount - 1)
				{
					Object.Instantiate(candidateRef, candidateCollection).gameObject.SetActive(value: true);
				}
				else
				{
					candidateCollection.GetChild(j).gameObject.SetActive(value: true);
				}
			}
		}
		else
		{
			for (int k = 0; k < candidateCollection.childCount; k++)
			{
				candidateCollection.GetChild(k).gameObject.SetActive(value: true);
			}
		}
	}

	private void UpdateCandidateCollectionSize()
	{
		int num = 0;
		for (int i = 0; i < candidateCollection.childCount; i++)
		{
			if (candidateCollection.GetChild(i).gameObject.activeSelf)
			{
				num = i + 1;
			}
		}
		float num2 = candidateCollection.GetComponent<VerticalLayoutGroup>().spacing * (float)(num - 1);
		float y = (float)num * candidateRefHeight + num2;
		RectTransform component = candidateCollection.GetComponent<RectTransform>();
		component.sizeDelta = new Vector2(component.sizeDelta.x, y);
	}

	private void UpdateLeaderboardSize()
	{
		float num = leaderboardContent.GetComponent<VerticalLayoutGroup>().spacing * (float)(leaderboardContent.childCount - 1);
		for (int i = 0; i < leaderboardContent.childCount; i++)
		{
			num += leaderboardContent.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
		}
		RectTransform component = leaderboardContent.GetComponent<RectTransform>();
		component.sizeDelta = new Vector2(component.sizeDelta.x, num);
	}

	private void LoadCandidateData()
	{
		for (int i = 0; i < Singleton.Instance.dataManager.leaderboard.data.candidates.Count; i++)
		{
			LeaderboardPerson leaderboardPerson = Singleton.Instance.dataManager.leaderboard.data.candidates[i];
			Transform child = candidateCollection.GetChild(i);
			child.GetChild(0).GetComponent<Image>().sprite = (leaderboardPerson.is_upgraded ? leaderboardUp : leaderboardDown);
			child.GetChild(1).GetComponent<TextMeshProUGUI>().text = leaderboardPerson.rank;
			child.GetChild(2).GetComponent<TextMeshProUGUI>().text = leaderboardPerson.mobile_number;
			child.GetChild(3).GetComponent<TextMeshProUGUI>().text = leaderboardPerson.score.ToString("F1") + " Pts";
		}
	}

	private void LoadPlayerData()
	{
		isPlayerUpgrated = Singleton.Instance.dataManager.leaderboard.data.player.is_upgraded;
		player.GetChild(0).GetComponent<Image>().sprite = (isPlayerUpgrated ? leaderboardUp : leaderboardDown);
		player.GetChild(1).GetComponent<TextMeshProUGUI>().text = Singleton.Instance.dataManager.leaderboard.data.player.rank;
		player.GetChild(2).GetComponent<TextMeshProUGUI>().text = Singleton.Instance.dataManager.leaderboard.data.player.mobile_number;
		player.GetChild(3).GetComponent<TextMeshProUGUI>().text = Singleton.Instance.dataManager.leaderboard.data.player.score.ToString("F1") + " Pts";
	}

	private void ShowUpgradedAnimation()
	{
		if (isPlayerUpgrated)
		{
			firework.gameObject.SetActive(value: true);
			firework.Play();
			StartCoroutine(StopFirework());
			playerUpgradedAudio.Play();
		}
		else
		{
			firework.Stop();
			firework.gameObject.SetActive(value: false);
			playerUpgradedAudio.Stop();
		}
	}

	private IEnumerator StopFirework()
	{
		yield return new WaitForSeconds(12f);
		if (firework.isPlaying)
		{
			firework.Stop();
			firework.gameObject.SetActive(value: false);
			playerUpgradedAudio.Stop();
		}
	}

	public void OpenLeaderBoardInfo()
	{
		Singleton.Instance.apiManager.CallPrizeDistributionAPI(delegate(bool status, string error)
		{
			if (status)
			{
				LoadPrizeDistributionData();
				leaderBoardInfo.SetActive(value: true);
			}
			else
			{
				errorManager.Show(error);
			}
		});
	}

	private void LoadPrizeDistributionData()
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < Singleton.Instance.dataManager.prizeDistributionData.data.Count; i++)
		{
			PrizeDistribution prizeDistribution = Singleton.Instance.dataManager.prizeDistributionData.data[i];
			if (prizeDistribution.position.Contains("-"))
			{
				string[] array = prizeDistribution.position.Split('-');
				prizeDistributionLabel[i].text = array[0] + GetDaySuffix(array[0]) + " - " + array[1] + GetDaySuffix(array[1]) + " Place";
				num2 = (float)(int.Parse(array[1]) - int.Parse(array[0]) + 1) * prizeDistribution.price_amount;
			}
			else
			{
				prizeDistributionLabel[i].text = prizeDistribution.position + GetDaySuffix(prizeDistribution.position) + " Place";
				num2 = prizeDistribution.price_amount;
			}
			num += num2;
			prizeDistributionAmount[i].text = prizeDistribution.price_amount.ToString();
		}
		prizeDistributionTotalLabel.text = num.ToString();
	}

	private string GetDaySuffix(string day)
	{
		switch (day)
		{
		case "01":
		case "21":
		case "31":
			return "st";
		case "02":
		case "22":
			return "nd";
		case "03":
		case "23":
			return "rd";
		default:
			return "th";
		}
	}

	public void CloseLeaderBoardInfo()
	{
		leaderBoardInfo.SetActive(value: false);
	}

	private void UpdateLeaderboardInfoContentSize()
	{
		float num = leaderBoardInfoContent.GetComponent<VerticalLayoutGroup>().spacing * (float)(leaderBoardInfoContent.childCount - 1);
		for (int i = 0; i < leaderBoardInfoContent.childCount; i++)
		{
			num += leaderBoardInfoContent.GetChild(i).GetComponent<RectTransform>().sizeDelta.y;
		}
		leaderBoardInfoContent.sizeDelta = new Vector2(leaderBoardInfoContent.sizeDelta.x, num);
	}

	public void DownloadLeaderboardPromotion()
	{
		Application.OpenURL("https://drive.google.com/file/d/1Gh4_I7bVdJuKHfdNuoigVhLrvqUeHw7g/view?usp=sharing");
	}
}
