using System.Collections.Generic;

namespace Lsr.Bio.HtsAnalysis.Simulations {
    /// <summary>
    /// Simple class to aggregate basic info about a set of hits to be applied to the screen in concert
    /// </summary>
    internal class HitSetInfo {
        /// <summary>
        /// A user-assigned integer id for the hit set
        /// </summary>
        public int HitSetId { get; set; }

        /// <summary>
        /// A user-assigned text name for the hit set
        /// </summary>
        public string HitSetName { get; set; }

        /// <summary>
        /// A list of HitInfo objects defining the hits found in the this hit set
        /// </summary>
        public List<NoiseMaker.HitInfo> HitInfos { get; set; }

        /// <summary>
        /// Default constructor that instantiates the list of HitInfo objects
        /// </summary>
        public HitSetInfo() {
            this.HitInfos = new List<NoiseMaker.HitInfo>();
        } //end constructor
    } //end class HitSetInfo
}
