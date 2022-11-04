using NetTopologySuite.Geometries;

namespace WebApplication1.Models
{
    public class ComplexQuery
    {
        /// <summary>
        /// 區域
        /// </summary>
        public string Area { get; set; }

        /// <summary>
        /// 商品種類
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 價格低於
        /// </summary>
        public int Price { get; set; }

        /// <summary>
        /// 關鍵字
        /// </summary>
        public string Keyword { get; set; }
    }
}