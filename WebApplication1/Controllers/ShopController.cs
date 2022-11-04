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
    public class ShopController : ControllerBase
    {
        private readonly Models.ShopController _context;

        public ShopController(Models.ShopController context)
        {
            _context = context;
        }

        /// <summary>
        /// 讀取店家資料
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
        /// 刪除店家資料
        /// </summary>
        /// <param name="ids">單個或多個店家編號，例如: id1,id2,id3...</param>
        [Route("get/info/{ids}")]
        [HttpGet]
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

        /// <summary>
        /// 新增店家資料
        /// </summary>
        /// <param name="value">店家參數</param>
        /// <returns></returns>
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
        /// 修改店家資料
        /// </summary>
        /// <param name="value">店家參數</param>
        /// <param name="id">店家編號</param>
        /// <returns></returns>
        [Route("post/info/{id}")]
        [HttpPost]
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

        /// <summary>
        /// 複合式查詢
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("post/complexQuery")]
        [HttpPost]
        public IEnumerable PostCompoundQuery([FromBody] ComplexQuery value)
        {
            DbSet<Info> result = _context.Info;
            var searchData = result.ToList().Where(x => x.Category.Contains(value.Keyword) || x.Price.ToString().Contains(value.Keyword) || x.Phone.Contains(value.Keyword) || x.Address.Contains(value.Keyword) || (x.Lat.ToString() + "," + x.Longitude).
            Contains(value.Keyword) || string.IsNullOrEmpty(value.Keyword));
            var data = searchData.Where(x => x.Address.Contains(value.Area) || string.IsNullOrEmpty(value.Area)).
            Where(x => x.Category.Contains(value.Category) || string.IsNullOrEmpty(value.Category)).
            Where(x => x.Price < value.Price || value.Price == 0);
            return data;
        }
    }
}