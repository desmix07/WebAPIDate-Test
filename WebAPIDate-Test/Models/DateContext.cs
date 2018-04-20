using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace WebAPIDate_Test.Models
{
    public class DateContext : DbContext
    {
        public DbSet <Interval> Intervals { get; set; }
    }
}