using System;
using System.Collections.Generic;
using System.Text;

namespace Connectors.AI.Gemini.Helper
{
    public class ErrorGemini
    {
        public ErrorInfo error { get; set; }
    }

    public class ErrorInfo
    {
        public int code { get; set; }
        public string message { get; set; }
        public string status { get; set; }
    }
}
