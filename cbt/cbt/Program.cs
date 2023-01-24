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

            // Debug grid
            Console.WriteLine(grid.ToString());
        }
    }
}