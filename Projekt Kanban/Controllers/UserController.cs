using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kanban.Model.Models.Request;
using Kanban.Model.Models.Response;
using Kanban.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Projekt_Kanban.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost ("AddUser")]
        public async Task<IActionResult> AddUser(UserWithoutIdVM userVM)
        {
            var result = await _userService.AddUser(userVM);
            if (result.Response != null)
                return BadRequest(result);
            return Ok("User was added");
        }

        [HttpPatch ("PatchUser")]
        public async Task<IActionResult> PatchUser(int userId, UserWithoutIdVM userWithoutIdVM)
        {
            var result = await _userService.PatchUser(userId, userWithoutIdVM);
            if (result.Response != null)
                return BadRequest(result);
            return Ok("User was patched");
        }

        [HttpGet("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var userList = await _userService.GetAllUsers();
            if (userList == null)
                return BadRequest("No users to show");
            return Ok(userList);
        }

        [HttpDelete ("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _userService.DeleteUser(userId);
            if (result.Response != null)
                return BadRequest(result);
            return Ok("User was deleted");
        }
    }
}