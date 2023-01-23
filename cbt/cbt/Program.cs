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
            State state = new State(new Grid(prefabs.GetPrefab(example)));
        }
    }
}