using System;
using UniRx;
using UnityEngine;

namespace ViewModel;

[CreateAssetMenu(fileName = "ErrorManager", menuName = "Manager/Error Manager", order = 0)]
public class ErrorManager : ScriptableObject
{
	public ReactiveProperty<Exception> lastException = new ReactiveProperty<Exception>();

	public readonly ISubject<Exception> OnError = new Subject<Exception>();

	public readonly ISubject<string> OnAlertError = new Subject<string>();
}
