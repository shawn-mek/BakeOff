using System;
using System.Collections.Generic;
using System.IO;
using Lsr.Bio.Benchmark.Utilities;

namespace Lsr.Bio.Benchmark.ScreenSimulator.NoiseSet {
    /// <summary>
    /// Class that can load a list of NoiseSetInfo objects from an input text source
    /// </summary>
    internal class NoiseSetTextParser: InfoTextParser, INoiseSetTextParser {
        #region members
        /// <summary>
        /// Header line expected to preceed a single line of data defining basic properties of the noise set
        /// </summary>
        private static readonly string _NOISE_SET_HEADER_LINE = "#noise_set_id	noise_set_name	floor	ceiling";

        /// <summary>
        /// Header line expected to preceed one or more lines of data, each of which defines a noise element to 
        /// be found in the noise set
        /// </summary>
        private static readonly string _NOISES_HEADER_LINE = "#element	element_index	mean_change	standard_dev";
        #endregion

        #region constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public NoiseSetTextParser() {
            Dictionary<string, ParseLine<NoiseSetInfo>> lineParsersByHeader =
                new Dictionary<string, ParseLine<NoiseSetInfo>>();
            lineParsersByHeader.Add(_NOISE_SET_HEADER_LINE, _ParseNoiseSetLine);
            lineParsersByHeader.Add(_NOISES_HEADER_LINE, _ParseNoisesLine);
            this.LineParsersByHeader = lineParsersByHeader;
            this.RecordDelimiter = _NOISE_SET_HEADER_LINE;
        } //end constructor
        #endregion

        #region INoiseSetTextParser members
        /// <summary>
        /// Method that loads a list of NoiseSetInfo objects from an input text source; implements INoiseSetTextParser
        /// interface
        /// </summary>
        /// <param name="reader">An instantiated TextReader subclass</param>
        /// <returns>An instantiated list of filled NoiseSetInfo objects</returns>
        public List<NoiseSetInfo> LoadNoiseSetInfo(TextReader reader) {
            return LoadInfo<NoiseSetInfo>(reader);
        } //end LoadHitSetInfo
        #endregion

        #region internal members
        /// <summary>
        /// Method that takes in a line of text describing the basic properties of a noise set and uses it to fill in
        /// the main fields of the input NoiseSetInfo object
        /// </summary>
        /// <param name="noiseSetLine">A line of text describing the basic properties of a noise set; expected to have the
        /// fields defined in _NOISE_SET_HEADER_LINE</param>
        /// <param name="noiseSetInfo">An instantiated NoiseSetInfo object that is filled (or overwritten if already
        /// filled) by the method</param>
        internal void _ParseNoiseSetLine(string noiseSetLine, ref NoiseSetInfo noiseSetInfo) {
            string floorString;
            string ceilingString;
			string[] fields = noiseSetLine.Split(InfoTextParser.DELIMITER.ToCharArray());
            noiseSetInfo.NoiseSetId = Convert.ToInt32(fields[0]);
            noiseSetInfo.NoiseSetName = fields[1];

            //if no value was provided for floor or ceiling, use negative or positive infinity, respectively
            floorString = fields[2];
            noiseSetInfo.Floor = floorString != string.Empty ? Convert.ToDouble(floorString) : Double.NegativeInfinity;
            ceilingString = fields[3];
            noiseSetInfo.Ceiling = ceilingString != string.Empty ? Convert.ToDouble(ceilingString) : Double.PositiveInfinity;
            //TODO: replace hardcoded indices with symbolic constants
        } //end _ParseNoiseSetLine

        /// <summary>
        /// Method that takes in a line of text describing a noise element in the noise set, uses it to fill in
        /// a new NoiseMaker.NoiseGeneratorInfo object, and adds that filled object to the input NoiseSetInfo object's 
        /// NoiseGeneratorInfos list
        /// </summary>
        /// <param name="noisesLine">A line of text describing a noise element to be added to the screen; expected to have
        /// the fields defined in _NOISES_HEADER_LINE</param>
        /// <param name="noiseSetInfo">An instantiated NoiseSetInfo object that is added to by the method</param>
        internal void _ParseNoisesLine(string noisesLine, ref NoiseSetInfo noiseSetInfo) {
			string[] fields = noisesLine.Split(InfoTextParser.DELIMITER.ToCharArray());
            NoiseMaker.PlateElement element = (NoiseMaker.PlateElement)
                Enum.Parse(typeof(NoiseMaker.PlateElement), fields[0]);
            int elementIndex = Convert.ToInt32(fields[1]);
            double mean = Convert.ToDouble(fields[2]);
            double stdDev = Convert.ToDouble(fields[3]);
            NoiseMaker.NoiseGeneratorInfo info = 
                new NoiseMaker.NoiseGeneratorInfo(element, elementIndex, mean, stdDev);
            noiseSetInfo.NoiseGeneratorInfos.Add(info);
        } //end _ParseNoisesLine
        #endregion
    } //end class
}
