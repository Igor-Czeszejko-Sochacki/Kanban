﻿using System;
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
        [HttpPost]
        public async Task<IActionResult> AddUser(UserVM userVM)
        {
            var result = await _userService.AddUser(userVM);
            if (result.Response != null)
                return BadRequest(result);
            return Ok("User was added");
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllUsers()
        {
            var userList = await _userService.GetAllUsers();
            if (userList == null)
                return BadRequest("No users to show");
            return Ok(userList);
        }
        [HttpGet("GetSingleUser")]
        public async Task<IActionResult> GetSingleUser(int userId)
        {
            var singleUser = await _userService.GetSingleUser(userId);
            if (singleUser.SingleUser == null)
                return BadRequest("User not found");
            return Ok(singleUser);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int userId)
        {
            var result = await _userService.DeleteUser(userId);
            if (result.Response != null)
                return BadRequest(result);
            return Ok("User was deleted");
        }
        [HttpPatch]
        public async Task<IActionResult> PatchUser(int userId, UserVM userVM)
        {
            var result = await _userService.PatchUser(userId, userVM);
            if (result.Response != null)
                return BadRequest(result);
            return Ok("User was patched");
        }
        [HttpPost("AssignTaskToUser")]
        public async Task<IActionResult> AssignTaskToUser(int taskId, int userId)
        {
            var result = await _userService.AssignTaskToUser(taskId, userId);
            if (result.Response != null)
                return BadRequest(result);
            return Ok("Task was assigned to user");
        }
        [HttpGet("AllTasksPerUser")]
        public async Task<IActionResult> GetAllTasksPerUser()
        {
            var userTaskList = await _userService.GetAllTasksPerUser();
            if (userTaskList == null)
                return BadRequest("No users to show");
            return Ok(userTaskList);
        }
        [HttpDelete("DeleteTaskFromUser")]
        public async Task<IActionResult> DeleteTaskFromUser(int userId, int taskId)
        {
            var result = await _userService.DeleteTaskFromUser(userId, taskId);
            if (result.Response != null)
                return BadRequest(result);
            return Ok("Task was deleted from user");
        }
        [HttpPost("AddTaskWithUser")]
        public async Task<IActionResult> AddTaskWithUser(TaskToUserVM taskToUser)
        {
            var result = await _userService.AddTaskWithUser(taskToUser);
            if (result.Response != null)
                return BadRequest(result);
            return Ok("Task was added to user");
        }

        [HttpGet("AllUsersPerTask")]
        public async Task<IActionResult> GetAllUsersPerTask()
        {
            var userTaskList = await _userService.GetAllUsersPerTask();
            if (userTaskList == null)
                return BadRequest("No tasks to show");
            return Ok(userTaskList);
        }

        [HttpPatch("PatchTaskWithUser")]
        public async Task<IActionResult> PatchTaskWithUser(TaskWithUsersVM taskToUser)
        {
            var result = await _userService.PatchTaskWithUser(taskToUser);
            if (result.Response != null)
                return BadRequest(result);
            return Ok("Task was patched");
        }

    }
}