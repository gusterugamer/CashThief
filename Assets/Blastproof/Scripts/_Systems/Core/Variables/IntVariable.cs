using Sirenix.OdinInspector;
using UnityEngine;
using System;

namespace Blastproof.Systems.Core.Variables
{
    [CreateAssetMenu(menuName = "Blastproof/Variables/IntVariable")]
    public class IntVariable : ScriptableObject
    {
        public Action onValueChanged;
        
        protected int val;
        [ShowInInspector] public virtual int Value
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

        protected virtual void UpdateBackingField(int newValue)
        {
            val = newValue;
        }
    }
}