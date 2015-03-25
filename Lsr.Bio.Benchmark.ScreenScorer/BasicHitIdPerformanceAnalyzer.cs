using Lsr.Bio.HtsAnalysis.Core;

namespace Lsr.Bio.Benchmark.ScreenScorer {
	class BasicHitIdPerformanceAnalyzer : IHitIdPerformanceAnalyzer {
		public HitIdPerformance Analyze(ArrayedScreenData hitCalledData) {

			//IF(AND(TPR=1,FPR<0.1),4,
			//IF(AND(TPR>0.95,FPR<0.1),3,
			//IF(AND(TPR>0.75,FPR<0.1),2,
			//IF(AND(TPR>0.5,FPR<0.1),1,
			//IF(OR(TPR<0.1,FPR>0.2),-1,0)

			//get the values for the signal isHit
			//compare to the values for the true hits (get how)

			return  new HitIdPerformance(); //Dummy--replace with filled object
		}
	}
}
