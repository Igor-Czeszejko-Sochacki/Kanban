using System;
using System.Collections.Generic;
using System.Text;


namespace Kanban.Model.Models.Response
{
    public class SubtaskWithoutIdDTO
    {
        public string Description { get; set; }
        public bool CompletionStatus { get; set; }
    }
}
