using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using static UIBehaviour_Generic;

namespace Blastproof.Systems.Core
{
    /*
		An event that may be fired inside the Blastproof.Systems namespace
	*/
    [CreateAssetMenu(menuName = "Blastproof/Events/PopupEvent")]
    public class PopupEvent : ScriptableObject
    {
        // The event 
        [BoxGroup("Event"), ShowInInspector, ReadOnly] private UnityEvent_Popup eventHappen = new UnityEvent_Popup();

        // What listeners are searching for it
        [BoxGroup("Listeners"), ShowInInspector, ReadOnly] private List<UnityAction<string, string, List<GenericButton>>> listeners = new List<UnityAction<string, string, List<GenericButton>>>();

        // The last value the event was invoked with
        [BoxGroup("Info"), SerializeField, ReadOnly] private int _lastValue;

        // The event invocation method
        public void Invoke(string title, string message, List<GenericButton> buttons)
        { eventHappen.Invoke(title, message, buttons); }

        // Ways to subscribe/unsubscribe to it
        public void Subscribe(UnityAction<string, string, List<GenericButton>> action)
        { listeners.Add(action); eventHappen.AddListener(action); }

        public void Unsubscribe(UnityAction<string, string, List<GenericButton>> action)
        { listeners.Remove(action); eventHappen.RemoveListener(action); }
    }
}
