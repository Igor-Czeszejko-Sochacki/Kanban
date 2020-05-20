using Kanban.Model.DbModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kanban.Model
{
    public class KanbanTask : Entity
    {
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Status { get; set; }
        public int ProgressStatus { get; set; }
        public int Priority { get; set; }
        public bool Blocked { get; set; } = false;
        public string Color { get; set; } = "default";
        public List<Subtask> Subtasks { get; set; }
    }
}

