using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Kanban.Model;
using Kanban.Model.Models.Request;
using Kanban.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Projekt_Kanban.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KanbanTaskController : ControllerBase
    {
        private readonly IKanbanTaskService _kanbanTaskService;

        public KanbanTaskController(IKanbanTaskService kanbanTaskService)
        {
            _kanbanTaskService = kanbanTaskService;
        }

        [HttpPost("AddTaskWithUser")]
        public async Task<IActionResult> AddTaskWithUser(AddTaskWithUserVM taskToUser)
        {
            var result = await _kanbanTaskService.AddTaskWithUser(taskToUser);
            if (result.Response != null)
                return BadRequest(result);
            return Ok("Task was added to user");
        }

        [HttpPatch("PatchTaskStatus")]
        public async Task<IActionResult> PatchStatus(int kanbanTaskId, PatchKanbanTaskStatusVM patchKanbanTaskStatusVM)
        {
            var result = await _kanbanTaskService.PatchStatus(kanbanTaskId, patchKanbanTaskStatusVM);
            if (result.Response != null)
                return BadRequest(result);
            return Ok("Task status was patched");
        }

        [HttpPatch("PatchTaskProgressStatus")]
        public async Task<IActionResult> PatchProgressStatus(int kanbanTaskId, PatchKanbanTaskProgressStatusVM progressStatusVM)
        {
            var result = await _kanbanTaskService.PatchProgressStatus(kanbanTaskId, progressStatusVM);
            if (result.Response != null)
                return BadRequest(result);
            return Ok("Progress status was patched");
        }

        [HttpPatch("PatchTaskWithUser")]
        public async Task<IActionResult> PatchTaskWithUser(TaskWithUsersVM taskToUser)
        {
            var result = await _kanbanTaskService.PatchTaskWithUser(taskToUser);
            if (result.Response != null)
                return BadRequest(result);
            return Ok("Task was patched");
        }

        [HttpGet("GetSingleTask")]
        public async Task<IActionResult> GetSingleKanbanTask(int kanbanTaskId)
        {
            var kanbanTask = await _kanbanTaskService.GetSingleKanbanTask(kanbanTaskId);
            if (kanbanTask == null)
                return BadRequest("Task not found");
            return Ok(kanbanTask);
        }

        [HttpGet("GetTasksByPriority")]
        public async Task<IActionResult> GetTasksByPriority()
        {
            var taskWithProrityList = await _kanbanTaskService.GetTasksByPriority();
            if (taskWithProrityList == null)
                return BadRequest("No tasks to show");
            return Ok(taskWithProrityList);
        }

        [HttpDelete("DeleteTask")]
        public async Task<IActionResult> DeleteKanbanTask(int kanbanTaskId)
        {
            var result = await _kanbanTaskService.DeleteKanbanTask(kanbanTaskId);
            if (result.Response != null)
                return BadRequest("Task not found");
            return Ok("Task was deleted");
        }
    }
}