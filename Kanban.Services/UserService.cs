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

    }
}
