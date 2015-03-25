using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lsr.Bio.Core.Utility;

namespace Lsr.Bio.HtsAnalysis.Simulations {
	internal class SimulationUtilities {
		internal static readonly string DELIMITER = TextUtilities.TAB;
		internal static readonly string VALUE_TYPE_HEADER = "ValueType";

		/// <summary>
		/// Character that is expected at the beginning of all header lines in the input files
		/// </summary>
		internal static readonly string COMMENT_MARK = "#";
	}
}
