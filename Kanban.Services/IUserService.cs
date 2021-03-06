﻿using Kanban.Model.DbModels;
using Kanban.Model.Models.Request;
using Kanban.Model.Models.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kanban.Service
{
    public interface IUserService
    {
        Task<ResultDTO> AddUser(UserWithoutIdVM userVM);
        Task<ResultDTO> PatchUser(int userId, UserWithoutIdVM userWithoutIdVM);
        Task<UserDTO> GetAllUsers(); 
        Task<ResultDTO> DeleteUser(int userId);

        //------------------------------ Interfejsy używane do poprzednich etapów projektu --------------------------


        // Task<UserDTO> GetSingleUser(int userId);
        // Task<ResultDTO> PatchUser(int userId, UserVM userVM);
        // Task<ResultDTO> AssignTaskToUser(int taskId, int userId);
        //  Task<UsersTasksListDTO> GetAllTasksPerUser();
        //   Task<ResultDTO> DeleteTaskFromUser(int userId, int taskId);
        //  Task<TaskWithUserListDTO> GetAllUsersPerTask();

    }
}
