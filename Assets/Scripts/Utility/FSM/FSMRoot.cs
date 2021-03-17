using System;
using System.Collections.Generic;
using UnityEngine;

namespace KimiClient
{
    namespace Utility
    {
        public class FSMRoot<TFSMName>
        {
            public FSMState<TFSMName> CurrentState { get; private set; }            

            public Action OnRunAction;
            public Action OnUpdateAction;
            public Action OnStopAction;

            public bool bRun { get; private set; }
            public int StateCount { get { return stateList.Count; } }

            private List<FSMState<TFSMName>> stateList;

            protected virtual void OnRun() { }
            public bool Run(TFSMName inStartID)
            {
                FSMState<TFSMName> state = GetState(inStartID);
                if (state == null)
                {
                    Debug.LogWarning("[FSMRoot]Run State is Null! key : " + inStartID);
                    return false;
                }

                if(bRun)
                {
                    Debug.LogWarning("[FSMRoot]Run Already Run! key : " + inStartID);
                    return false;
                }

                CurrentState = state;

                bRun = true;

                OnRun();

                if (OnRunAction != null)
                    OnRunAction();

                if (CurrentState != null)
                    CurrentState.Enter();

                return true;
            }

            protected virtual void OnUpdate() { }
            public bool Update()
            {
                if (!bRun)
                    return false;

                OnUpdate();

                if (OnUpdateAction != null)
                    OnUpdateAction();

                if(CurrentState != null)
                    CurrentState.Stay();

                return true;
            }

            protected virtual void OnStop() { }
            public bool Stop()
            {
                if (!bRun)
                    return false;

                OnStop();

                if (OnStopAction != null)
                    OnStopAction();

                if (CurrentState != null)
                    CurrentState.Exit();

                bRun = false;

                return true;
            }

            public void Clear()
            {
                if (bRun)
                {
                    Stop();
                }

                foreach (var state in stateList)
                {
                    state.Clear();
                }

                CurrentState = null;

                OnRunAction = null;
                OnUpdateAction = null;
                OnStopAction = null;
            }

            public bool AddState(FSMState<TFSMName> inNewState)
            {
                if(inNewState == null)
                {
                    Debug.LogWarning("[FSMRoot]AddState inNewState is Null!");
                    return false;
                }

                if (stateList == null)
                {
                    stateList = new List<FSMState<TFSMName>>();
                }

                if(GetState(inNewState.ID) != null)
                {
                    Debug.LogWarning("[FSMRoot]AddState Same Id Already Contain! ID : " + inNewState.ID);
                    return false;
                }

                stateList.Add(inNewState);

                if (CurrentState == null)
                {
                    CurrentState = inNewState;
                }

                return true;
            }

            public bool Transition(TFSMName inID)
            {
                if (CurrentState != null)
                {
                    CurrentState.Exit();                    
                }

                FSMState<TFSMName> transitState = GetState(inID);
                if(transitState == null)
                {
                    Debug.LogWarning("[FSMRoot]Transition transitState is Null! ID : " + inID);
                    return false;
                }

                CurrentState = transitState;
                if(CurrentState != null)
                {
                    CurrentState.Enter();
                }

                return true;
            }

            public FSMState<TFSMName> GetState(TFSMName key)
            {
                if (stateList == null)
                {
                    Debug.LogWarning("[FSMRoot]GetState stateList is Null!");
                    return null;
                }

                return stateList.Find(x => x.ID.Equals(key));
            }
        }
    }
}