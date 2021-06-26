using System.Collections.Generic;

namespace StorekeeperAssistant.Api.Models.Movings
{
    public class GetMovingResponse : BaseResponse
    {
        public int TotalCount { get; set; }
        public IEnumerable<MovingDTO> Movings { get; set; } = new List<MovingDTO>();
    }
}
