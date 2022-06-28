using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class UserConnectionsController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserConnectionsController (IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;
        }
        
        [HttpPost("{username}")]
        public async Task<ActionResult> AddUserConnectionRequest(string username) {
            var sourceUserId = User.GetUserId();
            var requestedUser = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            var sourceUser = await _unitOfWork.UserConnectionRepository.GetUserWithConnectionRequests(sourceUserId);

            if (requestedUser == null) return NotFound();

            if (sourceUser.UserName == username) return BadRequest("Can you not do that");

            var userConnectionRequest = await _unitOfWork.UserConnectionRepository.GetUserConnectionRequest(sourceUserId, requestedUser.Id);

            if (userConnectionRequest != null) return BadRequest("You already like this user");

            userConnectionRequest = new UserConnectionRequest
            {
                SourceUserId = sourceUserId,
                RequestedUserId = requestedUser.Id
            };

            sourceUser.RequestedUsers.Add(userConnectionRequest);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to like user");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConnectionRequestDto>>> GetUserConnectionRequests([FromQuery]ConnectionRequestParams connectionRequestParams) {
            connectionRequestParams.UserId = User.GetUserId();
            var users = await _unitOfWork.UserConnectionRepository.GetUserConnectionRequests(connectionRequestParams);

            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPage);

            return Ok(users);
        }
    }
}