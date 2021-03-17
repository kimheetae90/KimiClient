using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimiClient
{
    namespace Core
    {
        using Utility;

        public class GameInstance : MonoBehaviour
        {
            public static GameInstance Instance;
            private List<ManagerBase> managerList;

            protected void Awake()
            {
                Instance = this;
                Initialize();
            }

            public void Initialize()
            {
                managerList = new List<ManagerBase>();
            }

            public bool RegistManager(ManagerBase inManager)
            {
                if (managerList.Contains(inManager))
                {
                    Debug.LogError("GameInstnace already contains ManagerBase : " + inManager);
                    return false;
                }

                managerList.Add(inManager);
                return true;
            }

            public bool UnRegistManager(ManagerBase inManager)
            {
                if (!managerList.Contains(inManager))
                {
                    Debug.LogError("GameInstnace doesn't contains ManagerBase : " + inManager);
                    return false;
                }

                managerList.Remove(inManager);
                return true;
            }

            public void PublishGameEvent(GameEvent inGameEvent)
            {
            }
        }
    }
}