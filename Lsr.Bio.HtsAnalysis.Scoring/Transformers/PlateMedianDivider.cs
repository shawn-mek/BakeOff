using Lsr.Bio.HtsAnalysis.Core;
using System.Linq;

namespace Lsr.Bio.HtsAnalysis.Scoring.Transformers {
	/// <summary>
	/// Transformer that calculates the value of input values divided by the median of all values on the plate on 
	/// which they appear
	/// </summary>
    class PlateMedianDivider: MedianDivider {
		/// <summary>
		/// Override of base method to get the values for the current plate and calculate their median
		/// </summary>
		/// <param name="rowColSignals">A filled array of data for the plate as doubles</param>
        protected override void SetUpTransformForPlate(Plate<double> plate) {
			this.SetUpTransform(plate.GetWellValues());
        }
    } //end class
}
