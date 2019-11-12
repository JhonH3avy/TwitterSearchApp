using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TweeterSearchApp.Models;

namespace TweeterSearchApp
{
    public class TweetsDbContext : DbContext
    {
        public DbSet<Tweet> Tweets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-AN4MSQE\SQLEXPRESS;Database=TweetDataWarehouse;Trusted_Connection=Yes");
        }
    }
}
