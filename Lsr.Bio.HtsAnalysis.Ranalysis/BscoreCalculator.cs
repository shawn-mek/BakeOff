using System;
using System.Collections.Generic;
using System.Linq;
using System.IO; //for File
using Lsr.Bio.HtsAnalysis.Core;

namespace Lsr.Bio.HtsAnalysis.RAnalysis {
    public class BscoreCalculator<T>: RnaitherScriptRunner<T> {
// ReSharper disable InconsistentNaming
// Caps because readonly and assigned at declaration--essentially a constant
        private readonly string _HEADER_OF_COL_CALCULATED_ON = "SigIntensity";
// ReSharper restore InconsistentNaming
        private readonly string _BSCORE_SCRIPT_TEMPLATE; 
        private readonly bool _EXCLUDE_CONTROLS;

        public BscoreCalculator(RnaitherDatasetGenerator datasetGenerator,
			KeyedPlatesetInfos<T> platesetsToOutput, bool excludeControls) 
            : base (datasetGenerator, platesetsToOutput){

            this._BSCORE_SCRIPT_TEMPLATE = @"normres <- BScore(header, dataset, list(""" + this._HEADER_OF_COL_CALCULATED_ON
                + @""", {1}))
write.table(normres[[2]], ""{2}"", col.names=F, quote=FALSE)";
            this._EXCLUDE_CONTROLS = excludeControls;
        } //end constructor

        protected override string GenerateRscript() {
            string scriptTemplate = this.DATASET_LOAD_SCRIPT_TEMPLATE + this._BSCORE_SCRIPT_TEMPLATE;
            string result = string.Format(scriptTemplate, this.DatasetFilePath, Convert.ToInt32(this._EXCLUDE_CONTROLS),
                this.OutputFilePath);
            return result;
        } //end GenerateRscript

		public KeyedPlatesetInfos<T> GetBscores() {
			KeyedPlatesetInfos<T> result;
            IList<string> platesetNames = this.PlatesetsToOutput.Keys.ToList();

            TextReader inReader = base.Run();
            result = RnaitherDatasetGenerator.ReadColumnFromDataset<T>(inReader, "BscoreCalculator", 
                this._HEADER_OF_COL_CALCULATED_ON, platesetNames, this.PlatesetsToOutput.NumPlates, 
                this.PlatesetsToOutput.NumRows, this.PlatesetsToOutput.NumColumns);
            return result;
        } //end GetBscores
    } //end class
}
