using System.Collections.Generic;
using System.Linq;

namespace Lsr.Bio.HtsAnalysis.Core {
	public static class ExtensionsToIGetWells {
		public static IEnumerable<T> GetWellValues<T>(this IGetWells<T> input) {
			IEnumerable<Well<T>> wells = input.GetWells();
			IEnumerable<T> wellValues = wells.Select(x => x.Value);
			return wellValues;
		}
	}
}
