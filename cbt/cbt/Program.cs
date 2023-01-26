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
            int example = 3;

            Prefabs prefabs = new Prefabs();
            Grid grid = new Grid(prefabs.GetPrefab(example));
            ORM orm = new ORM();
            CBT cbt = new CBT();
            State state = new State(grid);

            // Setup domains
            orm.SetupDomains(grid);

            // Run CBT
            Console.WriteLine($"Example: {example}");
            Console.WriteLine($"Start Grid: {state.grid}");
            cbt.Run(state, orm);
        }
    }
}