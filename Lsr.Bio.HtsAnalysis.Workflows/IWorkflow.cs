using System.Collections.Generic;
using Lsr.Bio.HtsAnalysis.Core;

namespace Lsr.Bio.HtsAnalysis.Workflows {
	/// <summary>
	/// Public interface defining method(s) for executing a data analysis workflow such as identifying hits
	/// from real or simulated screening data.
	/// </summary> 
	public interface IWorkflow {
		/// <summary>
		/// Method to execute the data analysis workflow over the input data
		/// </summary>
		/// <param name="data">A filled ArrayedScreenData object containing the data to analyze</param>
		void Execute(ArrayedScreenData data, IEnumerable<string> signalNames);
	}
}