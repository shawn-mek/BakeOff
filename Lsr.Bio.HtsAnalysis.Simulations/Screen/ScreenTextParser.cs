using System;
using System.Collections.Generic;
using System.IO;
using Lsr.Bio.Core.Utility;

namespace Lsr.Bio.HtsAnalysis.Simulations {
    /// <summary>
    /// Class that can load a list of ScreenInfo objects from an input text source
    /// </summary>
    internal class ScreenTextParser: InfoTextParser, IScreenTextParser {
        #region members
        /// <summary>
        /// Header line expected to preceed a single line of data defining basic properties of the screen
        /// </summary>
        private static readonly string _SCREEN_HEADER_LINE = "#screen_id	screen_name	num_rows	num_columns	num_plates	num_replicates	default_value";
        
        /// <summary>
        /// Header line expected to preceed one or more lines of data, each of which defines a control to be found
        /// in the screen
        /// </summary>
        private static readonly string _CONTROLS_HEADER_LINE = "#control_name	control_symbol	control_strength";
        #endregion

        #region constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public ScreenTextParser() {
            Dictionary<string, _ParseLine<ScreenInfo>> lineParsersByHeader = 
                new Dictionary<string, _ParseLine<ScreenInfo>>();
            lineParsersByHeader.Add(_SCREEN_HEADER_LINE, _ParseScreenLine);
            lineParsersByHeader.Add(_CONTROLS_HEADER_LINE, _ParseControlLine);
            this.LineParsersByHeader = lineParsersByHeader;
            this._RECORD_DELIMITER = _SCREEN_HEADER_LINE;
        } //end constructor
        #endregion

        #region IScreenTextParser implementation
        /// <summary>
        /// Method that loads a list of ScreenInfo objects from an input text source; implements IScreenTextParser
        /// interface
        /// </summary>
        /// <param name="reader">An instantiated TextReader subclass</param>
        /// <returns>An instantiated list of filled ScreenInfo objects</returns>
        public List<ScreenInfo> LoadScreenInfo(TextReader reader) {
            return LoadInfo<ScreenInfo>(reader);
        } //end LoadScreenInfo
        #endregion

        #region private methods
        /// <summary>
        /// Method that takes in a line of text describing the basic properties of a screen and uses it to fill in
        /// the main fields of the input ScreenInfo object
        /// </summary>
        /// <param name="screenLine">A line of text describing the basic properties of a screen; expected to have the
        /// fields defined in _SCREEN_HEADER_LINE</param>
        /// <param name="screenInfo">An instantiated ScreenInfo object that is filled (or overwritten if already
        /// filled) by the method</param>
        private void _ParseScreenLine(string screenLine, ref ScreenInfo screenInfo) {
            string[] fields = screenLine.Split(TextUtilities.TAB.ToCharArray());
            screenInfo.ScreenId = Convert.ToInt32(fields[0]);
            screenInfo.ScreenName = fields[1];
            screenInfo.NumRows = Convert.ToInt32(fields[2]);
            screenInfo.NumColumns = Convert.ToInt32(fields[3]);
            screenInfo.NumReplicates = Convert.ToInt32(fields[4]);
            screenInfo.DefaultValue = Convert.ToDouble(fields[5]);
            //TODO: replace hardcoded indices with symbolic constants
        } //end _ParseScreenLine

        /// <summary>
        /// Method that takes in a line of text describing a control to be found in the screen, uses it to fill in
        /// a new NoiseMaker.ControlInfo object, and adds that filled object to the input ScreenInfo object's 
        /// ControlInfos list
        /// </summary>
        /// <param name="controlLine">A line of text describing a control to be found in the screen; expected to have
        /// the fields defined in _CONTROLS_HEADER_LINE</param>
        /// <param name="screenInfo">An instantiated ScreenInfo object that is added to by the method</param>
        private void _ParseControlLine(string controlLine, ref ScreenInfo screenInfo) {
            string[] fields = controlLine.Split(TextUtilities.TAB.ToCharArray());
            string controlName = fields[0];
            string controlSymbol = fields[1];
            double controlStrength = Convert.ToDouble(fields[2]);
            NoiseMaker.ControlInfo info = new NoiseMaker.ControlInfo(controlName, controlSymbol, controlStrength);
            screenInfo.ControlInfos.Add(info);
        } //end _ParseControlLine
        #endregion
    } //end class
}
