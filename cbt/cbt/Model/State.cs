using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Sudoku.Service;

namespace Sudoku.Model
{
    public class State
    {
        /// <summary>
        /// The heuristic values of each row and column. Allows partial updating based on swap.
        /// </summary>
        private int[] tentativeValuesRow, tentativeValuesColumns;
        
        /// <summary>
        /// The domain of each variable (a variable being a Cell).
        /// </summary>
        private int[] domain = Enumerable.Range(1, 9).ToArray();
        
        /// <summary>
        /// The Sudoku puzzle.
        /// </summary>
        public Grid grid;
        
        /// <summary>
        /// The total heuristic value of the current state.
        /// </summary>
        public int heuristicValue;
        
        /// <summary>
        /// Service to approach data from grid.
        /// </summary>
        public ORM orm;
        
        /// <summary>
        /// Normal constructor.
        /// </summary>
        public State(Grid grid)
        {
            this.grid = grid;
            this.tentativeValuesRow = new int[9];
            this.tentativeValuesColumns = new int[9];
            this.orm = new ORM();
        }
        
        /// <summary>
        /// Clone constructor.
        /// </summary>
        public State(State state)
        {
            this.grid = state.grid.Clone();
            this.tentativeValuesRow = (int[]) state.tentativeValuesRow.Clone();
            this.tentativeValuesColumns = (int[]) state.tentativeValuesColumns.Clone();
            this.orm = new ORM();
            this.heuristicValue = 0;
        }
        
        /// <summary>
        /// Setup the starting grid (created from prefab).
        /// </summary>
        public void Initialise()
        {
            this.orm.FillGrid(this.grid);
            this.SetHeuristicValue();
        }
        
        /// <summary>
        /// Get all operators possible from the current state,
        /// </summary>
        public List<(Cell, Cell)> GetOperators(int bX, int bY)
        {
            Cell[] block = this.orm.ReadBlock(bX, bY, this.grid);
            return this.orm.GetSwaps(block);
        }
        
        /// <summary>
        /// Count the amount of missing numbers in a collection of Cells.
        /// </summary>
        private int GetHeuristicValueElements(Cell[] cells)
        {
            int[] numbers = cells.Select(x => x.value).ToArray();
            return domain.Except(numbers).ToArray().Length;
        }
        
        /// <summary>
        /// Calculate the heuristic value of each row.
        /// </summary>
        private void SetHeuristicValueRows()
        {
            List<Cell[]> rows = this.orm.GetRows(this.grid);
            for (int i = 0; i < rows.Count(); i++)
            {
                this.tentativeValuesRow[i] = GetHeuristicValueElements(rows[i]);
            }
        }
        
        /// <summary>
        /// Calculate the heuristic value of each column.
        /// </summary>
        private void SetHeuristicValueColumns()
        {
            List<Cell[]> columns = this.orm.GetColumns(this.grid);
            for (int i = 0; i < columns.Count(); i++)
            {
                this.tentativeValuesColumns[i] = GetHeuristicValueElements(columns[i]);
            }
        }
        
        /// <summary>
        /// Calculate the total heuristic value.
        /// </summary>
        public void SetHeuristicValue()
        {
            this.SetHeuristicValueRows();
            this.SetHeuristicValueColumns();
            this.heuristicValue = this.tentativeValuesRow.Sum() + this.tentativeValuesColumns.Sum(); 
        }
        
        /// <summary>
        /// Update the heuristic value of mutated rows and columns only (more efficient).
        /// </summary>
        /// <param name="swap"></param>
        public void UpdateHeuristicValue((Cell, Cell) swap)
        {
            // Cells.
            Cell cellL = swap.Item1;
            Cell cellR = swap.Item2;
            
            // Columns.
            HashSet<int> columns = new HashSet<int>();
            columns.Add(cellL.x);
            columns.Add(cellR.x);

            foreach (int columnNumber in columns)
            {
                Cell[] column = this.orm.GetColumn(this.grid, columnNumber);
                this.tentativeValuesColumns[columnNumber] = GetHeuristicValueElements(column);
            }
            
            // Rows.
            HashSet<int> rows = new HashSet<int>();
            rows.Add(cellL.y);
            rows.Add(cellR.y);
            
            foreach (int rowNumber in rows)
            {
                Cell[] row = this.orm.GetRow(this.grid, rowNumber);
                this.tentativeValuesRow[rowNumber] = GetHeuristicValueElements(row);
            }
            
            this.heuristicValue = this.tentativeValuesRow.Sum() + this.tentativeValuesColumns.Sum(); 
        }

        /// <summary>
        /// Visualise the state.
        /// </summary>
        public override string ToString()
        {
            string result = $"H-value: {this.heuristicValue}";
            
            result += "\nH-values rows: ";
            foreach (var hValue in this.tentativeValuesRow)
            {
                result += $"{hValue}, ";
            }

            result += "\nH-values c: ";
            foreach (var hValue in this.tentativeValuesColumns)
            {
                result += $"{hValue}, ";
            }

            return result += "\n" + this.grid.ToString();
        }
    }
}