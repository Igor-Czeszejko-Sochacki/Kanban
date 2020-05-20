using Kanban.Model;
using Kanban.Model.DbModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demcio.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<KanbanTask> KanbanTasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserTask> UserTask { get; set; }
        public DbSet<Subtask> Subtasks { get; set; }
    }
}
