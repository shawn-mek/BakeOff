using System;
using System.Collections.Generic;
using System.IO;
using Lsr.Bio.Benchmark.Utilities;

namespace Lsr.Bio.Benchmark.ScreenSimulator.HitSet {
    /// <summary>
    /// Class that can load a list of HitSetInfo objects from an input text source
    /// </summary>
    public class HitSetTextParser: InfoTextParser, IHitSetTextParser {
        #region members
        /// <summary>
        /// Header line expected to preceed a single line of data defining basic properties of the hit set
        /// </summary>
        private static readonly string _HIT_SET_HEADER_LINE = "#hit_set_id	hit_set_name";

        /// <summary>
        /// Header line expected to preceed one or more lines of data, each of which defines a type of hit to be found
        /// in the hit set
        /// </summary>
        private static readonly string _HITS_HEADER_LINE = "#name	strength	number	ispercent";
        #endregion

        #region constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public HitSetTextParser() {
            Dictionary<string, ParseLine<HitSetInfo>> lineParsersByHeader =
                new Dictionary<string, ParseLine<HitSetInfo>>();
            lineParsersByHeader.Add(_HIT_SET_HEADER_LINE, _ParseHitSetLine);
            lineParsersByHeader.Add(_HITS_HEADER_LINE, _ParseHitsLine);
            this.LineParsersByHeader = lineParsersByHeader;
            this.RecordDelimiter = _HIT_SET_HEADER_LINE;
        } //end constructor
        #endregion

        #region IHitSetTextParser members
        /// <summary>
        /// Method that loads a list of HitSetInfo objects from an input text source; implements IHitSetTextParser
        /// interface
        /// </summary>
        /// <param name="reader">An instantiated TextReader subclass</param>
        /// <returns>An instantiated list of filled HitSetInfo objects</returns>
        public List<HitSetInfo> LoadHitSetInfo(TextReader reader) {
            return LoadInfo<HitSetInfo>(reader);
        } //end LoadHitSetInfo
        #endregion

        #region internal members
        /// <summary>
        /// Method that takes in a line of text describing the basic properties of a hit set and uses it to fill in
        /// the main fields of the input HitSetInfo object
        /// </summary>
        /// <param name="hitSetLine">A line of text describing the basic properties of a hit set; expected to have the
        /// fields defined in _HIT_SET_HEADER_LINE</param>
        /// <param name="hitSetInfo">An instantiated HitSetInfo object that is filled (or overwritten if already
        /// filled) by the method</param>
        internal void _ParseHitSetLine(string hitSetLine, ref HitSetInfo hitSetInfo) {
            string[] fields = hitSetLine.Split(InfoTextParser.DELIMITER.ToCharArray());
            hitSetInfo.HitSetId = Convert.ToInt32(fields[0]);
            hitSetInfo.HitSetName = fields[1];
            //TODO: replace hardcoded indices with symbolic constants?
        } //end _ParseHitSetLine

        /// <summary>
        /// Method that takes in a line of text describing a hit type to be found in the screen, uses it to fill in
        /// a new NoiseMaker.HitInfo object, and adds that filled object to the input HitSetInfo object's 
        /// HitInfos list
        /// </summary>
        /// <param name="hitsLine">A line of text describing a hit type to be found in the screen; expected to have
        /// the fields defined in _HITS_HEADER_LINE</param>
        /// <param name="hitSetInfo">An instantiated HitSetInfo object that is added to by the method</param>
        internal void _ParseHitsLine(string hitsLine, ref HitSetInfo hitSetInfo) {
			string[] fields = hitsLine.Split(InfoTextParser.DELIMITER.ToCharArray());
            string hitType = fields[0];
            double hitStrength = Convert.ToDouble(fields[1]);
            double hitNum = Convert.ToDouble(fields[2]);
            bool hitIsPercent = Convert.ToBoolean(fields[3]);
            NoiseMaker.HitInfo info = new NoiseMaker.HitInfo(hitType, hitStrength, hitNum, hitIsPercent);
            hitSetInfo.HitInfos.Add(info);
        } //end _ParseHitsLine
        #endregion
    } //end class
}
