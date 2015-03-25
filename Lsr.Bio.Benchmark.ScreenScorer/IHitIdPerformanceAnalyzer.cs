using Lsr.Bio.HtsAnalysis.Core;

namespace Lsr.Bio.Benchmark.ScreenScorer {
	interface IHitIdPerformanceAnalyzer {
		HitIdPerformance Analyze(ArrayedScreenData hitCalledData);
	}
}
