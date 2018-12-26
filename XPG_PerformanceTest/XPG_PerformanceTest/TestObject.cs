using System;
using System.Runtime.Serialization;

namespace XPG_PerformanceTest {
    [Serializable]
    public class TestObject {
        public TestObject() {
        }
        public string Product { get; set; }
        public string Subcategory { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public int Quantity { get; set; }       

        
    }
}