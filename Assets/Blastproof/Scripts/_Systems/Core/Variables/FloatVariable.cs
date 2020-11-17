using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Blastproof.Systems.Core.Variables
{
    [CreateAssetMenu(menuName = "Blastproof/Variables/FloatVariable")]
    public class FloatVariable : ScriptableObject
    {
        public Action onValueChanged;

        [SerializeField, HideInInspector] protected float _val;
        [ShowInInspector]
        public virtual float Value
        {
            get => _val;  set { _val = value; onValueChanged.Fire(); }          
        }      

        public void Increment() { _val++; }
        public void Decrement() { _val--; }

        public static implicit operator float (FloatVariable fv)
        {
            return fv._val;
        }     

        protected virtual void UpdateBackingField(float newValue)
        {
            _val = newValue;
        }

    }
}
