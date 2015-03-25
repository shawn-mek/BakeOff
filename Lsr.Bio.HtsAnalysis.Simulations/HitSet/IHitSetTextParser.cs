using System.Collections.Generic;
using System.IO;

namespace Lsr.Bio.HtsAnalysis.Simulations {
    /// <summary>
    /// Interface defining the method(s) for loading hit set info from a text source
    /// </summary>
    internal interface IHitSetTextParser {
        List<HitSetInfo> LoadHitSetInfo(TextReader reader);
    } //end IHitSetTextParser
}
