using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;

namespace elZach.TerrariaLike
{
    public class BlockGrid
    {
        [Serializable]
        public class CreationSetting
        {
            public int width = 10, height = 5;
            public PerlinSettings ground;
            public PerlinSettings stone;
        }
        
        [Serializable]
        public class PerlinSettings
        {
            public Vector2 offset;
            public float scale = 0.13457f;
            public float threshold = 0.5f;
            public bool Evaluate(float x, float y) => Mathf.PerlinNoise( offset.x + x * scale, offset.y + y * scale) > threshold;
        }
        
        public Dictionary<(int x, int y), Block> data;

        public event Action<(int x, int y)> onBlockDestroyed;

        public void InitRandom(CreationSetting settings)
        {
            data = new Dictionary<(int x, int y), Block>();
            for (int x = -settings.width; x < settings.width; x++)
            for (int y = -settings.height; y < settings.height; y++)
            {
                // Debug.Log($"{x}:{y} - {Mathf.PerlinNoise(x * randomScale, y * randomScale)}");
                if (settings.ground.Evaluate(x, y))
                    CreateBlock(x, y, Block.BlockType.Ground);
                else if (settings.stone.Evaluate(x, y))
                    CreateBlock(x, y, Block.BlockType.Stone);
            }
        }

        public void CreateBlock(int x, int y, Block.BlockType type)
        {
            var blockData = new Block(type);

            void Destroy()
            {
                var tuple = (x, y);
                onBlockDestroyed?.Invoke(tuple);
                data.Remove(tuple);
            }
            blockData.onDestroy += Destroy;
            data.Add((x, y), blockData);
        }

        public void RemoveCircle(Vector2 position, float radius)
        {
            Vector2Int max = new Vector2Int(Mathf.CeilToInt(position.x + radius), Mathf.CeilToInt(position.y + radius));
            Vector2Int min = new Vector2Int(Mathf.FloorToInt(position.x - radius), Mathf.FloorToInt(position.y - radius));
            
            for (int x = min.x; x < max.x; x++)
            for (int y = min.y; y < max.y; y++)
            {
                if (!IsInsideGrid(x, y)) continue;
                if (Vector2.Distance(position, new Vector2(x, y)) <= radius) data[(x,y)].Destroy();
            }
        }
        
        public bool IsInsideGrid(int x, int y)
        {
            return data.ContainsKey((x, y));
        }

        public void RemoveBlock(int x, int y)
        {
            if (!IsInsideGrid(x, y)) return;
            data[(x,y)].Destroy();
        }
    }
}