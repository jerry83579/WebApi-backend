using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Geometries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http.Results;
using WebApplication1.Models;
using static System.Net.WebRequestMethods;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly Models.ShopController _context;
        private readonly IConfiguration _config;
        private readonly Config _memberConfig;

        public ShopController(Models.ShopController context, IConfiguration config)
        {
            _context = context;
            _config = config;
            _memberConfig = new Config
            {
                ImgPath = _config.GetValue<string>("Path:ImgPath"),
                NotFoundPath = _config.GetValue<string>("Path:NotFoundPath")
            };
        }

        /// <summary>
        /// 預覽照片
        /// </summary>
        /// <param name="id">圖片對應的店家編號</param>
        [Route("get/previewImg/{id}")]
        [HttpGet]
        public IActionResult GetPreviewImg(int id)
        {
            DbSet<Info> result = _context.Info;
            var data = result.Single(x => x.Id == id);
            string imgUrl = data.ImgUrl;
            string path = string.Format(@"{0}\{1}", _memberConfig.ImgPath, imgUrl);
            if (System.IO.File.Exists(path))
            {
                var image = System.IO.File.OpenRead(path);
                return File(image, "image/jpeg");
            }
            else
            {
                string notFoundPath = string.Format(@"{0}", _memberConfig.NotFoundPath);
                var image = System.IO.File.OpenRead(notFoundPath);
                return File(image, "image/jpeg");
            }
        }

        /// <summary>
        /// 刪除照片
        /// </summary>
        /// <param name="id">圖片對應的店家編號</param>
        [Route("deleteImg/{id}")]
        [HttpGet]
        public IActionResult DeleteImg(int id)
        {
            DbSet<Info> result = _context.Info;
            Info Data = result.Single(x => x.Id == id);
            string path = string.Format(@"{0}\{1}", _memberConfig.ImgPath, Data.ImgUrl);
            System.IO.File.Delete(path);
            Data.ImgUrl = null;
            _context.SaveChanges();
            string notFoundPath = string.Format(@"{0}", _memberConfig.NotFoundPath);
            var image = System.IO.File.OpenRead(notFoundPath);
            return File(image, "image/jpeg");
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
        /// 上傳照片
        /// </summary>
        /// <param name="files">表單數據</param>
        /// <param name="id">店家編號</param>
        /// <returns></returns>
        [Route("post/uploadImg/{id}")]
        [HttpPost]
        public async Task<FileStreamResult> UploadImgAsync([FromForm] IFormFile files, int id)
        {
            DbSet<Info> result = _context.Info;
            // 讀取照片資料流存取並創建照片到伺服器位置
            string extension = Path.GetExtension(files.FileName);
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            string path = string.Format(@"{0}\{1}{2}", _memberConfig.ImgPath, fileName, extension);
            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await files.CopyToAsync(stream);
                }

                var data = result.Single(x => x.Id == id);
                // 刪除舊的照片
                if (!string.IsNullOrEmpty(data.ImgUrl))
                {
                    System.IO.File.Delete(string.Format(@"{0}\{1}", _memberConfig.ImgPath, data.ImgUrl));
                }
                // 上傳新的照片路徑到資料庫
                data.ImgUrl = string.Format(@"{0}{1}", fileName, extension);
                _context.SaveChanges();
                // 回傳照片
                var image = System.IO.File.OpenRead(path);
                return File(image, "image/jpeg");
            }
            catch
            {
                path = string.Format(@"{0}\{1}", _memberConfig.NotFoundPath);
                var image = System.IO.File.OpenRead(path);
                return File(image, "image/jpeg");
            }
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