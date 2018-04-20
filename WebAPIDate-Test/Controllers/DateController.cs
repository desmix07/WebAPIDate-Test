using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebAPIDate_Test.Models;

namespace WebAPIDate_Test.Controllers
{
    public class DateController : ApiController
    {
        //получение списка всех элементов в таблице
        public IEnumerable<Interval> GetAllIntervals()
        {
            using (DateContext db = new DateContext())
            {
                var intervalList = db.Intervals.ToList();
                return intervalList;
            }  
        }

        //получение списка элементов, которые пересекаются с заданым интервалом
        public IHttpActionResult GetInterval(DateTime begin, DateTime due)
        {
            using (DateContext db = new DateContext())
            {
                var intervalList = db.Intervals.ToList();
            
                //проверка на пересечение
                var interval = intervalList.Where(u => begin <= u.DueDate && u.BeginDate <= due).ToList();
                if (interval == null)
                {
                    return NotFound();
                }
                return Ok(interval);
            }
        }
        //добавление нового элемента в таблицу
        [HttpPost]
        public IHttpActionResult PostInterval ([FromBody] Interval interval)
        {
            if (ModelState.IsValid)
            {
                using (DateContext db = new DateContext())
                {
                    db.Intervals.Add(interval);
                    db.SaveChanges();
                    return CreatedAtRoute("DefaultApi", new { id = interval.Id }, interval );
                }
            }
            return BadRequest(ModelState);
        }
    }
}
