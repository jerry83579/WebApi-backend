using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Collections;


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

        // GET api
        //[Route("get/info")]
        //[HttpGet]
        //public Info Get(int value)
        //{
        //    //從資料庫撈資料
        //    var result = _context.Info;

        //    //篩選資料
        //    var Data = result.Single(w => w.Id == value);

        //    return Data;
        //}


        [Route("get/export")]
        [HttpGet]
        public IEnumerable GetOds()
        {
            //從資料庫撈資料
            var result = _context.Info;

            return result;
        }



        [Route("get/info")]
        [HttpGet]
        public IEnumerable Get()
        {
            //從資料庫撈資料
            var result = _context.Info;

            return result;
        }

        // POST api
        [Route("post/info")]
        [HttpPost]
        public Info Post([FromBody]requestBody value)
        {
            var Data = new Info
            {
                Name = value.Name,
                Address = value.Address,
                Phone = value.Phone,
                Food = value.Food,
                Location = new Point((double)value.Lat, (double)value.Longitude)
                {
                    SRID = 4326
                }
            };
            _context.Info.Add(Data);
            _context.SaveChanges();

            return Data;
        }

        //PUT api
        [Route("put/info/{id}")]
        [HttpPut]
        public Info Put([FromBody]requestBody value, int id)
        {
            var result = _context.Info;

            var Data = result.Single(x => x.Id == id);
            Data.Name = value.Name;
            Data.Phone = value.Phone;
            Data.Address = value.Address;
            Data.Food = value.Food;

            _context.SaveChanges();

            return Data;
        }
        // DELETE api
        [Route("delete/info/{id}")]
        [HttpDelete]
        public Info Delete(int id)
        {
            var result = _context.Info;

            var Data = result.Single(x => x.Id == id);
            _context.Info.Remove(Data);
            _context.SaveChanges();

            return Data;
        }
    }
}
