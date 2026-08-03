using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ViewModel;

[CreateAssetMenu(fileName = "LoaderManager", menuName = "Manager/Loader Manager", order = 0)]
public class LoaderManager : ScriptableObject
{
	public int loadingQueue;

	public readonly ISubject<bool> OnLoading = new Subject<bool>();

	public void OpenScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex);
	}

	public void OpenAdditiveScene(int sceneIndex)
	{
		SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);
	}

	public void CloseAdditiveScene(int sceneIndex)
	{
		if (SceneManager.sceneCount >= 2)
		{
			SceneManager.UnloadSceneAsync(sceneIndex);
		}
	}
}
