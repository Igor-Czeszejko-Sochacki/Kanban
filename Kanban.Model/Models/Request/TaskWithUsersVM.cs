using Kanban.Model.DbModels;
using Kanban.Model.Models.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;


namespace Kanban.Model.Models.Request
{
    public class TaskWithUsersVM : Entity
    {
        public List<UserWithoutIdDTO> UserList { get; set; }
        public List<SubtaskWithoutIdDTO> SubtaskList { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Status { get; set; }
        public string Color { get; set; }
        public bool Blocked { get; set; }
    }
}
