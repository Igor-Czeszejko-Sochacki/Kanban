﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpPost]
        public async Task<IActionResult> AddKanbanTask(string title, string description, string status)
        {
            await _kanbanTaskService.AddKanbanTask(title, description, status);
            return Ok("Task został dodany");
        }
        [HttpGet]
        public async Task<IActionResult> GetAllKanbanTasks()
        {
            var kanbanTaskList = await _kanbanTaskService.GetAllKanbanTasks();
            return Ok(kanbanTaskList);
        }

        [HttpGet ("BySingleEntity")]
        public async Task<IActionResult> GetSingleKanbanTask(int kanbanTaskId)
        {
            var task = await _kanbanTaskService.GetSingleKanbanTask(kanbanTaskId);
            if (task == null)
            {
                return BadRequest("Task not found");
            }
            return Ok(task);
        }
        //[HttpDelete]
        //public async Task<IActionResult> DeleteKanbanTask(int kanbanTaskId)
        //{
        //    await _kanbanTaskService.DeleteKanbanTask(kanbanTaskId);
        //    return Ok("Usunięto Kanban Taska");
        //}
    }
}