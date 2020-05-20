using Kanban.Model;
using Kanban.Model.DbModels;
using Kanban.Model.Models.Request;
using Kanban.Model.Models.Response;
using Kanban.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanban.Service
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userrepo;
        private readonly IRepository<KanbanTask> _kanbantaskrepo;
        private readonly IRepository<UserTask> _usertaskrepo;
        public UserService(IRepository<User> userrepo, IRepository<KanbanTask> taskrepo, IRepository<UserTask> usertaskrepo)
        {
            _userrepo = userrepo;
            _kanbantaskrepo = taskrepo;
            _usertaskrepo = usertaskrepo;
        }

        public async Task<ResultDTO> AddUser(UserWithoutIdVM userVM)
        {
            var result = new ResultDTO()
            {
                Response = null
            };
            try
            {
                await _userrepo.Add(new User
                {
                    Name = userVM.Name,
                    Surname = userVM.Surname
                });
            }
            catch (Exception e)
            {
                result.Response = e.Message;
                return result;
            }
            return result;
        }

        public async Task<ResultDTO> PatchUser(int userId, UserWithoutIdVM userWithoutIdVM)
        {
            var result = new ResultDTO()
            {
                Response = null
            };
            try
            {
                var user = await _userrepo.GetSingleEntity(x => x.Id == userId);
                if (user == null)
                    result.Response = "User not found";
                if (userWithoutIdVM.Name != null)
                    user.Name = userWithoutIdVM.Name;
                if (userWithoutIdVM.Surname != null)
                    user.Surname = userWithoutIdVM.Surname;
                await _userrepo.Patch(user);
            }
            catch (Exception e)
            {
                result.Response = e.Message;
                return result;
            }
            return result;
        }

        public async Task<UserDTO> GetAllUsers()
        {
            var userList = new UserDTO()
            {
                UserList = await _userrepo.GetAll()
            };
            return userList;
        }

        public async Task<ResultDTO> DeleteUser(int userId)
        {
            var result = new ResultDTO()
            {
                Response = null
            };
            try
            {
                var user = await _userrepo.GetSingleEntity(x => x.Id == userId);
                if (user == null)
                    result.Response = "User not found";
                await _userrepo.Delete(user);
            }
            catch (Exception e)
            {
                result.Response = e.Message;
                return result;
            }
            return result;
        }

        //-------------------------------- Funkcje używane do poprzednich etapów projektu -------------------------



        //public async Task<UserDTO> GetSingleUser(int userId)
        //{
        //    var user = new UserDTO()
        //    {
        //        SingleUser = await _repo.GetSingleEntity(x => x.Id == userId)
        //    };
        //    if (user == null)
        //        return null;
        //    return user;
        //}

       

        //public async Task<ResultDTO> AssignTaskToUser(int taskId, int userId)
        //{
        //    var result = new ResultDTO()
        //    {
        //        Response = null
        //    };
        //    try
        //    {
        //        var user = await _repo.GetSingleEntity(x => x.Id == userId);
        //        var kanbanTask = await _taskrepo.GetSingleEntity(y => y.Id == taskId);
        //        if (user != null && kanbanTask != null)
        //        {
        //            var task = await _usertaskrepo.GetSingleEntity(x => x.UserId == userId && x.KanbanTaskId == taskId);
        //            if (task == null)
        //            {
        //                var usertask = new UserTask()
        //                {
        //                    User = user,
        //                    KanbanTask = kanbanTask
        //                };
        //                await _usertaskrepo.Add(usertask);
        //            }
        //            else
        //                result.Response = "Task already assigned to user";
        //        }
        //        else
        //            result.Response = "Task or user not found";
        //    }
        //    catch (Exception e)
        //    {
        //        result.Response = e.Message;
        //        return result;
        //    }
        //    return result;
        //}

        //public async Task<UsersTasksListDTO> GetAllTasksPerUser()
        //{
        //    var list = await _usertaskrepo.GetAll( x => x.User, y => y.KanbanTask);
        //    var newlist = list.GroupBy(x => x.UserId)
        //        .Select(y => new UserTasksDTO
        //        {
        //            User = y.Select(z => z.User).FirstOrDefault(x => x.Id == y.Key),
        //            KanbanTaskList = y.Select(z => z.KanbanTask).OrderBy( x => x.Id).ToList()
        //        }).OrderBy(x => x.User.Id).ToList(); 
        //    var userList = new UsersTasksListDTO()
        //    {
        //        UsersTasksList = newlist
        //    };
        //    return userList;
        //}

        //public async Task<ResultDTO> DeleteTaskFromUser(int userId, int taskId)
        //{
        //    var result = new ResultDTO()
        //    {
        //        Response = null
        //    };
        //    try
        //    {
        //        var task = await _usertaskrepo.GetSingleEntity(x => x.UserId == userId && x.KanbanTaskId == taskId);
        //        if (task == null)
        //            result.Response = "Task is not assigned to this user";
        //        await _usertaskrepo.Delete(task);
        //    }
        //    catch (Exception e)
        //    {
        //        result.Response = e.Message;
        //        return result;
        //    }
        //    return result;
        //}


        //public async Task<TaskWithUserListDTO> GetAllUsersPerTask()
        //{
        //    var list = await _taskrepo.GetAll();
        //    var userTaskList = await _usertaskrepo.GetAll();
        //    List<TaskWithUserDTO> newList = new List<TaskWithUserDTO>();
        //    foreach (KanbanTask task in list)
        //    {
        //        List<UserTask> ifHasUser = new List<UserTask>();
        //        foreach (UserTask checkIfExists in userTaskList)
        //        {
        //            if (checkIfExists.KanbanTaskId == task.Id)
        //                ifHasUser.Add(checkIfExists);
        //        }
        //        if (ifHasUser == null)
        //        {
        //            newList.Add(new TaskWithUserDTO
        //            {
        //                KanbanTask = task,
        //                UserList = new List<UserWithoutIdDTO>()
        //            });
        //        }
        //        else
        //        {
        //            List<UserWithoutIdDTO> finalList = new List<UserWithoutIdDTO>();
        //            foreach (UserTask userTask in userTaskList)
        //            {
        //                if (userTask.KanbanTaskId == task.Id)
        //                {
        //                    var user = await _repo.GetSingleEntity(x => x.Id == userTask.UserId);
        //                    var newUser = new UserWithoutIdDTO
        //                    {
        //                        Name = user.Name,
        //                        Surname = user.Surname
        //                    };
        //                    finalList.Add(newUser);
        //                }
        //            }
        //            newList.Add(new TaskWithUserDTO
        //            {
        //                KanbanTask = task,
        //                UserList = finalList
        //            });
        //        }
        //    }
        //    var userList = new TaskWithUserListDTO()
        //    {
        //        TaskWithUserList = newList
        //    };
        //    return userList;
        //}


    }
}
