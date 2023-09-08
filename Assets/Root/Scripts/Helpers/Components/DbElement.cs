using Root.Scripts.Managers;
using UnityEngine;

namespace Root.Scripts.Helpers.Components
{
    public class DbElement : MonoBehaviour
    {
        public virtual void OnEnable() => ReferenceDb.Set(transform, this);

        public virtual void OnDisable() => ReferenceDb.Remove(transform);
    }
}