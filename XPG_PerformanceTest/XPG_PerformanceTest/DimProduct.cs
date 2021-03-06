//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XPG_PerformanceTest
{
    using System;
    using System.Collections.Generic;
    
    public partial class DimProduct
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DimProduct()
        {
            this.FactSales = new HashSet<FactSale>();
        }
    
        public int ProductKey { get; set; }
        public string ProductLabel { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public Nullable<int> ProductSubcategoryKey { get; set; }
        public string Manufacturer { get; set; }
        public string BrandName { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public string StyleID { get; set; }
        public string StyleName { get; set; }
        public string ColorID { get; set; }
        public string ColorName { get; set; }
        public string Size { get; set; }
        public string SizeRange { get; set; }
        public string SizeUnitMeasureID { get; set; }
        public Nullable<double> Weight { get; set; }
        public string WeightUnitMeasureID { get; set; }
        public string UnitOfMeasureID { get; set; }
        public string UnitOfMeasureName { get; set; }
        public string StockTypeID { get; set; }
        public string StockTypeName { get; set; }
        public Nullable<decimal> UnitCost { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<System.DateTime> AvailableForSaleDate { get; set; }
        public Nullable<System.DateTime> StopSaleDate { get; set; }
        public string Status { get; set; }
        public string ImageURL { get; set; }
        public string ProductURL { get; set; }
        public Nullable<int> ETLLoadID { get; set; }
        public Nullable<System.DateTime> LoadDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    
        public virtual DimProductSubcategory DimProductSubcategory { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FactSale> FactSales { get; set; }
    }
}
