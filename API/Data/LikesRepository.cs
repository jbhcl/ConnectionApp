using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserConnectionRepository : IUserConnectionRepository
    {
        private readonly DataContext _context;
        public UserConnectionRepository(DataContext context) {
            _context = context;

        }
        public async Task<UserConnectionRequest> GetUserConnectionRequest(int sourceUserId, int likedUserId)
        {
            return await _context.ConnectionRequests.FindAsync(sourceUserId, likedUserId);
        }

        public async Task<PagedList<ConnectionRequestDto>> GetUserConnectionRequests(ConnectionRequestParams likesParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.ConnectionRequests.AsQueryable();

            if (likesParams.predicate == "liked") {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.RequestedUser);
            }

            if (likesParams.predicate == "likedBy") {
                likes = likes.Where(like => like.RequestedUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            var likedUsers = users.Select(user => new ConnectionRequestDto {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url,
                City = user.City,
                Id = user.Id
            });

            return await PagedList<ConnectionRequestDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithConnectionRequests(int userId)
        {
            return await _context.Users
                .Include(x => x.RequestedUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);
        }
    }
}