using System.Collections.Generic;
using System.Linq;
using Lsr.Bio.Core.Utility;

namespace Lsr.Bio.HtsAnalysis.Combiners {
	/// <summary>
	/// Combiner that calculates the median of double input values
	/// </summary>
    public class MedianCombiner: Combiner<double, double> {
		#region constructors
		/// <summary>
		/// Default constructor that sets the value of the OutputDescriptor property
		/// </summary>
        public MedianCombiner() {
            this.OutputDescriptor = "Median";
        } //end constructor
		#endregion constructors

		#region methods
		/// <summary>
		/// Override of abstract Combine method that calculates the median of the double input values
		/// </summary>
		/// <param name="wellValues">A list of input double values, as from a single well across multiple 
		/// replicates</param>
		/// <returns>The median of the input double values</returns>
        protected override double Combine(IEnumerable<double> wellValues) {
            double[] wellValuesArray = wellValues.ToArray();
            double result = AnalysisUtilities.GetMedian(wellValuesArray);
            return result;
        } //end Combine
		#endregion methods
	} //end MedianCombiner
}
