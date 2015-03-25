using Lsr.Bio.HtsAnalysis.Core;
using System.Linq;

namespace Lsr.Bio.HtsAnalysis.Scoring.Transformers {
	/// <summary>
	/// Transformer that calculates the z score of input values relative to all values in the experiment in which
	/// they appear
	/// </summary>
    class ZScorePerExperiment: ZScore {
		/// <summary>
		/// Override of base method to get the values for the current experiment and calculate their mean and
		/// standard deviation
		/// </summary>
		/// <param name="plateRowColSignals">A filled array of data for the experiment as doubles</param>
        protected override void SetUpTransformForExpt(Plateset<double> plateset) {
            this.SetUpTransform(plateset.GetWellValues());
        }
    } //end class
}
