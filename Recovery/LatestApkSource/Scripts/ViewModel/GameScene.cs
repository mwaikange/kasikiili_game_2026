using System;

namespace ViewModel;

[Serializable]
public class GameScene
{
	public string name;

	public int index;

	public bool isDefault;

	public GameState state;
}
