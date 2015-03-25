using System.Collections.Generic;
using System.Linq;

namespace Lsr.Bio.HtsAnalysis.Core {
	public class Plateset<T>: IGetWells<T> {
		private readonly Plate<T>[] _PLATES;

		public Plate<T> this[int plateIndex] {
			get { return this._PLATES[plateIndex]; }
		}

		public Well<T> this[int plateIndex, int rowIndex, int colIndex] {
			get { return this._PLATES[plateIndex][rowIndex, colIndex]; }
			set { this._PLATES[plateIndex][rowIndex, colIndex] = value; }
		}

		public int Length {
			get { return this._PLATES.Length; }
		}

		public IEnumerable<Well<T>> GetWells() {
			Well<T>[][] partiallyFlattened = new Well<T>[this.Length][];

			for (int i = 0; i < this.Length; i++) {
				Plate<T> currPlate = this[i];
				partiallyFlattened[i] = currPlate.GetWells().ToArray();
			}

			return HtsAnalysis.Core.Utility.Flatten(partiallyFlattened);
		}

		public Plateset(int numPlates, int numRows, int numColumns) {
			this._PLATES = new Plate<T>[numPlates];
			for (int i = 0; i < numPlates; i++) {
				this._PLATES[i] = new Plate<T>(numRows, numColumns);
			}
		}
	}
}
