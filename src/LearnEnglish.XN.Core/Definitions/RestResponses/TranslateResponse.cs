using System.Collections.Generic;

namespace LearnEnglish.XN.Core.Definitions.RestResponses;

public struct TranslateResponse
{
    public int Code { get; set; }
    public IEnumerable<string> Text { get; set; }
}
