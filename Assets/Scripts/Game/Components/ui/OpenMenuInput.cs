using System.Collections;
using System.Collections.Generic;
using Commands;
using UnityEngine;
using ViewModel;

namespace Components
{
    public class OpenMenuInput : MonoBehaviour
    {
        public RouletteManager rouletteManager; 
        public AudioManager audioManager;
        public RouletteCmdFactory rouletteCmdFactory;
        public GameObject exitMenu;

        public void OnClick()
        {
            Debug.Log("OpenMenuInput");
            rouletteCmdFactory.TurnRouletteState(rouletteManager, audioManager, RouletteState.Pause).Execute();
            exitMenu.SetActive(true);
        }
    }
}
