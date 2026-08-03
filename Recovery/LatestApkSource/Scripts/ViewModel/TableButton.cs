using UniRx;
using UnityEngine;

namespace ViewModel;

[CreateAssetMenu(fileName = "New Number", menuName = "Scriptable/Table/Table Button")]
public class TableButton : ScriptableObject
{
	public int numberParent;

	public int betValue;

	public BoolReactiveProperty isSelected = new BoolReactiveProperty();

	public readonly ISubject<bool> Refresh = new Subject<bool>();
}
