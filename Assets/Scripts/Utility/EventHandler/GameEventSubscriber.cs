using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimiClient
{
    namespace Utility
    {
        public interface IEventSubscriber
        {
            void Receive(GameEvent inEvent);
        }
    }
}