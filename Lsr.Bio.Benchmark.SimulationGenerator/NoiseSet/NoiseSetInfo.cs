using System.Collections.Generic;

namespace Lsr.Bio.Benchmark.ScreenSimulator.NoiseSet {
    /// <summary>
    /// Simple class to aggregate basic info about a set of noise elements to be applied to the screen in concert
    /// </summary>
    internal class NoiseSetInfo {
        /// <summary>
        /// A user-assigned integer id for the noise set
        /// </summary>
        public int NoiseSetId { get; set; }

        /// <summary>
        /// A user-assigned text name for the noise set
        /// </summary>
        public string NoiseSetName { get; set; }

        /// <summary>
        /// Double containing a value that the generated noisy values may not be less than
        /// </summary>
        public double Floor { get; set; }

        /// <summary>
        /// Double containing a value that the generated noisy values may not be more than
        /// </summary>
        public double Ceiling { get; set; }

        /// <summary>
        /// A list of NoiseGeneratorInfo objects defining the noise elements found in the this noise set
        /// </summary>
        public List<NoiseMaker.NoiseGeneratorInfo> NoiseGeneratorInfos { get; set; }

        /// <summary>
        /// Default constructor that instantiates the list of NoiseGeneratorInfo objects
        /// </summary>
        public NoiseSetInfo() {
            this.NoiseGeneratorInfos = new List<NoiseMaker.NoiseGeneratorInfo>();
        } //end constructor
    } //end class NoiseSetInfo
}
