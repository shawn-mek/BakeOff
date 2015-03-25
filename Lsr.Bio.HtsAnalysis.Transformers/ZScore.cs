using System;
using System.Collections.Generic;
using System.Linq;

namespace Lsr.Bio.HtsAnalysis.Transformers {
	/// <summary>
	/// Abstract Transformer class that implements members and methods shared by classes such as 
	/// ZScorePerExperiment and ZScorePerPlate
	/// </summary>
    public abstract class ZScore: Transformer<double, double> {
		#region members
		/// <summary>
		///  Double containing the mean of the relevant signals
		/// </summary>
		protected double Mean;

		/// <summary>
		/// Double containing the standard deviation of the relevant signals
		/// </summary>
        protected double StdDev;
		#endregion members

		#region methods
		/// <summary>
		/// Override of abstract Transform method that calculates the z score of the input value, subtracting the
		/// mean and then dividing by the standard deviation
		/// </summary>
		/// <param name="wellSignal">A double to be transformed</param>
		/// <returns>The z score of the input</returns>
		protected override double Transform(double wellSignal) {
            double result = (wellSignal - this.Mean) / this.StdDev;
            return result;
        }

		/// <summary>
		/// Method that calculates the mean and standard deviation of the input signals
		/// </summary>
		/// <param name="allSignals">A filled array of doubles containing the signals for which to calculate the
		/// mean and standard deviation</param>
        protected void SetUpTransform(IEnumerable<double> allSignals) {
            this.Mean = allSignals.Average();
			this.StdDev = allSignals.StandardDeviation();
		}
		#endregion methods
    } //end class

	/// <summary>
	/// This class or at least its contents probably belong in Lsr.Bio.Core; I will move them there in a future
	/// release of SharedSource [ab 03/16/2012].
	/// </summary>
	public static class MathExtensions {
		/// <summary>
		/// Extension method to calculate the variance of an input IEnumerable of doubles.
		/// </summary>
		/// <param name="source">A filled IEnumerable containing doubles</param>
		/// <returns>The variance of the input doubles</returns>
		/// <remarks>This code was taken from http://www.codeproject.com/Articles/42492/Using-LINQ-to-Calculate-Basic-Statistics . 
		/// It is available under the The Code Project Open License (CPOL) 1.02 and can be used for commercial 
		/// applications.</remarks>
		public static double Variance(this IEnumerable<double> source) {
			double avg = source.Average();
			double d = source.Aggregate(0.0,
			                            (total, next) => total += Math.Pow(next - avg, 2));
			return d / (source.Count() - 1);
		}

		/// <summary>
		/// Extension method to calculate the standard deviation of an input IEnumerable of doubles.
		/// </summary>
		/// <param name="source">A filled IEnumerable containing doubles</param>
		/// <returns>The standard deviation of the input doubles</returns>
		/// <remarks>This code was taken from http://www.codeproject.com/Articles/42492/Using-LINQ-to-Calculate-Basic-Statistics . 
		/// It is available under the The Code Project Open License (CPOL) 1.02 and can be used for commercial 
		/// applications.</remarks>
		public static double StandardDeviation(this IEnumerable<double> source) {
			return Math.Sqrt(source.Variance());
		}
	} //end class
}
