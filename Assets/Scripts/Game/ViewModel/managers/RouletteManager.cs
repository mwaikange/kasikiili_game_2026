using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Components;

namespace ViewModel
{
    [CreateAssetMenu(fileName = "New Roulette Manager", menuName = "Scriptable/Manager/Roulette Manager")]
    public class RouletteManager : ScriptableObject
    {
        public int gameLimit;
        [Header("Runtime")]
        public BoolReactiveProperty gameActive = new BoolReactiveProperty();
        public BoolReactiveProperty tableActive = new BoolReactiveProperty();
        [Header("State")]
        public RouletteState lastState;
        
        public readonly ISubject<bool> OnStart = new Subject<bool>();
        public readonly ISubject<bool> OnRefresh = new Subject<bool>();
    }   
}
