using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace Sudoku.Model
{
    public struct Cell : ICloneable
    {
        /// <summary>
        /// The actual number stored in the sudoku puzzle.
        /// </summary>
        public int value;
        
        /// <summary>
        /// Indicator that specifies whether the cell is fixed.
        /// </summary>
        public bool set;

        /// <summary>
        /// Cell saves their index relative to the grid.
        /// </summary>
        public int index;

        /// <summary>
        /// Domain of variable.
        /// </summary>
        public HashSet<int> domain;

        public Cell(int value, int index, bool set, HashSet<int> domain = null)
        {
            this.value = value;
            this.set = set;
            this.index = index;
            this.domain = domain ?? Enumerable.Range(1, 9).ToHashSet();
        }
        
        /// <summary>
        /// Deep clone a Cell.
        /// </summary>
        public Object Clone()
        {
            HashSet<int> newDomain = new HashSet<int>(this.domain);
            return new Cell(this.value, this.index, this.set, newDomain);
        }

        public bool ApplyDomainConsistency(int constraintValue)
        {
            if (this.set)
            {
                return true;
            }
            
            this.domain.Remove(constraintValue);
            
            if (this.DomainIsEmpty())
            {
                return false;
            }

            return true;

        }

        public bool DomainIsEmpty()
        {
            return !this.domain.Any();
        }

        public int? PopDomain()
        {
            if (this.DomainIsEmpty())
            {
                return null;
            }
            
            int domainValue = this.domain.First();
            this.domain.Remove(domainValue);
            return domainValue;
        }

        public void WriteValue(int newValue)
        {
            this.value = newValue;
        }

        public override string ToString()
        {
            return $"({this.index}) {this.value} - domainCount: {this.domain.Count}";
        }
    }
}