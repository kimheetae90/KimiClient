using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KimiClient
{
    namespace Utility
    {
        public interface IGameEventSubscriber
        {
            void Receive(GameEvent inEvent);
        }

        public interface IGameEventPublisher
        {
            void FireEvent(GameEvent inEvent);
            bool Subscribe(int inID, IGameEventSubscriber inSubscriber);
            bool UnSubscribe(int inID, IGameEventSubscriber inSubscriber);
        }
    }
}