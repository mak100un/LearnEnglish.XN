using System;

namespace LearnEnglish.XN.Core.Definitions.Exceptions;

public class LogicException : Exception
{
    public LogicException(string message)
        : base(message)
    {

    }
}
