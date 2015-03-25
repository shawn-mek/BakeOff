using System.Collections.Generic;

namespace Lsr.Bio.HtsAnalysis.Core {
	public class Column<T>: IGetWells<T> {
		private readonly Well<T>[] _VALS_BY_ROW;

		public int Length {
			get { return this._VALS_BY_ROW.Length; }
		}

		public Well<T> this[int rowIndex] {
			get { return this._VALS_BY_ROW[rowIndex]; }
		}

		public Column(Well<T>[] columnValues) {
			this._VALS_BY_ROW = columnValues;
		}

		public Column(IList<Well<T>> columnValues) {
			this._VALS_BY_ROW = new Well<T>[columnValues.Count];
			columnValues.CopyTo(this._VALS_BY_ROW, 0);
		}

		public IEnumerable<Well<T>> GetWells() {
			return this._VALS_BY_ROW;
		}
	}

}
