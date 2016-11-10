using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.UI.Grid;

namespace SSMScripter.Integration
{
    class ResultGrid : IResultGrid
    {
        private readonly IGridControl _grid;

        public ResultGrid(IGridControl grid)
        {
            _grid = grid;
        }

        public string GetSelectedValue()
        {
            BlockOfCellsCollection cells = _grid.SelectedCells;

            if (cells.Count == 0)
                return null;

            BlockOfCells cellsSet = cells[0];
            IGridStorage storage = _grid.GridStorage;

            return storage.GetCellDataAsString(cellsSet.Y, cellsSet.X);
        }
    }
}
