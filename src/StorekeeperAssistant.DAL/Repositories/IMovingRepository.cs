using StorekeeperAssistant.DAL.Entities;
using StorekeeperAssistant.DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StorekeeperAssistant.DAL.Repositories
{
    public interface IMovingRepository
    {
        Task<GetIsActiveMovingsDALResponse> GetIsActiveMovingsAsync(int skip, int take);
    }
}
