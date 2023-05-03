using UnityEngine;
using Random = UnityEngine.Random;

namespace Blocks
{
    public class TurnBlock: BaseBlock
    {
        public override BlockType Type => BlockType.TurnBlock;

        private Player _player;

        public override void OnSpawned()
        {
            var r = Random.Range(0, 2);
            if (r == 0)
            {
                transform.rotation *= Quaternion.Euler(0,90,0);
            }
            else
            {
                transform.rotation *= Quaternion.Euler(0,-90,0);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            _player = other.GetComponent<Player>();
            _player.isRunning = false;
        }

        private void OnTriggerStay(Collider other)
        {
            other.transform.rotation = 
                Quaternion.Lerp(other.transform.rotation, transform.rotation, Time.deltaTime * 30);
            other.transform.position =
                Vector3.Lerp(other.transform.position, transform.position + transform.forward, Time.deltaTime * 15);
        }

        private void OnTriggerExit(Collider other)
        {
            other.transform.rotation = transform.rotation;
            other.transform.position = transform.position + transform.forward;
            
            _player.isRunning = true;
            enabled = false;
        }
    }
}