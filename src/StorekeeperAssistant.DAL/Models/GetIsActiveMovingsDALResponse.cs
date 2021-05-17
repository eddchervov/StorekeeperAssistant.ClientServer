using StorekeeperAssistant.DAL.Entities;
using System.Collections.Generic;

namespace StorekeeperAssistant.DAL.Models
{
    public class GetIsActiveMovingsDALResponse
    {
        public int TotalCount { get; set; }
        public List<Moving> Movings { get; set; } = new List<Moving>();
    }
}
