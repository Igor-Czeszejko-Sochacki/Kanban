using Kanban.Model;
using Kanban.Model.Models.Request;
using Kanban.Model.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kanban.Services
{
    public interface IKanbanTaskService
    {
        Task<ResultDTO> AddTaskWithUser(AddTaskWithUserVM addTaskWithUserVM);
        Task<ResultDTO> PatchStatus(int kanbanTaskId, PatchKanbanTaskStatusVM patchKanbanTaskStatusVM);
        Task<ResultDTO> PatchProgressStatus(int kanbanTaskId, PatchKanbanTaskProgressStatusVM progressStatusVM);
        Task<ResultDTO> PatchTaskWithUser(TaskWithUsersVM taskToUsersVM);
        Task<TaskWIthUsersDTO> GetSingleKanbanTask(int kanbanTaskId);
        Task<PriorityWithAllTasksListDTO> GetTasksByPriority();
        Task<ResultDTO> DeleteKanbanTask(int kanbanTaskId);
    }
}
