using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
// using Components;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "RoundManager", menuName = "Scriptable/Manager/Round Manager")]
    public class RoundManager : ScriptableObject
    {
        [Header("Round Sequence")] 
        public float wheelDelay;
        public float wheelDuration;
        [Space] 
        public float probabilityDelay;
        public float probabilityDuration;
        [Space] 
        public float probabilityJackpotDelay;
        public float probabilityJackpotDuration;
        [Space] 
        public float moneyDelay;
        public float moneyDuration;
        [Space] 
        public float roundResetDelay;
        
        [Header("Runtime")]
        public IntReactiveProperty winNumber = new IntReactiveProperty();
        public IntReactiveProperty probabilityNumber = new IntReactiveProperty();
        public FloatReactiveProperty probabilityPla = new FloatReactiveProperty();
        public FloatReactiveProperty probabilityAfa = new FloatReactiveProperty();
        public FloatReactiveProperty probabilityTub = new FloatReactiveProperty();
        public StringReactiveProperty probabilityLetter = new StringReactiveProperty();

        public readonly ISubject<int> OnRound = new Subject<int>();
        public readonly ISubject<int> OnPayment = new Subject<int>();
        public readonly ISubject<int> OnProbability = new Subject<int>();
        public readonly ISubject<int> OnJackpot = new Subject<int>();
        public readonly ISubject<int> OnWin = new Subject<int>();
        public readonly ISubject<bool> OnReset = new Subject<bool>();
        public readonly ISubject<bool> OnClear = new Subject<bool>();
    }
}
