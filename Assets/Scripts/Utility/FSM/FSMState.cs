using System;

namespace KimiClient
{
    namespace Utility
    {
        public class FSMState<TFSMName>
        {
            public TFSMName ID;

            public Action OnEnterAction;
            public Action OnStayAction;
            public Action OnExitAction;
            public Action OnDisposeAction;

            public FSMState(TFSMName inID)
            {
                ID = inID;
            }

            public void Clear()
            {
                Dispose();

                OnEnterAction = null;
                OnStayAction = null;
                OnExitAction = null;
                OnDisposeAction = null;
            }

            protected virtual void OnEnter() { }
            internal void Enter() 
            {
                OnEnter();

                if (OnEnterAction != null)
                    OnEnterAction();
            }

            protected virtual void OnStay() { }
            internal void Stay() 
            {
                OnStay();

                if (OnStayAction != null)
                    OnStayAction();
            }

            protected virtual void OnExit() { }
            internal void Exit() 
            {
                OnExit();

                if (OnExitAction != null)
                    OnExitAction();
            }

            protected virtual void OnDispose() { }
            internal void Dispose() 
            {
                OnDispose();

                if (OnDisposeAction != null)
                    OnDisposeAction();
            }
        }
    }
}
