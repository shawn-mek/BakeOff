using System.Collections.Generic;
using System.IO;

namespace Lsr.Bio.Benchmark.ScreenScorer.ScoringSet {
	/// <summary>
	/// Interface defining the method(s) for loading scoring set info from a text source
	/// </summary>
	interface IScoringSetTextParser {
		List<ScoringSetInfo> LoadScoringSetInfo(TextReader reader);
	}
}
