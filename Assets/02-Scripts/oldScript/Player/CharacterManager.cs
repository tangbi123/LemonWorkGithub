using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lemon.Character
{
    ///<summary>
    ///人物切换或变身实现数据保存
    ///<summary>
	public class CharacterManager : MonoBehaviour
    {
        private CharacterDataTB cd;

        private void Start()
        {
            cd.InitPlayer();
        }

       
    }
}

