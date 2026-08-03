using UnityEngine;

public class Singleton : MonoBehaviour
{
	public DataManager dataManager;

	public APIManager apiManager;

	public static Singleton Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			Object.DontDestroyOnLoad(Instance);
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}
}
