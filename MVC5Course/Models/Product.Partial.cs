namespace MVC5Course.Models
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(ProductMetaData))]
    public partial class Product
    {
    }

    /// <summary>
    /// 模型驗證
    /// </summary>
    public partial class Product : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //可以實作更複雜的商業邏輯
            //已經先將資料載入，所以直接用this就可以取得到資料
            //如果ID不等於defult值(0)，則是更新；反之為新增
            if (this.ProductId == default(int))
            {
                //Create
                //if (this.Stock < 5)
                //{
                    
                //    yield return new ValidationResult("庫存量過低，無法新增商品", new string[] { "Stock" });
                //}
            }
            else
            {
                //Update
            }

            //if (this.Price < 100)
            //{
            //    yield return new ValidationResult("價格設定錯誤", new string[] { "Price" });
            //}
            yield break;
        }
    }
   
    /// <summary>
    /// 自訂驗證屬性
    /// </summary>
    public partial class ProductMetaData
    {
        [Required]
        public int ProductId { get; set; }
        [Required]
        //[產品名稱必須至少包含兩個空白字元(ErrorMessage = "產品名稱必須至少包含兩個空白字元")]
        [StringLength(80, ErrorMessage="欄位長度不得大於 80 個字元")]
        public string ProductName { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<decimal> Stock { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderLine> OrderLine { get; set; }
    }
}
