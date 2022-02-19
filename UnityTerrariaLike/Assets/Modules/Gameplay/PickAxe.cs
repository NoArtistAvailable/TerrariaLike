using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace elZach.TerrariaLike
{
    public class PickAxe : MonoBehaviour
    {
        private BlockGrid _world;
        private BlockGrid World => _world ??= FindObjectOfType<BlockSpawner>().Grid;

        void OnEnable()
        {
            GetComponent<PlayerControls>().onMine += MineSingleBlock;
        }

        public void MineSingleBlock(Vector2Int pos)
        {
            World.RemoveBlock(pos.x, pos.y);
        }
    }
}
