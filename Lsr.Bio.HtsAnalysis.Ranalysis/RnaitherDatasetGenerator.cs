using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Lsr.Bio.HtsAnalysis.Core;

namespace Lsr.Bio.HtsAnalysis.RAnalysis {
    public class RnaitherDatasetGenerator {
    	internal static readonly string DELIMITER = "\t";

		public static KeyedPlatesetInfos<T> ReadColumnFromDataset<T>(TextReader inReader, string action, 
            string columnHeader, IList<string> platesetNames, int numPlates, int numRows, int numColumns) {

            string line;
			int desiredColIndex = 0;
            int maxHeaderLineIndex = 3;
            int lineNum = 0;
			KeyedPlatesetInfos<T> result = new KeyedPlatesetInfos<T>();
            while ((line = inReader.ReadLine()) != null) {
            	string[] fields = line.Split(DELIMITER.ToCharArray());
            	if (lineNum < maxHeaderLineIndex) {
                    //do nothing--go on to next line
                } else if (lineNum == maxHeaderLineIndex) {
                    desiredColIndex = fields.ToList().IndexOf(columnHeader) + 1; //one more column than header
                } else {
                    int replicateIndex;
                    int plateIndex;
                    int rowIndex;
                    int colIndex;
                    Utility.GetLocationFromFlattenedReplicates(lineNum - maxHeaderLineIndex, numRows, numColumns,
                        out replicateIndex, out plateIndex, out rowIndex, out colIndex);
                    string platesetName = platesetNames[replicateIndex];
                    if (!result.ContainsKey(platesetName)) {
                        Provenance newProvenance = RnaitherDatasetGenerator._GenerateProvenance(platesetName, action);
                        string newPlatesetName = string.Format("{0} of {1}", action, platesetName);
                        result[platesetName] = new PlatesetInfo<T>(newPlatesetName, newProvenance, numPlates, numRows, 
                            numColumns);
                    }
                    PlatesetInfo<T> currPlatesetInfo = result[platesetName].CastToPlatesetInfo<T>();
                    string fieldOfInterest = fields[desiredColIndex];
					T currValue = (T) Convert.ChangeType(fieldOfInterest, typeof(T));
                    currPlatesetInfo[plateIndex, rowIndex, colIndex] = new Well<T>(currValue);
                } //end if this is/isn't the line of the file containing the headers
            } //next line

            return result;
        } //end ReadColumnFromDataset

        private static Provenance _GenerateProvenance(string inputName, string action) {
            Provenance result = new Provenance();
            result.InputNames = new List<string> {inputName};
            result.Action = action;
            return result;
        }
 
        private ArrayedScreenData _Data;
        private IList<string> _NegControlReagentKinds;
        private IList<string> _PosControlReagentKinds;

        public RnaitherDatasetGenerator(ArrayedScreenData data, IList<string> negControlKinds, 
            IList<string> posControlKinds) {

            this._Data = data;
            this._NegControlReagentKinds = negControlKinds;
            this._PosControlReagentKinds = posControlKinds;
        } //end constructor

        //note that there will be one more field output in each data row than in the header row; this is not a 
        //mistake but rather an (odd) expectation of RNAither
		public string GenerateDatasetText<T>(KeyedPlatesetInfos<T> platesetsToOutput) {
            string columnHeaders = "Spotnumber Internal_GeneID GeneName SpotType SigIntensity SDSIntensity Background LabtekNb RowNb ColNb ScreenNb NbCells PercCells";
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(this._GenerateDatasetHeaderText(this._Data));
            builder.AppendLine(columnHeaders);

            string notApplicable = "NA";
            List<string> fields;
            int platePosition;
            int index = 1;
            int replicateIndex = 0;
            foreach (string currPlatesetKey in platesetsToOutput.Keys) {
                IPlatesetInfo currIPlatesetInfo = this._Data.Signals[currPlatesetKey];
                PlatesetInfo<T> currPlatesetInfo = currIPlatesetInfo.CastToPlatesetInfo<T>();
                for (int plateIndex = 0; plateIndex < currPlatesetInfo.NumPlates; plateIndex++) {
                    platePosition = 1;
                    for (int rowIndex = 0; rowIndex < currPlatesetInfo.NumRows; rowIndex++) {
                        for (int colIndex = 0; colIndex < currPlatesetInfo.NumColumns; colIndex++) {
                            Reagent currReagent = this._Data.Reagents[plateIndex, rowIndex, colIndex].Value;
                            fields = new List<string>();
                            fields.Add(index.ToString());
                            fields.Add(platePosition.ToString());
                            //fields.Add(currReagent.Id);
                            fields.Add(currReagent.Name); //TEMPorary
                            fields.Add(currReagent.Name);

                            bool ignorePosition = currPlatesetInfo.GetIgnoreValue(plateIndex, rowIndex, colIndex);
                            string spotType = this._GenerateSpotType(currReagent.Kind, ignorePosition);
                            fields.Add(spotType);

                            fields.Add(currPlatesetInfo[plateIndex, rowIndex, colIndex].ToString());
                            fields.Add(notApplicable);
                            fields.Add(notApplicable);
                            fields.Add((plateIndex + 1).ToString());
                            fields.Add((rowIndex + 1).ToString());
                            fields.Add((colIndex + 1).ToString());
                            fields.Add((replicateIndex + 1).ToString());
                            fields.Add(notApplicable);
                            fields.Add(notApplicable);

							string currLine = string.Join(DELIMITER, fields.ToArray());
                            builder.AppendLine(currLine);

                            platePosition++;
                            index++;
                        } //next column
                    } //next row
                } //next plate

                replicateIndex++;
            } //next replicate

            return builder.ToString();
        } //end _GenerateDatasetText

        private string _GenerateDatasetHeaderText(ArrayedScreenData data) {
            string headerTemplate = @"external_experiment_name,{0}
type_of_data,Well plate data          
comments,NA";
            return string.Format(headerTemplate, data.ScreenName);
        } //end _GenerateDatasetHeaderText

        private string _GenerateSpotType(string currReagentKind, bool ignorePosition) {

            string result;
            if (this._NegControlReagentKinds.Contains(currReagentKind)) {
                result = "0";
            } else if (this._PosControlReagentKinds.Contains(currReagentKind)) {
                result = "1";
            } else if (ignorePosition) {
                result = "-1";
            } else {
                result = "2";
            } //end if
            return result;
        } //end _GenerateSpotType
    } //end class
}
