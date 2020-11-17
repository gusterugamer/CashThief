using System;
using System.Collections.Generic;
using UnityEngine.Events;
using static UIBehaviour_Generic;

namespace Blastproof.Systems.Core
{
	// Event Initializers
	[Serializable] public class UnityEvent_Bool : UnityEvent<bool> { }
	[Serializable] public class UnityEvent_Int : UnityEvent<int> { }
	[Serializable] public class UnityEvent_String : UnityEvent<string> { }
    [Serializable] public class UnityEvent_Popup : UnityEvent<string,string, List<GenericButton>> { }
}
