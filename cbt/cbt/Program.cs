using System;
using System.Collections.Generic;
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
            ORM orm = new ORM();

            // Debug grid
            Console.WriteLine(grid.ToString());

            // Setup domains
            orm.SetupDomains(grid);
            Console.WriteLine(grid.ToString());
        }
    }
}