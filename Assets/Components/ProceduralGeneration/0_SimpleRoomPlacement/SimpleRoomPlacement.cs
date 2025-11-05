using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VTools.Grid;
using VTools.ScriptableObjectDatabase;

namespace Components.ProceduralGeneration.SimpleRoomPlacement
{
    [CreateAssetMenu(menuName = "Procedural Generation Method/Simple Room Placement")]
    public class SimpleRoomPlacement : ProceduralGenerationMethod
    {
        [Header("Room Parameters")]
        [SerializeField] private int _maxRooms = 10;
        
        protected override async UniTask ApplyGeneration(CancellationToken cancellationToken)
        {
            // Declare variables here
            int roomNumber = 0;

            for (int i = 0; i < _maxSteps; i++)
            {
                if(roomNumber >= _maxRooms)
                {
                    return;
                }
                // Check for cancellation
                cancellationToken.ThrowIfCancellationRequested();

                // Your algorithm here
                int height = RandomService.Range(2, 10);
                int width = RandomService.Range(2, 10);
                int xPos = RandomService.Range(0, 64 - width);
                int yPos = RandomService.Range(0, 64 - height);
                RectInt room = new(xPos, yPos, width, height);

                PlaceRoom(room);

                

                // Waiting between steps to see the result.
                await UniTask.Delay(GridGenerator.StepDelay, cancellationToken : cancellationToken);
            }
            
            // Final ground building.
            BuildGround();
        }

        private void PlaceRoom(RectInt _room)
        {
            if (CanPlaceRoom(_room, 10))
            {
                for (int ix = _room.xMin; ix < _room.xMax; ix++)
                {
                    for (int iy = _room.yMin; iy < _room.yMax; iy++)
                    {
                        if(!Grid.TryGetCellByCoordinates(ix, iy, out var cell))
                        {
                            continue;
                        }

                        AddTileToCell(cell, ROOM_TILE_NAME, true);
                    }
                }
            }
        }

        private void BuildGround()
        {
            var groundTemplate = ScriptableObjectDatabase.GetScriptableObject<GridObjectTemplate>("Grass");
            
            // Instantiate ground blocks
            for (int x = 0; x < Grid.Width; x++)
            {
                for (int z = 0; z < Grid.Lenght; z++)
                {
                    if (!Grid.TryGetCellByCoordinates(x, z, out var chosenCell))
                    {
                        Debug.LogError($"Unable to get cell on coordinates : ({x}, {z})");
                        continue;
                    }
                    
                    GridGenerator.AddGridObjectToCell(chosenCell, groundTemplate, false);
                }
            }
        }
    }
}