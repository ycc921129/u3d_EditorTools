//------------------------------
// Author: yangchengchao
// Data:   2020
//------------------------------

using System;
using UnityEngine;

namespace MMYang
{
    public partial class UnityTestOne : MonoBehaviour
    {
        [SerializeField]
        private float animSpeed;
        public float AnimSpeed { get { return animSpeed; } set { animSpeed = value; } }

        [ContextMenuItem("还原","ReturnName")]
        public string name = "小黑";


        [ContextMenu("改名")] 
        void SetName()    
        {
            name = "小红";  
        }

        private void ReturnName()
        {
            name = "小黑";
        }
    }
}

