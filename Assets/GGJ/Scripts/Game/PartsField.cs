using GGJ.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ.Game
{
    public class PartsField : SingletonMonoBehaviour<PartsField>
    {
        protected override bool dontDestroyOnLoad { get { return false; } }
    }
}
