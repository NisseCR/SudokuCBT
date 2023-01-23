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
            int example = 5;

            Prefabs prefabs = new Prefabs();
            Grid grid = new Grid(prefabs.GetPrefab(1));
            ORM orm = new ORM();
            
            // Debug grid
            Console.WriteLine(grid.ToString());
            
            // debug
            var block = orm.ReadBlock(0, 0, grid);
            var row = orm.GetRow(grid, 0);
            var column = orm.GetColumn(grid, 0);
            
            // toekennen 4 links boven
            // --> fc neighbours worden consistent
            // --> aanpassen domain van alle neighbours
            // --> indices van neigbours
            // ^ 
            
            // fw empty domain
            // 
            
            
            // TODO
            // refactor datastructure for grid:
            //  - Ez acces to row, column and block indices
            //  - Fast cloning
            // Becomes a biiiig array of 81 elements 
            // Thomas
            
            // DFS w/ backtracking front and neighbour selection
            // 2 cases where backtracking is required:
            //  - Not a partial solution (mayhaps not need)
            //  - Empty domain in forward checking
            // Mikey
            
            
            // Forward checking function with given indices
            // Thomas
            
            // cloning and hashset refactor
            // Nisse
            
            // DFS
        }
    }
}