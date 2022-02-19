using System;
using UnityEngine;

namespace elZach.TerrariaLike
{
    public class Block
    {
        public enum BlockType
        {
            Empty = 0,
            Ground = 1,
            Stone = 2,
            Ore = 3,
            Tree = 4
        }
        
        public Block(){}

        public Block(BlockType blockType)
        {
            block = blockType;
        }

        public BlockType block;
        public GameObject spawnedBlock;

        public event Action onDestroy;

        public void Destroy()
        {
            if(spawnedBlock) spawnedBlock.Despawn();
            onDestroy?.Invoke();
        }
    }
}