using System;
using System.Linq;
using Blastproof.Systems.Core;
using Blastproof.Systems.Core.Variables;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Blastproof.Systems.UI
{ 
    [CreateAssetMenu(menuName = "Blastproof/Systems/UI/UISystem")]
    public class UISystem : BlastproofSystem
    {
        [BoxGroup("Info"), SerializeField] private SimpleEvent _onSystemsInitialized;
        [BoxGroup("Info"), SerializeField] private BoolVariable _loggedVariable;
        [BoxGroup("Info"), SerializeField] private UIState _loginState;
        [BoxGroup("Info"), SerializeField] private UIState _gameplayState;

        [BoxGroup("Info"), ShowInInspector, ReadOnly] public UIState CurrentState;
        //[BoxGroup("Info"), ShowInInspector, ReadOnly] public UISubState CurrentSubState;

        [HideInInspector] public Action<UIState> onStateChanged;
        //[HideInInspector] public Action<UISubState> onSubStateChanged;

        [BoxGroup("Info"), ShowInInspector, ReadOnly] private UIState lockState;
        [BoxGroup("Info"), ShowInInspector, ReadOnly] private UIState memorizedState;

        public override void Initialize()
        {
            base.Initialize();
            var referencer = FindObjectOfType<ScriptableObjectReferencer>();
            var allSubStates = referencer.scriptableObjects.FindAll(x => x is UISubState).Cast<UISubState>();
            foreach (var ss in allSubStates) ss.Reset();

            onSystemInitialized.Fire();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _onSystemsInitialized.Subscribe(OnSystemsInitialized);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _onSystemsInitialized.Unsubscribe(OnSystemsInitialized);
        }

        private void OnSystemsInitialized()
        {
            if (_loggedVariable.Value)
                _gameplayState.Activate();
            else
                _loginState.Activate();
        }

        [Button]
        public void ChangeState(UIState newState)
        {
            Debug.Log("Activating state: " + newState);
            if (newState.isPopUp)
                memorizedState = CurrentState;

            if (!newState.isPopUp && memorizedState != null)
            {
                var cacheState = memorizedState;
                memorizedState = null;
                ChangeState(cacheState);
                return;
            }
            
            if (lockState == null || lockState == newState)
            {
                CurrentState = newState;
                onStateChanged.Fire(CurrentState);

                /*
                // If content contains states, activate FirstInHierarchy sub-state
                if (CurrentState.subStates.Count > 0)
                {
                    var active = CurrentState.subStates.Where(x => x.Active).OrderBy(x => x.FirstActiveIndex).FirstOrDefault();

                    // Enable the first active one (or first in list)
                    ChangeSubstate(active ?? CurrentState.subStates[0]);
                }
                */
            }
        }

        /*
        [Button]
        public void ChangeSubstate(UISubState newSubState)
        {
            CurrentSubState = newSubState;
            onSubStateChanged.Fire(CurrentSubState);
        }
        */

        [Button]
        public override void Reset()
        {
            base.Reset();
            CurrentState = null;
            //CurrentSubState = null;
            memorizedState = null;
        }

        public void LockStateFor(UIState state)
        {
            lockState = state;
        }
    }
}