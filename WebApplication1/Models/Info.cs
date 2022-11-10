using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NetTopologySuite.Geometries;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace WebApplication1.Models
{
    /// <summary>
    /// 店家資訊
    /// </summary>
    public partial class Info
    {
        /// <summary>
        /// 店家編號
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 商品種類
        /// </summary>
        [Required]
        public string Category { get; set; }

        /// <summary>
        /// 商品價格
        /// </summary>
        [Required]
        public int Price { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [Required]
        public string Address { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        [Required]
        public string Phone { get; set; }

        /// <summary>
        /// 緯度
        /// </summary>
        [Required]
        public double? Lat { get; set; }

        /// <summary>
        /// 經度
        /// </summary>
        [Required]
        public double? Longitude { get; set; }

        /// <summary>
        /// 座標
        /// </summary>
        public Geometry Location { get; set; }

        /// <summary>
        /// 圖片路徑
        /// </summary>
        public string ImgUrl { get; set; }
    }
}