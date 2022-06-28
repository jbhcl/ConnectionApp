using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IUserConnectionRepository
    {
        Task<UserConnectionRequest> GetUserConnectionRequest(int sourceUserId, int likedUserId);   
        Task<AppUser> GetUserWithConnectionRequests(int userId);
        Task<PagedList<ConnectionRequestDto>> GetUserConnectionRequests(ConnectionRequestParams likesParams);
    }
}