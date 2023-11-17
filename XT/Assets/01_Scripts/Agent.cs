using System;
using Unity.VisualScripting;
using UnityEngine;

namespace _01_Scripts
{
    public class Agent : MonoBehaviour
    {
        [SerializeField] private float speed;
        
        private int _fi;
        private int _fj;
        private int _ti;
        private int _tj;

        public void Init(int fi, int fj, int ti, int tj)
        {
            _fi = fi;
            _fj = fj;
            _ti = ti;
            _tj = tj;
        }

        private void FixedUpdate()
        {
        }
    }
    
}