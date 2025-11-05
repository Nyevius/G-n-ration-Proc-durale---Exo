using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;
using VTools.Utility;

namespace Components.ProceduralGeneration.SimpleRoomPlacement
{
    [CreateAssetMenu(menuName = "Procedural Generation Method/Cellular Automata")]
    public class CellularAutomata : ProceduralGenerationMethod
    {
        [SerializeField] private float _noiseDensity = 0.5f;
        private Dictionary<Vector2Int, string> tmpGrid = new();

        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            CreateNoiseGrid();
            for (int i = 0; i < _maxSteps; i++)
            {

                Arrange();
                Replace();
                tmpGrid.Clear();
                await UniTask.Delay(200);
            }
        }

        public void CreateNoiseGrid()
        {
            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    if (!Grid.TryGetCellByCoordinates(x, y, out var cell))
                        continue;
                    bool isWater = RandomService.Chance(0.5f);
                    if (isWater)
                    {
                        AddTileToCell(cell, WATER_TILE_NAME, true);
                    }
                    else
                    {
                        AddTileToCell(cell, GRASS_TILE_NAME, true);
                    }
                }
            }
        }
        public void Arrange()
        {
            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    if (!Grid.TryGetCellByCoordinates(x, y, out var cell))
                        continue;
                    int voidCell = 0;
                    int grassCell = 0;
                    for (int up = -1; up <= 1; up++)
                    {
                        for (int down = -1; down <= 1; down++)
                        {
                            if (!Grid.TryGetCellByCoordinates(x + up, y + down, out var nextCell))
                            {

                                voidCell++;
                            }
                            else
                            {


                                if (nextCell.GridObject.Template.Name == GRASS_TILE_NAME)
                                {
                                    grassCell++;
                                }
                            }
                        }
                    }
                    Debug.Log(voidCell);
                    if (voidCell == 0) // normal
                    {
                        if (grassCell >= 4)
                        {
                            tmpGrid.Add(new Vector2Int(x, y), GRASS_TILE_NAME);
                        }
                        else
                        {
                            tmpGrid.Add(new Vector2Int(x, y), WATER_TILE_NAME);
                        }
                    }
                    else if (voidCell == 3) // border
                    {
                        if (grassCell >= 3)
                        {
                            tmpGrid.Add(new Vector2Int(x, y), GRASS_TILE_NAME);
                        }
                        else
                        {
                            tmpGrid.Add(new Vector2Int(x, y), WATER_TILE_NAME);
                        }
                    }
                    else // corner
                    {
                        if (grassCell >= 2)
                        {
                            tmpGrid.Add(new Vector2Int(x, y), GRASS_TILE_NAME);
                        }
                        else
                        {
                            tmpGrid.Add(new Vector2Int(x, y), WATER_TILE_NAME);
                        }
                    }
                }
            }
        }
        private void Replace()
        {
            for (int x = 0; x < 64; x++)
            {
                for (int y = 0; y < 64; y++)
                {
                    if (!Grid.TryGetCellByCoordinates(x, y, out var cell))
                        continue;

                    string tile = tmpGrid[new Vector2Int(x, y)];
                    AddTileToCell(cell, tile, true);
                }
            }
        }
    }
}