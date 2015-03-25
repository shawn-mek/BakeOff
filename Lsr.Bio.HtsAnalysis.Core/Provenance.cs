using System.Collections.Generic;

namespace Lsr.Bio.HtsAnalysis.Core {
    /// <summary>
    /// Struct used to store information about the history of a PlatesetInfo
    /// </summary>
    public class Provenance {
        /// <summary>
        /// List of strings describing the inputs to a PlatesetInfo; may contain names of other PlatesetInfos or some other
        /// descriptors, such as the name(s) of the file(s) from which data was read
        /// </summary>
        public IList<string> InputNames;

        /// <summary>
        /// String describing the action on the inputs that yielded the current PlatesetInfo, such as "ZScorePerExperiment"
        /// </summary>
        public string Action;
    }
}
