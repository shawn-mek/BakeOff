using System.Collections.Generic;

namespace Lsr.Bio.HtsAnalysis.Core {
	public class Plate<T> : IGetWells<T> {
		#region members
		/// <summary>
		/// Rectangular array of plate/row/columns holding data of type T for each well position
		/// </summary>
		private readonly Well<T>[][] _VALS_BY_ROW_COL;
		#endregion members

		#region properties
		public int NumRows {
			get { return this._VALS_BY_ROW_COL.Length; }
		}

		public int NumColumns {
			//TODO: refactor to deal with case where plate is empty
			get { return this._VALS_BY_ROW_COL[0].Length; }
		}

		/// <summary>
		/// Indexer returning the data value, of type T, for the input plate index, row index, and column index
		/// </summary>
		/// <param name="rowIndex">A non-negative number indicating the 0-based index of the row for which to
		/// return data</param>
		/// <param name="colIndex">A non-negative number indicating the 0-based index of the column for which to
		/// return data</param>
		/// <returns>The data value, of type T, for the input plate index, row index, and column index</returns>
		/// <remarks>The requested value is returned even if it has a value of true in _IgnoreDataAtPosition</remarks>
		public Well<T> this[int rowIndex, int colIndex] {
			get { return this._VALS_BY_ROW_COL[rowIndex][colIndex]; }
			set { this._VALS_BY_ROW_COL[rowIndex][colIndex] = value; }
		}
		#endregion properties

		#region constructors
		public Plate(int numRows, int numColumns) {
			this._VALS_BY_ROW_COL = Utility.InstantiateArray<T>(numRows, numColumns);
		}
		#endregion constructors

		public Row<T> GetRow(int rowIndex) {
			return new Row<T>(this._VALS_BY_ROW_COL[rowIndex]);
		}

		public Column<T> GetColumn(int colIndex) {
			List<Well<T>> colValues = new List<Well<T>>();
			for (int i = 0; i < this._VALS_BY_ROW_COL.Length; i++) {
				colValues.Add(this._VALS_BY_ROW_COL[i][colIndex]);
			}
			return new Column<T>(colValues);
		}

		public IEnumerable<Well<T>> GetWells() {
			return HtsAnalysis.Core.Utility.Flatten(this._VALS_BY_ROW_COL);
		}  
	}
}
