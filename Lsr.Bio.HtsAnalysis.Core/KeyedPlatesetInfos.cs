using System;

namespace Lsr.Bio.HtsAnalysis.Core {
	public class KeyedPlatesetInfos<T>: KeyedIPlatesetInfos {
		/// <summary>
		/// Constructor that adds the input PlatesetInfos to the dictionary, keyed by their names, after validating
		/// that they all have the same plate, row, and column dimensions.
		/// </summary>
		/// <param name="inputPlatesets">A param array containing instantiated PlatesetInfos with data of type T</param>
		public KeyedPlatesetInfos(params PlatesetInfo<T>[] inputPlatesets) {
			foreach (PlatesetInfo<T> currPlateset in inputPlatesets) {
				this.Add(currPlateset);
			} //next plateset
		}

		/// <summary>
		/// Method that adds the input PlatesetInfo to the dictionary, keyed by its name, after validating
		/// that it has the same plate, row, and column dimensions as any PlatesetInfos already in the dictionary.
		/// </summary>
		/// <param name="iplatesetInfo">An instantiated PlatesetInfo with data of type T</param>
		public new void Add(IPlatesetInfo iplatesetInfo) {
			if (iplatesetInfo == null) { throw new ArgumentException("Input IPlatesetInfo may not be null"); }

			if (iplatesetInfo.CastToPlatesetInfo<T>() == null) {
				throw new ArgumentException("Input IPlatesetInfo is not of the same type as this KeyedPlatesetInfos object");
			}

			base.Add(iplatesetInfo);
		}

		public new PlatesetInfo<T> this[string key] {
			get { return this.InternalKeyedIPlatesetInfos[key].CastToPlatesetInfo<T>(); }
			set { this.InternalKeyedIPlatesetInfos[key] = value; }
		}
	}
}
