using Kanban.Model.DbModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kanban.Model.Models.Response
{
    public class TaskWIthUsersDTO
    {
        public KanbanTask KanbanTask { get; set; }
        public List<SubtaskWithoutIdDTO> SubtaskList { get; set; }
        public List<User> UserList { get; set; }
    }
}
