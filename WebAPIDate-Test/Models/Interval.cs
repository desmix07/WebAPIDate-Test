using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPIDate_Test.Models
{
    public class Interval
    {
        public int Id { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime DueDate { get; set; }
    }
}