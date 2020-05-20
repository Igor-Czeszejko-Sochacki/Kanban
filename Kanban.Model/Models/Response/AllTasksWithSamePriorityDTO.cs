using Kanban.Model.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kanban.Model.Models.Response
{
    public class AllTasksWithSamePriorityDTO : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string Status { get; set; }
        public int ProgressStatus { get; set; }
        public bool Blocked { get; set; }
        public string Color { get; set; }
        public List<User> UserList { get; set; }
        public List<SubtaskWithoutIdDTO> SubtaskList { get; set; }

    }
}
