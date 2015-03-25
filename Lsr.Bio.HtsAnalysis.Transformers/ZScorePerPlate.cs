using Lsr.Bio.HtsAnalysis.Core;

namespace Lsr.Bio.HtsAnalysis.Transformers {
	/// <summary>
	/// Transformer that calculates the z score of input values relative to all values on the plate on which
	/// they appear
	/// </summary>
    public class ZScorePerPlate: ZScore {
		/// <summary>
		/// Override of base method to get the values for the current plate and calculate their mean and
		/// standard deviation
		/// </summary>
		/// <param name="rowColSignals">A filled array of data for the plate as doubles</param>
        protected override void SetUpTransformForPlate(Plate<double> plate) {
            this.SetUpTransform(plate.GetWellValues());
        }
    } //end class
}
