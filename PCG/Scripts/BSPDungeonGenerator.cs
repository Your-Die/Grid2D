using System.Collections.Generic;
using System.Linq;
using Chinchillada.Generation.BSP;
using Chinchillada.Foundation;
using Chinchillada.Grid;
using UnityEngine;
using Utilities.Algorithms;

namespace Chinchillada.Generation.Grid
{
    public class BSPDungeonGenerator : AsyncGeneratorComponentBase<Grid2D>
    {
        [SerializeField] private int roomValue = 1;

        [SerializeField] private Vector2Int minimumRoomSize = new Vector2Int(2, 2);

        [SerializeField, FindComponent(SearchStrategy.InChildren)]
        private BSPTreeGenerator treeGenerator;

        private readonly Dictionary<BSPTree, BoundsInt> rooms = new Dictionary<BSPTree, BoundsInt>();
        
        public override IEnumerable<Grid2D> GenerateAsync()
        {
            this.rooms.Clear();
            
            // Construct empty grid.
            var tree = this.treeGenerator.Generate();
            var grid = ConstructGrid(tree);
            yield return grid;

            // Recurse over the tree to generate the dungeon.
            foreach (var gridState in this.GenerateDungeon(tree, grid))
                yield return gridState;
        }
        
        protected override Grid2D GenerateInternal() => this.GenerateAsync().Last();

        private IEnumerable<Grid2D> GenerateDungeon(BSPTree tree, Grid2D grid)
        {
            if (tree.IsLeafNode)
            {
                yield return this.PlaceRoom(tree, grid);
                yield break;
            }

            foreach (var gridState in this.GenerateDungeon(tree.FirstChild, grid))
                yield return gridState;

            foreach (var gridState in this.GenerateDungeon(tree.SecondChild, grid))
                yield return gridState;

            yield return this.Connect(tree.FirstChild, tree.SecondChild, grid);
        }

        private Grid2D PlaceRoom(BSPTree tree, Grid2D grid)
        {
            // Get the maximum lower bounds.
            var bounds = tree.Bounds;
            var leftMax = bounds.xMax - this.minimumRoomSize.x;
            var topMax = bounds.yMax - this.minimumRoomSize.y;

            // Choose lower bounds.
            var left = Random.Range(bounds.xMin, leftMax + 1);
            var top = Random.Range(bounds.yMin, topMax + 1);

            // Get the minimum upper bounds. (minus one is because the lower bounds are inclusive.)
            var rightMin = left + this.minimumRoomSize.x - 1;
            var bottomMin = top + this.minimumRoomSize.y - 1;

            // Choose upper bounds.
            var right = Random.Range(rightMin, bounds.xMax);
            var bottom = Random.Range(bottomMin, bounds.yMax);
            
            Debug.Log($"Placing room: x: ({left} - {right}) y: ({top} - {bottom})");
            this.rooms[tree] = new BoundsInt
            {
                xMin = left,
                xMax = right + 1,
                yMin = top,
                yMax = bottom + 1
            };
            
            // Place room.
            for (var x = left; x <= right; x++)
            for (var y = top; y <= bottom; y++)
                grid[x, y] = this.roomValue;
            
            return grid;
        }

        private Grid2D Connect(BSPTree segment1, BSPTree segment2, Grid2D grid)
        {
            // Get rooms.
            var room1 = this.rooms.TryGetValue(segment1, out var segment1Room) ? segment1Room : segment1.Bounds;
            var room2 = this.rooms.TryGetValue(segment2, out var segment2Room) ? segment2Room : segment2.Bounds;

            // Get a path between the rooms.
            var connectionProblem = new RoomProblem(room1, room2, grid, this.roomValue);
            var path = Search.AStar(connectionProblem);

            // Draw the path.
            foreach (var position in path) 
                grid[position] = this.roomValue;

            return grid;
        }

        private static Grid2D ConstructGrid(BSPTree tree)
        {
            var size = tree.Bounds.size;
            return new Grid2D(size.x, size.y);
        }
        
        private class RoomProblem : ISearchProblem<Vector2Int>
        {
            private readonly BoundsInt originRoom;
            private readonly BoundsInt targetRoom;
            
            private readonly Grid2D grid;
            private readonly int roomValue;

            private Vector2Int targetCenter;
            
            public Vector2Int InitialState { get; }

            public RoomProblem(BoundsInt originRoom, BoundsInt targetRoom, Grid2D grid, int roomValue)
            {
                this.originRoom = originRoom;
                this.targetRoom = targetRoom;
                this.grid = grid;
                this.roomValue = roomValue;

                this.InitialState = (Vector2Int) this.originRoom.GetCenterInt();
                this.targetCenter = (Vector2Int) this.targetRoom.GetCenterInt();
            }

            public float CalculateHeuristic(Vector2Int state) => Vector2Int.Distance(state, this.targetCenter);

            public bool IsGoalState(Vector2Int state) => this.targetRoom.Contains2D(state) && 
                                                         this.grid[state] == this.roomValue;

            public IEnumerable<SearchNode<Vector2Int>> GetSuccessors(Vector2Int state)
            {
                yield return this.CreateAction(state.x - 1, state.y);
                yield return this.CreateAction(state.x, state.y - 1);
                yield return this.CreateAction(state.x + 1, state.y);
                yield return this.CreateAction(state.x, state.y + 1);
            }

            private SearchNode<Vector2Int> CreateAction(int x, int y)
            {
                var position = new Vector2Int(x, y);
                var cost = this.GetCost(position);

                return new SearchNode<Vector2Int>(position, cost);
            }

            private float GetCost(Vector2Int position)
            {
                if (!this.grid.Contains(position))
                    return 1;

                return this.grid[position] == this.roomValue ? 0 : 1;
            }
        }
    }
}