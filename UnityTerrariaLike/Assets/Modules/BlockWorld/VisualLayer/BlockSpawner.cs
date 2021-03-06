using System.Collections.Generic;
using UnityEngine;

namespace elZach.TerrariaLike
{
    public class BlockSpawner : MonoBehaviour
    {
        public BlockGrid Grid;
        Dictionary<Vector2Int, Block> grid => Grid.data;
        public BlockGrid.CreationSetting worldSettings;
        public Transform player;
        public Vector2Int playerExtents = new Vector2Int(20, 15);
        public BlockLibrary library;
        public List<Vector2Int> inView = new List<Vector2Int>();

        void Start()
        {
            Grid = new BlockGrid();
            Grid.InitRandom(worldSettings);
            Grid.onBlockDestroyed += OnBlockRemoved;
            Debug.Log(Grid.data.Count);
            //Cut a hole for a valid starting position
            Grid.RemoveCircle(Vector2.zero,4.5f);
        }
        
        void Update()
        {
            var playerPos = new Vector2Int(Mathf.RoundToInt(player.position.x), Mathf.RoundToInt(player.position.y));
            var removeList = new List<Vector2Int>(inView);  //copy the list of inview
            for (int x = -playerExtents.x + playerPos.x; x < playerExtents.x + playerPos.x; x++)
            for (int y = -playerExtents.y + playerPos.y; y < playerExtents.y + playerPos.y; y++)
            {
                var pos = new Vector2Int(x, y);
                if (removeList.Contains(pos)) removeList.Remove(pos); //we remove everything that is in view
                if (!Grid.IsInsideGrid(x, y)) continue;
                var data = grid[pos];
                if (data.spawnedBlock) continue;    //if we already spawned the block, we can ignore it
                var block = library.GetPrefab(data.block).Spawn(transform);
                block.transform.position = new Vector3(x, y, 0f);
                data.spawnedBlock = block;
                inView.Add(pos);
            }
            // so now we instantiated everything in view - let's clean up what we probably don't see anymore

            for (int i = removeList.Count - 1; i >= 0; i--)
            {
                var pos = removeList[i];
                inView.Remove(pos);
                grid[(pos)].spawnedBlock.Despawn();
                grid[(pos)].spawnedBlock = null;
            }
        }

        void OnBlockRemoved(Vector2Int pos)
        {
            inView.Remove(pos);
        }
        

        bool isInsideView(Vector2Int playerPos, int x, int y)
        {
            if (x < -playerExtents.x + playerPos.x || x >= playerExtents.x + playerPos.x) return false;
            if (y < -playerExtents.y + playerPos.y || y >= playerExtents.y + playerPos.y) return false;
            return true;
        }
    }
}