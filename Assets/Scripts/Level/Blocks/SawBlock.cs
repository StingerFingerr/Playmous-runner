using System;
using UnityEditor;
using UnityEngine;

namespace Blocks
{
    public class SawBlock: BaseBlock
    {
        [SerializeField] private GameObject saw;
        public override BlockType Type => BlockType.SawBlock;

        private void Update()
        {
            saw.transform.Rotate(Vector3.up, 100 * Time.deltaTime);
        }
    }
}