using System.Collections.Generic;
using Lsr.Bio.HtsAnalysis.Workflows;

namespace Lsr.Bio.Benchmark.ScreenScorer.ScoringSet {
	/// <summary>
	/// Simple class to aggregate basic info about a set of scoring workflows to be applied to the simulated data
	/// </summary>
	internal class ScoringSetInfo {
		/// <summary>
		/// A user-assigned integer id for the scoring set
		/// </summary>
		public int ScoringSetId { get; set; }

		/// <summary>
		/// A user-assigned text name for the scoring set
		/// </summary>
		public string ScoringSetName { get; set; }

		/// <summary>
		/// A list of ScoringWorkflowInfos objects defining the scoring workflows to be applied to the simulated data
		/// </summary>
		public List<ScoringWorkflowInfo> ScoringWorkflowInfos { get; set; }

		/// <summary>
		/// Default constructor that instantiates the list of ScoringWorkflowInfo objects
		/// </summary>
		public ScoringSetInfo() {
			this.ScoringWorkflowInfos = new List<ScoringWorkflowInfo>();
		} //end constructor
	}
}
