using UniRx;
using UnityEngine;

namespace ViewModel;

[CreateAssetMenu(fileName = "MenuManager", menuName = "Manager/Menu Manager", order = 0)]
public class MenuManager : ScriptableObject
{
	public ReactiveProperty<ForgetPasswordStage> currentForgetPasswordStage = new ReactiveProperty<ForgetPasswordStage>();

	public BoolReactiveProperty isOtpValid = new BoolReactiveProperty();

	public readonly ISubject<bool> OnSignUp = new Subject<bool>();

	public readonly ISubject<bool> OnOtpPopUp = new Subject<bool>();

	public readonly ISubject<bool> OnOtpSent = new Subject<bool>();

	public readonly ISubject<bool> OnOtpValid = new Subject<bool>();

	public readonly ISubject<MenuType> OnFlowChange = new Subject<MenuType>();

	public readonly ISubject<PopUp> OnSuccessPopUp = new Subject<PopUp>();
}
