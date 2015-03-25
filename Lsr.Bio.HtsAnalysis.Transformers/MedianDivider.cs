using System.Collections.Generic;
using System.Linq;
using Lsr.Bio.Core.Utility;

namespace Lsr.Bio.HtsAnalysis.Transformers {
	/// <summary>
	/// Abstract Transformer class that implements members and methods shared by classes such as PlateMedianDivider 
	/// and SampleMedianDivider
	/// </summary>
    abstract class MedianDivider: Transformer<double, double> {
		#region members
		/// <summary>
		/// Double containing the median of the relevant signals; used as denominator in division transformation.
		/// </summary>
        protected double MedianOfSignals;
		#endregion members

		#region methods
		/// <summary>
		/// Override of abstract Transform method that divides the input value by the median of the relevant
		/// signals
		/// </summary>
		/// <param name="wellSignal">A double to be transformed</param>
		/// <returns>The input divided by the median of the relevant signals</returns>
		protected override double Transform(double wellSignal) {
            double result = wellSignal / this.MedianOfSignals;
            return result;
        }

		/// <summary>
		/// Method that calculates the median of the input signals
		/// </summary>
		/// <param name="allSignals">A filled array of doubles containing the signals for which to calculate the
		/// median</param>
        protected void SetUpTransform(IEnumerable<double> allSignals) {
            this.MedianOfSignals = AnalysisUtilities.GetMedian(allSignals.ToArray());
		}
		#endregion methods
	} //end class
}
