﻿namespace ProjectsSoftuni.Services.Contracts
{
    using System.Threading.Tasks;

    public interface ITeamService
    {
        Task<string> CreteTeam(string teamName, string projectId, string userId);
    }
}