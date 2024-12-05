using AutoMapper;
using LibrarySystem.Api.DTOs;
using LibrarySystem.Api.Errors;
using LibrarySystem.Core.Entitties.Identity;
using LibrarySystem.Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : ApiControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;

        public AdminController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager , IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        [HttpGet("AllUsers")]
        public async Task<ActionResult<UsersResponseDTO>> AllUsers()
        {
            var users = await _userManager.Users.ToListAsync();

            if (users == null || !users.Any())
                return NotFound(new ApiResponse(404, "There are no users"));

            var mappedUsers = new List<UsersReturnDTO>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var mappedUser = _mapper.Map<UsersReturnDTO>(user);
                mappedUser.Roles = roles.ToList();
                mappedUsers.Add(mappedUser);
            }

            var response = new UsersResponseDTO
            {
                TotalUsers = mappedUsers.Count,
                Users = mappedUsers
            };

            return Ok(response);
        }

        [HttpPost("PromoteToRole")]
        public async Task<ActionResult> PromoteToRole(string email, string role)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return NotFound(new ApiResponse(404, "User not found"));

            if (await _userManager.IsInRoleAsync(user, role))
                return BadRequest(new ApiResponse(400, $"User is already an {role}"));

            var validRoles = new[] { "Admin", "Librarian", "User" }; 

            if (!validRoles.Contains(role))
                return BadRequest(new ApiResponse(400, "Invalid Role"));

            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
                return BadRequest(new ApiResponse(400, $"Role '{role}' does not exist"));

            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse(400, $"Failed to promote user to {role}"));

            return Ok(new ApiResponse(200, $"User promoted to {role} successfully!"));
        }

        [HttpGet("GetUsersByRole")]
        public async Task<ActionResult> GetUsersByRole(string role)
        {
            var validRoles = new[] { "Admin", "Librarian", "User" };

            if (!validRoles.Contains(role))
                return BadRequest(new ApiResponse(400, "Invalid Role"));

            var roleExists = await _roleManager.RoleExistsAsync(role);
            if (!roleExists)
                return BadRequest(new ApiResponse(400, $"Role '{role}' does not exist"));

            var usersInRole = await _userManager.GetUsersInRoleAsync(role);

            if (usersInRole == null || usersInRole.Count == 0)
                return NotFound(new ApiResponse(404, "No users found in this role"));

            var mappedUsers = new List<UsersReturnDTO>();
            foreach (var user in usersInRole)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var mappedUser = _mapper.Map<UsersReturnDTO>(user);
                mappedUser.Roles = roles.ToList();
                mappedUsers.Add(mappedUser);
            }

            var response = new UsersResponseDTO
            {
                TotalUsers = mappedUsers.Count,
                Users = mappedUsers
            };

            return Ok(response);
        }


    }


}
