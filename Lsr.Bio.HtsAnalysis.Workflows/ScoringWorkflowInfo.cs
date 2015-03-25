namespace Lsr.Bio.HtsAnalysis.Workflows {
	public class ScoringWorkflowInfo {
		public string NormalizationMethod { get; set; }
		public string ReplicateCombBeforeScoringMethod { get; set; }
		public string ScoringMethod { get; set; }
		public string ReplicateCombAfterScoringMethod { get; set; }
		public string HitThresholdExpression { get; set; }

		public ScoringWorkflowInfo() {}

		public ScoringWorkflowInfo(string normalizationMethod, string replicateCombBeforeScoringMethod, string scoringMethod,
		                           string replicateCombAfterScoringMethod, string hitThresholdExpression) {

			this.NormalizationMethod = normalizationMethod;
			this.ReplicateCombBeforeScoringMethod = replicateCombBeforeScoringMethod;
			this.ScoringMethod = scoringMethod;
			this.ReplicateCombAfterScoringMethod = replicateCombAfterScoringMethod;
			this.HitThresholdExpression = hitThresholdExpression;
		}

		public override string ToString() {
			//TODO: implement
            return string.Empty;
		}
	}
}
