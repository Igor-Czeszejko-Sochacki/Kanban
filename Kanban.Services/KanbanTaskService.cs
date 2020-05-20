using Kanban.Model;
using Kanban.Model.DbModels;
using Kanban.Model.Models.Request;
using Kanban.Model.Models.Response;
using Kanban.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kanban.Services
{
    public class KanbanTaskService : IKanbanTaskService
    {
        private readonly IRepository<KanbanTask> _kanbantaskrepo;
        private readonly IRepository<UserTask> _usertaskrepo;
        private readonly IRepository<User> _userrepo;
        private readonly IRepository<Subtask> _subtaskrepo;

        public KanbanTaskService(IRepository<KanbanTask> taskrepo, IRepository<UserTask> usertaskrepo, IRepository<User> userrepo, IRepository<Subtask> subtaskrepo)
        {
            _kanbantaskrepo = taskrepo;
            _userrepo = userrepo;
            _usertaskrepo = usertaskrepo;
            _subtaskrepo = subtaskrepo;
        }

        public async Task<ResultDTO> AddTaskWithUser(AddTaskWithUserVM addTaskWithUserVM)
        {
            var result = new ResultDTO()
            {
                Response = null
            };
            try
            {
                var minPriority = 1;
                var maxPriority = 4;
                var task = (new KanbanTask
                {
                    Title = addTaskWithUserVM.Title,
                    Description = addTaskWithUserVM.Description,
                    Status = addTaskWithUserVM.Status,
                    Priority = addTaskWithUserVM.Priority,
                    Color = addTaskWithUserVM.Color,
                    Blocked = addTaskWithUserVM.Blocked
                });
                if (task.Priority < minPriority || task.Priority > maxPriority)
                {
                    result.Response = "Invalid Priority";
                }
                else
                {
                    await _kanbantaskrepo.Add(task);
                    var userList = addTaskWithUserVM.UserList;
                    var subtaskList = addTaskWithUserVM.SubtaskList;
                    foreach (UserWithoutIdDTO user in userList)
                    {
                        var findUser = await _userrepo.GetSingleEntity(x => x.Name == user.Name && x.Surname == user.Surname);
                        var usertask = new UserTask()
                        {
                            User = findUser,
                            KanbanTask = task
                        };
                        await _usertaskrepo.Add(usertask);
                    }
                    foreach (SubtaskWithoutIdDTO subtask in subtaskList)
                    {
                        var newSubtask = new Subtask()
                        {
                            Description = subtask.Description,
                            CompletionStatus = subtask.CompletionStatus,
                            KanbanTaskId = task.Id
                        };
                        await _subtaskrepo.Add(newSubtask);
                    }
                }
            }
            catch (Exception e)
            {
                result.Response = e.Message;
                return result;
            }
            return result;
        }

        public async Task<ResultDTO> PatchStatus(int kanbanTaskId, PatchKanbanTaskStatusVM patchKanbanTaskStatusVM)
        {
            var result = new ResultDTO()
            {
                Response = null
            };
            try
            {
                var kanbanTask = await _kanbantaskrepo.GetSingleEntity(x => x.Id == kanbanTaskId);
                if (kanbanTask == null)
                    result.Response = "Task not found";
                if (patchKanbanTaskStatusVM.Status != null)
                    kanbanTask.Status = patchKanbanTaskStatusVM.Status;
                await _kanbantaskrepo.Patch(kanbanTask);
            }
            catch (Exception e)
            {
                result.Response = e.Message;
                return result;
            }
            return result;
        }

        public async Task<ResultDTO> PatchProgressStatus(int kanbanTaskId, PatchKanbanTaskProgressStatusVM progressStatusVM)
        {
            var result = new ResultDTO()
            {
                Response = null
            };
            try
            {
                var kanbanTask = await _kanbantaskrepo.GetSingleEntity(x => x.Id == kanbanTaskId);
                if (kanbanTask == null)
                    result.Response = "Task not found";
                if (progressStatusVM.ProgressStatus < 6)
                    kanbanTask.ProgressStatus = progressStatusVM.ProgressStatus;
                else
                    result.Response = "Progress Status not found";
                await _kanbantaskrepo.Patch(kanbanTask);
            }
            catch (Exception e)
            {
                result.Response = e.Message;
                return result;
            }
            return result;
        }
        public async Task<ResultDTO> PatchTaskWithUser(TaskWithUsersVM taskToUsersVM)
        {
            var result = new ResultDTO()
            {
                Response = null
            };
            try
            {
                var task = await _kanbantaskrepo.GetSingleEntity(x => x.Id == taskToUsersVM.Id);
                if (task != null)
                {
                    if (taskToUsersVM.Title != null)
                        task.Title = taskToUsersVM.Title;
                    if (taskToUsersVM.Description != null)
                        task.Description = taskToUsersVM.Description;
                    if (taskToUsersVM.Status != null)
                        task.Status = taskToUsersVM.Status;
                    if (taskToUsersVM.Color != null)
                        task.Color = taskToUsersVM.Color;
                    if (taskToUsersVM.Blocked != null)
                        task.Blocked = taskToUsersVM.Blocked;
                    await _kanbantaskrepo.Patch(task);

                    var userTaskList = await _usertaskrepo.GetAll();
                    foreach (UserTask userTask in userTaskList)
                    {
                        if (userTask.KanbanTaskId == task.Id)
                        {
                            await _usertaskrepo.Delete(userTask);
                        }
                    }
                    foreach (UserWithoutIdDTO user in taskToUsersVM.UserList)
                    {
                        var findUser = await _userrepo.GetSingleEntity(x => x.Name == user.Name && x.Surname == user.Surname);
                        if (findUser != null)
                        {
                            var usertask = new UserTask()
                            {
                                User = findUser,
                                KanbanTask = task
                            };
                            await _usertaskrepo.Add(usertask);
                        }
                        else
                            result.Response = "Task was patched, but one of the users does not exist";
                    }

                    var subtaskList = await _subtaskrepo.GetAll();
                    foreach (Subtask subtask in subtaskList)
                    {
                        if (subtask.KanbanTaskId == task.Id)
                        {
                            await _subtaskrepo.Delete(subtask);
                        }
                    }
                    foreach (SubtaskWithoutIdDTO subtask in taskToUsersVM.SubtaskList)
                    { 
                            var newSubtask = new Subtask()
                            {
                                Description = subtask.Description,
                                CompletionStatus = subtask.CompletionStatus,
                                KanbanTaskId = task.Id
                            };
                            await _subtaskrepo.Add(newSubtask);
                    }

                }
                else
                    result.Response = "Task does not exist";
            }
            catch (Exception e)
            {
                result.Response = e.Message;
                return result;
            }
            return result;
        }

        public async Task<TaskWIthUsersDTO> GetSingleKanbanTask(int kanbanTaskId)
        {
            var kanbanTask = await _kanbantaskrepo.GetSingleEntity(x => x.Id == kanbanTaskId);
            if (kanbanTask == null)
                return null;
            else
            {
                var userTaskList = await _usertaskrepo.GetAll();
                var usersList = new List<User>();
                var subtaskList = await _subtaskrepo.GetAll();
                var finalSubtaskList = new List<SubtaskWithoutIdDTO>();
                foreach (UserTask userTask in userTaskList)
                {
                    if (userTask.KanbanTaskId == kanbanTaskId)
                    {
                        var user = await _userrepo.GetSingleEntity(x => x.Id == userTask.UserId);
                        usersList.Add(user);
                    }
                }
                foreach (Subtask subtask in subtaskList)
                {
                    if (subtask.KanbanTaskId == kanbanTaskId)
                    {
                        var newSubtask = new SubtaskWithoutIdDTO()
                        {
                            Description = subtask.Description,
                            CompletionStatus = subtask.CompletionStatus
                        };
                        finalSubtaskList.Add(newSubtask);
                    }
                }
                var finalKanbanTask = new TaskWIthUsersDTO()
                {
                    KanbanTask = kanbanTask,
                    SubtaskList = finalSubtaskList,
                    UserList = usersList           
                };
                return finalKanbanTask;
            }
        }

        public async Task<PriorityWithAllTasksListDTO> GetTasksByPriority()
        {
            var maxPriority = 4;
            var minPriority = 1;
            var kanbanTaskList = await _kanbantaskrepo.GetAll();
            List<PriorityWithAllTasksDTO> priorityWithAllTasksListList = new List<PriorityWithAllTasksDTO>();
            var userTaskList = await _usertaskrepo.GetAll();
            var subtaskList = await _subtaskrepo.GetAll();
            for (var i = minPriority; i <= maxPriority; i++)
            {
                PriorityWithAllTasksDTO priorityWithAllTasks = (new PriorityWithAllTasksDTO
                {
                    Priority = i,
                    KanbanTasksList = new List<AllTasksWithSamePriorityDTO>(),
                });
                foreach (KanbanTask task in kanbanTaskList)
                {
                    if (task.Priority == i)
                    {
                        List<User> userList = new List<User>();
                        foreach (UserTask userTask in userTaskList)
                        {
                            if (userTask.KanbanTaskId == task.Id)
                            {
                                var user = await _userrepo.GetSingleEntity(x => x.Id == userTask.UserId);
                                userList.Add(user);
                            }
                        }
                        List<SubtaskWithoutIdDTO> finalSubtaskList = new List<SubtaskWithoutIdDTO>();
                        foreach (Subtask subtask in subtaskList)
                        {
                            if (subtask.KanbanTaskId == task.Id)
                            {
                                var finalSubtask = new SubtaskWithoutIdDTO()
                                {
                                    Description = subtask.Description,
                                    CompletionStatus = subtask.CompletionStatus
                                };
                                finalSubtaskList.Add(finalSubtask);
                            }
                        }
                        var temp = new AllTasksWithSamePriorityDTO()
                        {
                            Id = task.Id,
                            Title = task.Title,
                            Description = task.Description,
                            Status = task.Status,
                            ProgressStatus = task.ProgressStatus,
                            Blocked = task.Blocked,
                            Color = task.Color,
                            UserList = userList,
                            SubtaskList = finalSubtaskList
                        };
                        priorityWithAllTasks.KanbanTasksList.Add(temp);
                    }
                }
                priorityWithAllTasksListList.Add(priorityWithAllTasks);
            }
            var allPriorityWithAllTasksList = new PriorityWithAllTasksListDTO()
            {
                TasksList = priorityWithAllTasksListList
            };
            return allPriorityWithAllTasksList;
        }
        public async Task<ResultDTO> DeleteKanbanTask(int kanbanTaskId)
        {
            var result = new ResultDTO()
            {
                Response = null
            };
            try
            {
                var kanbanTask = await _kanbantaskrepo.GetSingleEntity(x => x.Id == kanbanTaskId);
                if (kanbanTask == null)
                    result.Response = "Task not found";
                await _kanbantaskrepo.Delete(kanbanTask);
            }
            catch (Exception e)
            {
                result.Response = e.Message;
                return result;
            }
            return result;
        }
        
       

        

       

        //-------------------------------- Funkcje używane do poprzednich etapów projektu -------------------------



        //public async Task<ResultDTO> AddKanbanTask(KanbanTaskVM addKanbanTaskVM)
        //{
        //    var result = new ResultDTO()
        //    {
        //        Response = null
        //    };
        //    try
        //    {
        //        await _repo.Add(new KanbanTask
        //        {
        //            Title = addKanbanTaskVM.Title,
        //            Description = addKanbanTaskVM.Description,
        //            Status = addKanbanTaskVM.Status
        //        });
        //    }
        //    catch (Exception e)
        //    {
        //        result.Response = e.Message;
        //        return result;
        //    }
        //    return result;
        //}
        //public async Task<KanbanTaskDTO> GetAllKanbanTasks()
        //{
        //    var kanbanTaskList = new KanbanTaskDTO()
        //    {
        //        KanbanList = await _repo.GetAll()
        //    };
        //    return kanbanTaskList;
        //}

        //public async Task<ResultDTO> PatchKanbanTask(int kanbanTaskId, KanbanTaskVM patchKanbanTaskVM)
        //{
        //    var result = new ResultDTO()
        //    {
        //        Response = null
        //    };
        //    try
        //    {
        //        var kanbanTask = await _repo.GetSingleEntity(x => x.Id == kanbanTaskId);
        //        if (kanbanTask == null)
        //            result.Response = "Task not found";
        //        if (patchKanbanTaskVM.Title != null)
        //            kanbanTask.Title = patchKanbanTaskVM.Title;
        //        if (patchKanbanTaskVM.Description != null)
        //            kanbanTask.Description = patchKanbanTaskVM.Description;
        //        if (patchKanbanTaskVM.Status != null)
        //            kanbanTask.Status = patchKanbanTaskVM.Status;
        //        await _repo.Patch(kanbanTask);
        //    }
        //    catch (Exception e)
        //    {
        //        result.Response = e.Message;
        //        return result;
        //    }
        //    return result;
        //}

        //public async Task<ResultDTO> AddKanbanTaskWithPriority(KanbanTaskWithPriorityVM kanbanTaskWithPriorityVM)
        //{
        //    var result = new ResultDTO()
        //    {
        //        Response = null
        //    };
        //    try
        //    {
        //        KanbanTask task = (new KanbanTask
        //        {
        //            Title = kanbanTaskWithPriorityVM.Title,
        //            Description = kanbanTaskWithPriorityVM.Description,
        //            Status = kanbanTaskWithPriorityVM.Status,
        //            Priority = kanbanTaskWithPriorityVM.Priority
        //        });

        //        if (task.Priority < 1 || task.Priority >4)
        //        {
        //            result.Response = "Invalid Priority";
        //        }
        //        else
        //            await _repo.Add(task);
        //    }
        //    catch (Exception e)
        //    {
        //        result.Response = e.Message;
        //        return result;
        //    }
        //    return result;
        //}

        //public async Task<ResultDTO> PatchBlockedStatus(int kanbanTaskId, bool blockedStatus)
        //{
        //    var result = new ResultDTO()
        //    {
        //        Response = null
        //    };
        //    try
        //    {
        //        var kanbanTask = await _repo.GetSingleEntity(x => x.Id == kanbanTaskId);
        //        if (kanbanTask == null)
        //            result.Response = "Task not found";
        //        kanbanTask.Blocked = blockedStatus;
        //        await _repo.Patch(kanbanTask);
        //    }
        //    catch (Exception e)
        //    {
        //        result.Response = e.Message;
        //        return result;
        //    }
        //    return result;
        //}


        //public async Task<ResultDTO> PatchColor(int kanbanTaskId, string color)
        //{
        //    var result = new ResultDTO()
        //    {
        //        Response = null
        //    };
        //    try
        //    {
        //        var kanbanTask = await _repo.GetSingleEntity(x => x.Id == kanbanTaskId);
        //        if (kanbanTask == null)
        //            result.Response = "Task not found";
        //        kanbanTask.Color = color;
        //        await _repo.Patch(kanbanTask);
        //    }
        //    catch (Exception e)
        //    {
        //        result.Response = e.Message;
        //        return result;
        //    }
        //    return result;
        //}
    }
}