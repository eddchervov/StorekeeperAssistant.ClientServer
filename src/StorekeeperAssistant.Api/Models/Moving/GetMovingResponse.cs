using System.Collections.Generic;

namespace StorekeeperAssistant.Api.Models.Moving
{
    public class GetMovingResponse : BaseResponse
    {
        public int TotalCount { get; set; }
        public List<MovingModel> MovingModels { get; set; } = new List<MovingModel>();
    }
}
