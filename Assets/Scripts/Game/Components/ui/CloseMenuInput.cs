using System.Collections;
using System.Collections.Generic;
using Commands;
using UnityEngine;
using ViewModel;

namespace Components
{
    public class CloseMenuInput : MonoBehaviour
    {
        public RouletteManager rouletteManager; 
        public TableManager tableManager;
        public AudioManager audioManager;
        public RouletteCmdFactory rouletteCmdFactory;
        public GameObject exitMenu;

        public void OnClick()
        {
            var credits = tableManager.cashManager.currentCredit.Value;
            if (credits >= rouletteManager.gameLimit)
            {
                rouletteCmdFactory.TurnRouletteState(rouletteManager, audioManager, RouletteState.Cashout).Execute();
            }
            else
            {
                rouletteCmdFactory.TurnRouletteState(rouletteManager, audioManager, RouletteState.Game).Execute();
            }
            exitMenu.SetActive(false);
        }
    }
}
