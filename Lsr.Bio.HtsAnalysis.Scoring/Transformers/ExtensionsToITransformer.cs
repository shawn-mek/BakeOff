using System.Linq;
using Lsr.Bio.HtsAnalysis.Core;

namespace Lsr.Bio.HtsAnalysis.Scoring.Transformers {
	/// <summary>
	/// Class containing extension methods for objects having an ITransformer interface.
	/// </summary>
	/// <remarks>Usually a class like this would be named [base]Extensions, but in this case that would be
	/// ITransformerExtensions, which looks like the extension class itself is an interface ... hence the 
	/// nonstandard name.</remarks>
    public static class ExtensionsToITransformer {
		/// <summary>
		/// Extension method that allows an ITransformer to transform a single PlatesetInfo rather than
		/// the usual TypedPlatesetDictionary of PlatesetInfos.
		/// </summary>
		/// <typeparam name="TInput">The Type of data contained in the input PlatesetInfo</typeparam>
		/// <typeparam name="TOutput">The Type of data contained in the output PlatesetInfo</typeparam>
		/// <param name="transformer">An instantiated object implementing ITransformer</param>
		/// <param name="value">A filled PlatesetInfo to transform</param>
		/// <returns>A filled, transformed output PlatesetInfo</returns>
        public static PlatesetInfo<TOutput> Transform<TInput, TOutput>(this ITransformer<TInput, TOutput> transformer, PlatesetInfo<TInput> value) {
			KeyedPlatesetInfos<TInput> inputs = new KeyedPlatesetInfos<TInput>();
			inputs.Add(value);
			KeyedPlatesetInfos<TOutput> output = transformer.Transform(inputs);
            PlatesetInfo<TOutput> result = output.First().Value.CastToPlatesetInfo<TOutput>();
            return result;
        }
    }
}
