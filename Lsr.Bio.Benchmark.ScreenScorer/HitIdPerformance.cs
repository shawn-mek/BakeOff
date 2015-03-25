using System.Collections.Generic;
using Lsr.Bio.Benchmark.Utilities;
using Lsr.Bio.HtsAnalysis.Workflows;

namespace Lsr.Bio.Benchmark.ScreenScorer {
	class HitIdPerformance {
		public int ScreenId { get; set; }
		public int HitSetId { get; set; }
		public int NoiseSetId { get; set; }
		public ScoringWorkflowInfo ScoringWorkflow { get; set; }
		public Dictionary<string, HitTypePerformance> HitTypePerformances { get; set; }


		public override string ToString() {
			string result = string.Join(InfoTextParser.DELIMITER, ScreenId, HitSetId, NoiseSetId, ScoringWorkflow.ToString(),
				ScoringWorkflow.NormalizationMethod, ScoringWorkflow.ReplicateCombBeforeScoringMethod,
				ScoringWorkflow.ScoringMethod, ScoringWorkflow.ReplicateCombAfterScoringMethod,
				ScoringWorkflow.HitThresholdExpression, TruePositiveRate, FalsePositiveRate,
				Rating);

			return result;
		}
	}

	class HitTypePerformance {
		public string HitType { get; set; }
		public int NumTruePositives { get; set; }
		public int NumFalsePositives { get; set; }
		public int NumTrueNegatives { get; set; }
		public int NumFalseNegatives { get; set; }
		public int Rating { get; private set; }
	}
}
