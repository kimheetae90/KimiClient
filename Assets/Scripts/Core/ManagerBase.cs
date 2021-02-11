using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimiClient
{
    namespace Core
    {
        using Utility;

        public class ManagerBase : MonoBehaviour
        {


            protected void Awake()
            {
                GameInstance.Instance.RegistManager(this);
            }

            protected void OnDestroy()
            {
                GameInstance.Instance.UnRegistManager(this);
            }

            public void ReceiveGameEvent<GameEventType>(GameEvent<GameEventType> inGameEvent)
            {

            }
        }
    }
}

