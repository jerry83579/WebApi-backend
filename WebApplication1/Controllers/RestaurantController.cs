using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections;
using System.Collections.Generic;
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

        /// <summary>
        /// 複合式查詢
        /// </summary>
        /// <returns></returns>
        [Route("post/complexQuery")]
        [HttpPost]
        public IEnumerable GetCompoundQuery([FromBody] string[] queryArray)
        {
            DbSet<Info> result = _context.Info;
            var price = queryArray[2] == "0" ? 0 : Convert.ToInt32(queryArray[3]);
            var searchData = result.ToList().Where(x => x.Category.Contains(queryArray[0]) || x.Price.ToString().Contains(queryArray[0]) || x.Phone.Contains(queryArray[0]) || x.Address.Contains(queryArray[0]) || (x.Lat.ToString() + "," + x.Longitude).Contains(queryArray[0]) || queryArray[0] == "default");
            var data = searchData.Where(x => x.Address.Contains(queryArray[1]) || queryArray[1] == "default").
            Where(x => x.Category.Contains(queryArray[2]) || queryArray[2] == "default").
            Where(x => x.Price < price || price == 0);
            return data;
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