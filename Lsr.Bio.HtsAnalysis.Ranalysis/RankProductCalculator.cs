using System.IO; //for File
using Lsr.Bio.HtsAnalysis.Core;

namespace Lsr.Bio.HtsAnalysis.RAnalysis {
    public class RankProductCalculator<T>: RnaitherScriptRunner<T> {
        private readonly string _RANK_PRODUCT_SCRIPT_TEMPLATE = @"pvals1 <- RankProduct(dataset, list({1}, 1, ""SigIntensity"", ""GeneName""))
write.table(pvals1[[1]], ""{2}"", col.names=F, quote=FALSE)";
        private readonly int _NUM_SHUFFLES;

        public RankProductCalculator(RnaitherDatasetGenerator datasetGenerator,
			KeyedPlatesetInfos<T> platesetsToOutput, int numShuffles) 
            : base (datasetGenerator, platesetsToOutput){

            this._NUM_SHUFFLES = numShuffles;
        } //end constructor

        protected override string GenerateRscript() {
            string scriptTemplate = this.DATASET_LOAD_SCRIPT_TEMPLATE + this._RANK_PRODUCT_SCRIPT_TEMPLATE;
            string result = string.Format(scriptTemplate, this.DatasetFilePath, this._NUM_SHUFFLES,
                this.OutputFilePath);
            return result;
        } //end GenerateRscript

        public void GetRankProducts() {
            TextReader inReader = base.Run();
            //TODO: fill in code to return PlatesetInfo<T>
        } //end GetRankProducts
    } //end class
}
