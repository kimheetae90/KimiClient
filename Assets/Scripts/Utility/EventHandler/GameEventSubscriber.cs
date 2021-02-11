using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimiClient
{
    namespace Utility
    {
        public interface IEventSubscriber<GameEventType>
        {
            void Receive(GameEvent<GameEventType> inEvent);
        }
    }
}