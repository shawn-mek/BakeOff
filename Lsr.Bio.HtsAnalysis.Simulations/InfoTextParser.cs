using System;
using System.Collections.Generic;
using System.IO;

namespace Lsr.Bio.HtsAnalysis.Simulations {
    /// <summary>
    /// Abstract class that can load a generic object T from an input text source
    /// </summary>
    internal abstract class InfoTextParser {
        /// <summary>
        /// Delegate definition for parsing methods
        /// </summary>
        /// <typeparam name="T">Generic type standing for the object to be filled in with data from the input text 
        /// line by the parsing method</typeparam>
        /// <param name="line">A line of data from the text source to be parsed and filled into the info object
        /// </param>
        /// <param name="info">An object of generic type T to be filled in with the data from the input text line
        /// by the parsing method</param>
        protected delegate void _ParseLine<T>(string line, ref T info);

        /// <summary>
        /// Object holding a dictionary of delegate methods keyed by the header line defining the sorts of data
        /// that they are appropriate to parse
        /// </summary>
        /// <remarks>I would have preferred to define this as Dictionary<string, _ParseLine<T>> lineParsersByHeader,
        /// but this causes a compiler error for reasons that are unclear to me; therefore, I'm stuck making it
        /// a plain "object" and casting it at run time.</remarks>
        protected object LineParsersByHeader;

        /// <summary>
        /// String holding the header line that is the record delimiter (since a record may have more than one 
        /// kind of header line within it)
        /// </summary>
        protected string _RECORD_DELIMITER;

        /// <summary>
        /// Method that parses an input text source into a list of filled records of type T
        /// </summary>
        /// <typeparam name="T">Generic type standing for the object to be filled in with data from the input text 
        /// source</typeparam>
        /// <param name="reader">An instantiated TextReader subclass</param>
        /// <returns>A list of filled objects of type T</returns>
        /// <remarks>What sort of parsing each data line needs is determined by the most recent header line that
        /// occurred above it.  Each header line is associated with a parsing method (defined as a delegate) that is 
        /// used to parse all data lines encountered after an instance of that header.</remarks>
        public List<T> LoadInfo<T>(TextReader reader) where T: new() {
            Dictionary<string, _ParseLine<T>> lineParsersByHeader =
                this.LineParsersByHeader as Dictionary<string, _ParseLine<T>>;
            List<T> result = new List<T>();
            _ParseLine<T> currParseMethod = null;
            T currItem = default(T);
            string line;

            while ((line = reader.ReadLine()) != null) {
                if (line.StartsWith(SimulationUtilities.COMMENT_MARK)) {
                    if (!lineParsersByHeader.ContainsKey(line)) {
                        throw new Exception(string.Format("Unrecognized header line: {0}", line));
                    } //end if

                    currParseMethod = lineParsersByHeader[line];

                    //if this line is the record delimiter, collect any record made so far and start a new one
                    if (line == this._RECORD_DELIMITER) {
                        if (currItem != null) { result.Add(currItem); }
                        currItem = new T();
                    } //end if
                } else {
                    if (currParseMethod == null) {
                        throw new Exception(string.Format("No header line found before line: {0}", line));
                    } //end if

                    currParseMethod(line, ref currItem);
                } //end if
            } //end while

            //collect last record and return all records found
            result.Add(currItem);
            return result;
        } //end LoadInfo<T>
    } //end class InfoTextParser
}
