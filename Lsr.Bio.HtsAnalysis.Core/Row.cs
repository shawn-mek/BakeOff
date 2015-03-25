using System.Collections.Generic;

namespace Lsr.Bio.HtsAnalysis.Core {
	public class Row<T>: IGetWells<T> {
		private readonly Well<T>[] _VALS_BY_COL;

		public Well<T> this[int colIndex] {
			get { return this._VALS_BY_COL[colIndex]; }
		}

		public int Length {
			get { return this._VALS_BY_COL.Length; }
		}

		public Row(Well<T>[] rowValues) {
			this._VALS_BY_COL = rowValues;
		}

		public IEnumerable<Well<T>> GetWells() {
			return this._VALS_BY_COL;
		}
	}
}
