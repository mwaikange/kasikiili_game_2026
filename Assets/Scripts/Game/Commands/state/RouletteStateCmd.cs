using System;
using UnityEngine;
using ViewModel;

namespace Commands
{
    public class RouletteStateCmd : ICommand
    {
        private readonly RouletteManager _rouletteManager;
        private readonly AudioManager _audioManager;
        private readonly RouletteState _rouletteState;

        public RouletteStateCmd(RouletteManager rouletteManager, 
            AudioManager audioManager, RouletteState rouletteState)
        {
            _rouletteManager = rouletteManager;
            _audioManager = audioManager;
            _rouletteState = rouletteState;
        }

        public void Execute()
        {
            switch (_rouletteState)
            {
                case RouletteState.Game:
                    Play();
                    break;
                case RouletteState.Pause:
                    if (_rouletteManager.lastState != RouletteState.Game) return;
                    Pause();
                    break;
                case RouletteState.Cashout:
                    if (_rouletteManager.lastState != RouletteState.Game) return;
                    Cashout();
                    break;
                case RouletteState.Unavailable:
                    if (_rouletteManager.lastState != RouletteState.Game) return;
                    Unavailable();
                    break;
                default:
                    Debug.LogError("Not implemented scene! Please scene need to be valid to change.");
                    break;
            }
            _rouletteManager.lastState = _rouletteState;
        }

        private void Unavailable()
        {
            _audioManager.masterVolume.Value = 0.0045f;
            _rouletteManager.gameActive.Value = false;
            _rouletteManager.tableActive.Value = false;
        }

        private void Cashout()
        {
            _audioManager.masterVolume.Value = 0.0045f;
            _rouletteManager.gameActive.Value = false;
            _rouletteManager.tableActive.Value = false;
        }

        private void Pause()
        {
            _audioManager.masterVolume.Value = 0.0045f;
            _rouletteManager.gameActive.Value = false;
            _rouletteManager.tableActive.Value = false;
        }

        private void Play()
        {
            _audioManager.masterVolume.Value = _audioManager.masterDefault;
            _rouletteManager.gameActive.Value = true;
            _rouletteManager.tableActive.Value = true;
        }
    }
}