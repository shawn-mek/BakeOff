using System.Collections.Generic;

namespace Lsr.Bio.HtsAnalysis.Core {
	public interface IGetWells<T> {
		IEnumerable<Well<T>> GetWells();
	}
}
