using System;
using System.Linq;
using Sudoku.Model;
using Sudoku.Service;

namespace Sudoku
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setup.
            int example = 1;

            Prefabs prefabs = new Prefabs();
            Grid grid = new Grid(prefabs.GetPrefab(example));
            State state = new State(grid);
            ORM orm = new ORM();

            // Debug grid
            Console.WriteLine(state.ToString());

            int[] lookup = orm.GetColumnIndices(9);
            foreach (var i in lookup)
            {
                grid.tiles[i].value = -1;
            }
            
            Console.WriteLine(state.ToString());

        }
    }
}