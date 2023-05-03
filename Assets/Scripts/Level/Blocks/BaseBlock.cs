using UnityEngine;

namespace Blocks
{
    public abstract class BaseBlock: MonoBehaviour
    {
        public Transform savePos;
        public abstract BlockType Type { get; }

        public virtual void OnSpawned()
        {
            
        }
    }
}