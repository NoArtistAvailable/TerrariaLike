using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using elZach.TerrariaLike;
using UnityEngine;

[CreateAssetMenu(menuName = "TerrariaLike/Block Library")]
public class BlockLibrary : ScriptableObject
{
    [Serializable]
    public struct Map
    {
        public Block.BlockType type;
        public GameObject prefab;
    }

    [SerializeField] List<Map> blocks;

    public GameObject GetPrefab(Block.BlockType type) => blocks.FirstOrDefault(x => x.type == type).prefab;
}
