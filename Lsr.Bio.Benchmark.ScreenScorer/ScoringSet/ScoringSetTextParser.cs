using System;
using System.Collections.Generic;
using System.IO;
using Lsr.Bio.Benchmark.Utilities;
using Lsr.Bio.HtsAnalysis.Workflows;

namespace Lsr.Bio.Benchmark.ScreenScorer.ScoringSet {
    /// <summary>
    /// Class that can load a list of ScoringSetInfo objects from an input text source
    /// </summary>
	internal class ScoringSetTextParser: InfoTextParser, IScoringSetTextParser {
        #region members
        /// <summary>
        /// Header line expected to preceed a single line of data defining basic properties of the scoring set
        /// </summary>
        private static readonly string _SCORING_SET_HEADER_LINE = "#scoring_set_id	scoring_set_name";

        /// <summary>
        /// Header line expected to preceed one or more lines of data, each of which defines a scoring workflow to be applied
        /// in the simulated data
        /// </summary>
        private static readonly string _SCORING_WORKFLOW_HEADER_LINE = "#normalization_method	replicate_combination_before_scoring_method	scoring_method	replicate_combination_after_scoring_method	hit_id_threshold";
        #endregion

        #region constructors
        /// <summary>
        /// Default constructor
        /// </summary>
		public ScoringSetTextParser() {
            Dictionary<string, ParseLine<ScoringSetInfo>> lineParsersByHeader =
                new Dictionary<string, ParseLine<ScoringSetInfo>>();
			lineParsersByHeader.Add(_SCORING_SET_HEADER_LINE, _ParseScoringSetLine);
			lineParsersByHeader.Add(_SCORING_WORKFLOW_HEADER_LINE, _ParseScoringWorkflowInfoLine);
            this.LineParsersByHeader = lineParsersByHeader;
			this.RecordDelimiter = _SCORING_SET_HEADER_LINE;
        } //end constructor
        #endregion

        #region IScoringSetTextParser members
        /// <summary>
		/// Method that loads a list of ScoringWorkflowInfo objects from an input text source; implements IScoringSetTextParser
        /// interface
        /// </summary>
        /// <param name="reader">An instantiated TextReader subclass</param>
        /// <returns>An instantiated list of filled HitSetInfo objects</returns>
    	public List<ScoringSetInfo> LoadScoringSetInfo(TextReader reader) {
            return LoadInfo<ScoringSetInfo>(reader);
        } //end LoadScoringSetInfo
        #endregion

        #region private members
        /// <summary>
        /// Method that takes in a line of text describing the basic properties of a scoring set and uses it to fill in
        /// the main fields of the input ScoringSetInfo object
        /// </summary>
        /// <param name="hitSetLine">A line of text describing the basic properties of a scoring set; expected to have the
        /// fields defined in _SCORING_SET_HEADER_LINE</param>
        /// <param name="scoringSetInfo">An instantiated ScoringSetInfo object that is filled (or overwritten if already
        /// filled) by the method</param>
        private void _ParseScoringSetLine(string hitSetLine, ref ScoringSetInfo scoringSetInfo) {
            string[] fields = hitSetLine.Split(InfoTextParser.DELIMITER.ToCharArray());
            scoringSetInfo.ScoringSetId = Convert.ToInt32(fields[0]);
            scoringSetInfo.ScoringSetName = fields[1];
            //TODO: replace hardcoded indices with symbolic constants?
        } //end _ParseHitSetLine

        /// <summary>
        /// Method that takes in a line of text describing a scoring workflow to be applied to the simulated data, uses it to 
        /// fill in a new ScoringWorkflowInfo object, and adds that filled object to the input ScoringSetInfo object's 
		/// ScoringWorkflowInfos list
        /// </summary>
        /// <param name="scoringWorkflowLine">A line of text describing a scoring workflow to be applied to the simulated data; 
        /// expected to have the fields defined in _SCORING_WORKFLOW_HEADER_LINE</param>
        /// <param name="scoringSetInfo">An instantiated ScoringSetInfo object that is added to by the method</param>
        private void _ParseScoringWorkflowInfoLine(string scoringWorkflowLine, ref ScoringSetInfo scoringSetInfo) {
			string[] fields = scoringWorkflowLine.Split(InfoTextParser.DELIMITER.ToCharArray());
            string normalizationMethod = fields[0];
        	string replicateCombBeforeScoringMethod = fields[1];
        	string scoringMethod = fields[2];
        	string replicateCombAfterScoringMethod = fields[3];
        	string hitThresholdExpression = fields[4];
        	ScoringWorkflowInfo info = new ScoringWorkflowInfo(normalizationMethod, replicateCombBeforeScoringMethod,
        	    scoringMethod, replicateCombAfterScoringMethod, hitThresholdExpression);
            scoringSetInfo.ScoringWorkflowInfos.Add(info);
		} //end _ParseScoringWorkflowInfoLine
        #endregion
	}
}
