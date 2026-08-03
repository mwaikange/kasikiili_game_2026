using UnityEngine;

namespace Commands
{
    public class RwmProbability
    {
        /*
        RWM (Random Win Multiplier) based on the user's total balance. 
        The class is called "RwmProbability" and it contains a constructor which sets two instance variables _pla and _afa, 
        which are respectively 70% and 30% of the total user balance. There is also a function called "SelectRwm" that 
        takes in a balance value (in this case _pla) and returns an RWM value based on the input value, according to the 
        conditions specified in the if-else statements.
        */
        public float _afa;
        public float _pla;
        public string _letter;

        public RwmProbability(float tub)
        {
            // TUB is the total user balance
            // _pla represent the 70%
            _pla = tub * 0.7f;
            // afa represent the 30%
            _afa = tub * 0.3f;
            // Logs
            Debug.Log("Total user balance is " + tub);
            Debug.Log("70% PLA is " + _pla);
            Debug.Log("30% AFA is " + _afa);
        }

        // Function to select RWM based on PLA balance
        public int SelectRwm()
        {
            var rwm = 0;
            if (_pla >= 0 && _pla <= 999)
            {
                _letter = "a";
                Debug.Log("RWA is a!");
                rwm = 10;
            }
            else if (_pla >= 1000 && _pla <= 5000)
            {
                _letter = "b";
                Debug.Log("RWA is b!");
                var r = Random.Range(0f,1f);
                if(r<=0.65f)
                    rwm = 10;
                else
                    rwm = 25;
            }
            else if (_pla >= 5001 && _pla <= 20000)
            {
                _letter = "c";
                Debug.Log("RWA is c!");
                var r = Random.Range(0f,1f);
                if(r<=0.55f)
                    rwm = 10;
                else if(r<=0.85f)
                    rwm = 25;
                else
                    rwm = 50;
            }
            else if (_pla >= 20001 && _pla <= 50000)
            {
                _letter = "d";
                Debug.Log("RWA is d!");
                var r = Random.Range(0f,1f);
                if(r<=0.5f)
                    rwm = 10;
                else if(r<=0.75f)
                    rwm = 25;
                else if(r<=0.85f)
                    rwm = 50;
                else
                    rwm = 100;
            }
            else if (_pla >= 50001 && _pla <= 100000)
            {
                _letter = "e";
                Debug.Log("RWA is e!");
                var r = Random.Range(0f,1f);
                if(r<=0.45f)
                    rwm = 10;
                else if(r<=0.65f)
                    rwm = 25;
                else if(r<=0.7f)
                    rwm = 50;
                else if(r<=0.9f)
                    rwm = 100;
                else
                    rwm = 1000;
            }
            else if (_pla >= 100001 && _pla <= 180000)
            {
                _letter = "f";
                Debug.Log("RWA is f!");
                var r = Random.Range(0f,1f);
                if(r<=0.4f)
                    rwm = 10;
                else if(r<=0.55f)
                    rwm = 25;
                else if(r<=0.6f)
                    rwm = 50;
                else if(r<=0.75f)
                    rwm = 100;
                else if(r<=0.9f)
                    rwm = 1000;
                else
                    rwm = 2000;
            }
            else if (_pla >= 180001)
            {
                _letter = "g";
                Debug.Log("RWA is g!");
                var r = Random.Range(0f,1f);
                if(r<=0.35f)
                    rwm = 10;
                else if(r<=0.5f)
                    rwm = 25;
                else if(r<=0.55f)
                    rwm = 50;
                else if(r<=0.7f)
                    rwm = 100;
                else if(r<=0.85f)
                    rwm = 1000;
                else
                    rwm = 2000;
            }
            return rwm;
        }
    }
}
