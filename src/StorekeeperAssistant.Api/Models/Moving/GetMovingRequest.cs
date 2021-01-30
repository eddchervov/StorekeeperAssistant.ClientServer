using System;
using System.Collections.Generic;
using System.Text;

namespace StorekeeperAssistant.Api.Models.Moving
{
    public class GetMovingRequest
    {
        public int SkipCount { get; set; }
        public int TakeCount { get; set; }
    }
}
