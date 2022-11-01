using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections;
using System.Linq;
using WebApplication1.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly RestaurantContext _context;

        public RestaurantController(RestaurantContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 取得餐廳資訊
        /// </summary>
        /// <returns></returns>
        [Route("get/info")]
        [HttpGet]
        public IEnumerable Get()
        {
            //從資料庫撈資料
            DbSet<Info> result = _context.Info;
            return result;
        }

        // POST api
        [Route("post/info")]
        [HttpPost]
        public Info Post([FromBody] Info value)
        {
            Info Data = new Info
            {
                Category = value.Category,
                Address = value.Address,
                Phone = value.Phone,
                Price = value.Price,
                Longitude = value.Longitude,
                Lat = value.Lat,
                Location = new Point((double)value.Lat, (double)value.Longitude)
                {
                    SRID = 3826
                }
            };
            _context.Info.Add(Data);
            _context.SaveChanges();
            return Data;
        }

        /// <summary>
        /// 複合式查詢
        /// </summary>
        /// <returns></returns>
        [Route("get/CompoundQueryData/{value}")]
        [HttpGet]
        public IEnumerable GetCompoundQueryData(string value)
        {
            DbSet<Info> result = _context.Info;
            string[] roomvalue = value.Split(",");
            var data = result.Where(x => x.Address.Contains(roomvalue[0]))
           .Where(x => x.Category.Contains(roomvalue[1]) && !String.IsNullOrEmpty(roomvalue[1]));
            return data;
        }

        //PUT api
        [Route("put/info/{id}")]
        [HttpPut]
        public Info Put([FromBody] Info value, int id)
        {
            DbSet<Info> result = _context.Info;
            Info Data = result.Single(x => x.Id == id);
            Data.Category = value.Category;
            Data.Phone = value.Phone;
            Data.Address = value.Address;
            Data.Price = value.Price;
            Data.Longitude = value.Longitude;
            Data.Lat = value.Lat;
            Data.Location = new Point((double)value.Lat, (double)value.Longitude)
            {
                SRID = 3826
            };
            _context.SaveChanges();
            return Data;
        }

        // DELETE api
        [Route("delete/info/{ids}")]
        [HttpDelete]
        public void Delete(string ids)
        {
            string[] roomIds = ids.Split(",");
            DbSet<Info> result = _context.Info;
            foreach (string id in roomIds)
            {
                Info Data = result.Single(x => x.Id == Convert.ToInt32(id));
                _context.Info.Remove(Data);
            }
            _context.SaveChanges();
        }
    }
}