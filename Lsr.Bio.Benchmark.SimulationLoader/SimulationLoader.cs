using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lsr.Bio.Benchmark.Utilities;
using Lsr.Bio.HtsAnalysis.Core;

namespace Lsr.Bio.Benchmark.ScreenSimulationLoader {
	public class SimulationLoader {
		private string[] _ColumnHeaders;
		private int _ValueTypeIndex;

		private int _ReagentNameIndex {
			get { return this._ValueTypeIndex - 1; }
		} //end _ReagentNameIndex

		public ArrayedScreenData LoadFromReader<T>(TextReader inReader, string inputSource, int numPlates, int numRows, int numCols) {
			string line;
			int index = 0;
			Dictionary<string, Reagent> reagentsByName = new Dictionary<string, Reagent>();
			ArrayedScreenData result = new ArrayedScreenData(numPlates, numRows, numCols);
			Provenance provenance = this._GenerateProvenance(inputSource);
			PlatesetInfo<Reagent> reagents = new PlatesetInfo<Reagent>(result.REAGENT_PLATESET_NAME, provenance,
				numPlates, numRows, numCols);

			this._ClearState();
			while ((line = inReader.ReadLine()) != null) {
				string[] fields = line.Split(InfoTextParser.DELIMITER.ToCharArray());

				if (line.StartsWith(InfoTextParser.COMMENT_MARK)) {
					this._SetStateFromHeaderLine(fields, index);
				} else {
					//get the well location represented by this data line
					int plateIndex;
					int rowIndex;
					int colIndex;
					HtsAnalysis.Core.Utility.GetLocationFromFlattenedPlates(index, numRows, numCols, out plateIndex,
						out rowIndex, out colIndex);

					//collect the reagent described in this line
					Reagent currReagent = this._GetReagent(fields, reagentsByName);
					reagents[plateIndex, rowIndex, colIndex] = new Well<Reagent>(currReagent);

					//fill a PlatesetDictionary with signals for each column after ValueType, keyed by the field names
					for (int i = this._ValueTypeIndex + 1; i < fields.Length; i++) {
						PlatesetInfo<T> currPlatesetInfo = this._GetPlateset<T>(i, numPlates, numRows, numCols,
							provenance, result.Signals);
						T currWellValue = (T) Convert.ChangeType(fields[i], typeof(T));
						currPlatesetInfo[plateIndex, rowIndex, colIndex] = new Well<T>(currWellValue);
					} //next data column in this line

					index++;
				} //end if line does/doesn't start with comment mark
			} //end while

			result.Reagents = reagents;
			return result;
		} //end LoadFromReader

		private void _ClearState() {
			this._ColumnHeaders = null;
			this._ValueTypeIndex = 0;
		} //end _ClearState

		private void _SetStateFromHeaderLine(string[] fields, int index) {
			if (index != 0) {
				throw new Exception(string.Format("Unexpected comment mark appears at line {0}", index + 1));
			} //end if comment mark is on line other than first line of file
			this._ColumnHeaders = fields;
			this._ValueTypeIndex = fields.ToList().IndexOf(InfoTextParser.VALUE_TYPE_HEADER);
		} //end _SetStateFromHeaderLine

		private Reagent _GetReagent(string[] fields, Dictionary<string, Reagent> reagentsByName) {
			//use the info from the reagent field (valuetype -1 index)
			//and the ValueType field to fill the PlatesetDictionary of reagents;
			//only remake reagents we haven't seen before, otherwise reference
			string currReagentName = fields[this._ReagentNameIndex];
			if (!reagentsByName.ContainsKey(currReagentName)) {
				Reagent newReagent = new Reagent(null, currReagentName, fields[this._ValueTypeIndex]);
				reagentsByName.Add(currReagentName, newReagent);
			} //end if current reagent hasn't been encountered before
			return reagentsByName[currReagentName];
		} //end 

		private PlatesetInfo<T> _GetPlateset<T>(int colIndex, int numPlates, int numRows, int numCols,
			Provenance provenance, KeyedIPlatesetInfos signals) {

			PlatesetInfo<T> result;
			IPlatesetInfo currSignal;
			string signalKey = this._ColumnHeaders[colIndex];
			if (!signals.ContainsKey(signalKey)) {
				signals.Add(new PlatesetInfo<T>(signalKey, provenance, numPlates, numRows, numCols));
			} //end if plateset dictionary for this signal doesn't exist yet
			currSignal = signals[signalKey];
			result = currSignal.CastToPlatesetInfo<T>();
			return result;
		} //end _GetPlateset

		private Provenance _GenerateProvenance(string inputName) {
			Provenance result = new Provenance();
			result.InputNames = new List<string> { inputName };

			Type currType = this.GetType();
			result.Action = currType.Name;
			return result;
		} //end _GenerateProvenance
	} //end class
}
