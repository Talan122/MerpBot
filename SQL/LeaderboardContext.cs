using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MerpBot.Services;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;

namespace MerpBot.SQL
{
    public class LeaderboardContext : DbContext
    {
        public DbSet<UserData> UserData { get; set; }

        public string DbPath { get; }

        public LeaderboardContext()
        {
            DbPath = Helpers.MakeDirectoryFromWorking("leaderboards.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
        }
    }

    public class UserData
    {
        public SocketUser User { get; set; }
        public int SentMessages { get; set; }
    }
}
