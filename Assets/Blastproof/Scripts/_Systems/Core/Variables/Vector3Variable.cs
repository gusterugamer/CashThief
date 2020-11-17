using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace Blastproof.Systems.Core.Variables
{
    [CreateAssetMenu(menuName = "Blastproof/Variables/Vector3Variable")]
    public class Vector3Variable : ScriptableObject
    {
        public Action onValueChanged;

        protected Vector3 _vec;
        [ShowInInspector]
        public virtual Vector3 Value
        {
            get => _vec; set { _vec = value; onValueChanged.Fire(); }
        }   

        public static implicit operator Vector3(Vector3Variable fv)
        {
            return fv._vec;
        }

        protected virtual void UpdateBackingField(Vector3 newVector)
        {
            _vec = newVector;
        }
    }
}
