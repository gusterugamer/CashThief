using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Blastproof.Systems.Core.Variables
{
    [CreateAssetMenu(menuName = "Blastproof/Variables/IntVariable")]
    public class FloatVariable : ScriptableObject
    {
        public Action onValueChanged;

        protected float val;
        [ShowInInspector]
        public virtual float Value
        {
            get => val;
            set
            {
                UpdateBackingField(value);
                onValueChanged.Fire();
            }
        }      

        public void Increment() { Value++; }
        public void Decrement() { Value--; }

        protected virtual void UpdateBackingField(float newValue)
        {
            val = newValue;
        }
    }
}
