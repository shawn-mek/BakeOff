using System.Collections.Generic;

namespace Lsr.Bio.HtsAnalysis.Simulations {
    /// <summary>
    /// Simple class to aggregate basic info about the screen to be simulated
    /// </summary>
    internal class ScreenInfo {
        /// <summary>
        /// A user-assigned integer id for the simulated screen
        /// </summary>
        public int ScreenId { get; set; }

        /// <summary>
        /// A user-assigned text name for the simulated screen
        /// </summary>
        public string ScreenName { get; set; }

        /// <summary>
        /// Number of rows on each plate of the simulated screen
        /// </summary>
        public int NumRows { get; set; }

        /// <summary>
        /// Number of columns on each plate of the simulated screen
        /// </summary>
        public int NumColumns { get; set; }

        /// <summary>
        /// Number of replicates in the simulated screen
        /// </summary>
        public int NumReplicates { get; set; }

        /// <summary>
        /// Default value of all non-hit, non-control wells in the simulated screen
        /// </summary>
        public double DefaultValue { get; set; }

        /// <summary>
        /// A list of ControlInfo objects defining the controls found in the simulated screen
        /// </summary>
        public List<NoiseMaker.ControlInfo> ControlInfos { get; set; }

        /// <summary>
        /// Property that creates a dictionary of the control infos found in the ScreenInfo object, keyed 
        /// by their Name property, suitable for use as input to NoiseMaker.RandomHitMaker.MakeRandomHits calls.
        /// </summary>
        public Dictionary<string, NoiseMaker.ControlInfo> ControlInfosDict {
            get {
                Dictionary<string, NoiseMaker.ControlInfo> result = new Dictionary<string, NoiseMaker.ControlInfo>();
                foreach (NoiseMaker.ControlInfo currControlInfo in this.ControlInfos) {
                    result.Add(currControlInfo.Name, currControlInfo);
                } //next controlInfo
                return result;
            } //end get
        } //end ControlInfosDict

        /// <summary>
        /// Default constructor that instantiates the list of ControlInfo objects
        /// </summary>
        public ScreenInfo() {
            this.ControlInfos = new List<NoiseMaker.ControlInfo>();
        } //end constructor
    } //end class ScreenInfo
}
