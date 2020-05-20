using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;


namespace Kanban.Model.DbModels
{
    public class Subtask : Entity
    {
        public string Description { get; set; }
        public bool CompletionStatus { get; set; } = false;
        public int KanbanTaskId { get; set; }
        [JsonIgnore]
        public KanbanTask KanbanTask { get; set; }
        
    }
}
