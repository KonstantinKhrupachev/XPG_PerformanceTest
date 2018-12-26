using DevExpress.PerformanceTests.PerfomanceMonitor;
using DevExpress.XtraPivotGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XPG_PerformanceTest {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
        }

        TestObject[] dataSource;
        Stopwatch sw = new Stopwatch();
        StringBuilder sb = new StringBuilder();
        List<TestAction> testActions = new List<TestAction>();
        int testIndex = 0;
        private void Form1_Load(object sender, EventArgs e) {
            pivotGridControl1.OptionsData.DataProcessingEngine = PivotDataProcessingEngine.LegacyOptimized;
            InitActions();
            this.BeginInvoke((Action)(() => PrepareData()));
        }

        private void InitActions() {

            Action configure_6x2 = () => {
                pivotGridControl1.BeginUpdate();
                pivotGridControl1.Fields.Clear();

                pivotGridControl1.Fields.Add("Category", DevExpress.XtraPivotGrid.PivotArea.RowArea);
                pivotGridControl1.Fields.Add("Subcategory", DevExpress.XtraPivotGrid.PivotArea.RowArea);
                pivotGridControl1.Fields.Add("Product", DevExpress.XtraPivotGrid.PivotArea.RowArea);
                pivotGridControl1.Fields.Add(new PivotGridField("Date", PivotArea.ColumnArea) { GroupInterval = PivotGroupInterval.DateYear, Name = "fieldYear", Caption = "Year" });
                pivotGridControl1.Fields.Add(new PivotGridField("Date", PivotArea.ColumnArea) { GroupInterval = PivotGroupInterval.DateQuarter, Name = "fieldQuarter", Caption = "Quarter" });
                pivotGridControl1.Fields.Add(new PivotGridField("Date", PivotArea.ColumnArea) { GroupInterval = PivotGroupInterval.DateMonthYear, Name = "fieldMonth", Caption = "Month" });
                pivotGridControl1.Fields.Add("Amount", DevExpress.XtraPivotGrid.PivotArea.DataArea);
                pivotGridControl1.Fields.Add("Quantity", DevExpress.XtraPivotGrid.PivotArea.DataArea);

                pivotGridControl1.EndUpdate();
            };
            Action filter_6x2 = () => {
                pivotGridControl1.BeginUpdate();
                pivotGridControl1.Fields["Category"].FilterValues.ValuesIncluded = pivotGridControl1.Fields["Category"].GetUniqueValues().Take(5).ToArray();
                var fieldYear = pivotGridControl1.Fields.GetFieldByName("fieldYear");
                fieldYear.FilterValues.ValuesExcluded = fieldYear.GetUniqueValues().Take(1).ToArray();
                var fieldMonth = pivotGridControl1.Fields.GetFieldByName("fieldMonth");
                fieldMonth.FilterValues.ValuesExcluded = fieldMonth.GetUniqueValues().Take(3).ToArray();
                pivotGridControl1.EndUpdate();
            };
            Action configure_6x2a = () => {
                pivotGridControl1.BeginUpdate();
                pivotGridControl1.Fields.Clear();

                pivotGridControl1.Fields.Add(new PivotGridField("Date", PivotArea.RowArea) { GroupInterval = PivotGroupInterval.DateYear, Name = "fieldYear", Caption = "Year" });
                pivotGridControl1.Fields.Add(new PivotGridField("Date", PivotArea.RowArea) { GroupInterval = PivotGroupInterval.DateQuarter, Name = "fieldQuarter", Caption = "Quarter" });
                pivotGridControl1.Fields.Add(new PivotGridField("Date", PivotArea.RowArea) { GroupInterval = PivotGroupInterval.DateMonth, Name = "fieldMonth", Caption = "Month" });
                pivotGridControl1.Fields.Add("Category", DevExpress.XtraPivotGrid.PivotArea.ColumnArea);
                pivotGridControl1.Fields.Add("Subcategory", DevExpress.XtraPivotGrid.PivotArea.ColumnArea);
                pivotGridControl1.Fields.Add("Product", DevExpress.XtraPivotGrid.PivotArea.ColumnArea);
                pivotGridControl1.Fields.Add("Amount", DevExpress.XtraPivotGrid.PivotArea.DataArea);
                pivotGridControl1.Fields.Add("Quantity", DevExpress.XtraPivotGrid.PivotArea.DataArea);

                pivotGridControl1.EndUpdate();
            };
            Action configure_2x1 = () => {
                pivotGridControl1.BeginUpdate();
                pivotGridControl1.Fields.Clear();
                pivotGridControl1.Fields.Add("Category", DevExpress.XtraPivotGrid.PivotArea.RowArea);
                pivotGridControl1.Fields.Add("Amount", DevExpress.XtraPivotGrid.PivotArea.DataArea);
                pivotGridControl1.Fields.Add(new PivotGridField("Date", PivotArea.ColumnArea) { GroupInterval = PivotGroupInterval.DateMonthYear, Name = "fieldYear", Caption = "Year" });
                pivotGridControl1.EndUpdate();
            };
            Action filter_2x1 = () => {
                pivotGridControl1.BeginUpdate();
                pivotGridControl1.Fields["Category"].FilterValues.ValuesIncluded = pivotGridControl1.Fields["Category"].GetUniqueValues().Take(5).ToArray();
                var fieldYear = pivotGridControl1.Fields.GetFieldByName("fieldYear");
                fieldYear.FilterValues.ValuesExcluded = fieldYear.GetUniqueValues().Take(1).ToArray();
                pivotGridControl1.EndUpdate();
            };

            testActions.Add(new TestAction() {
                Description = "Load1",
                ReassignDataSource = true,
                ClearFilter = false,
                Action = configure_6x2
            });
            testActions.Add(new TestAction() {
                Description = "Fitler1",
                ReassignDataSource = false,
                ClearFilter = false,
                Action = filter_6x2
            });
            testActions.Add(new TestAction() {
                Description = "Load2",
                ReassignDataSource = true,
                ClearFilter = true,
                Action = configure_6x2
            });
            testActions.Add(new TestAction() {
                Description = "Filter2",
                ReassignDataSource = false,
                ClearFilter = false,
                Action = filter_6x2
            });

        }


        private void timer1_Tick(object sender, EventArgs e) {



            timer1.Enabled = false;
            if (testIndex < testActions.Count) {
                PerformTest(testActions[testIndex]);
            }
            else {
                Restart();
            }
            testIndex++;
            timer1.Enabled = true;

        }
        
        private void PerformTest(TestAction testAction) {
            
            if (testAction.ReassignDataSource)
                pivotGridControl1.DataSource = null;
            if (testAction.ClearFilter) {
                foreach (PivotGridField field in pivotGridControl1.Fields) {
                    if (field.FilterValues.ValuesExcluded.Length > 0) {
                        field.FilterValues.ValuesExcluded = new object[0];
                    }
                }
            }
            sw.Restart();
            testAction.Action();
            if (testAction.ReassignDataSource)
                pivotGridControl1.DataSource = dataSource;
            pivotGridControl1.Update();
            //CheckCells();
            sw.Stop();
            pivotGridControl1.Invalidate();

            sb.Append($"{testAction.Description},{sw.ElapsedMilliseconds},");
            this.Text = sb.ToString();
        }

        private void CheckCells() {
            // touch cells
            int rowCount = pivotGridControl1.Cells.RowCount;
            int columnCount = pivotGridControl1.Cells.ColumnCount;
            for (int i = 0; i < rowCount; i++) {
                for (int j = 0; j < columnCount; j++) {
                    var value = pivotGridControl1.GetCellValue(j, i);
                }
            }
        }

        private void PrepareData() {

            dataSource = new ContosoRetailDWEntities().FactSales.OrderBy(o => o.ProductKey)
                //.Take(30000)
                .Select(s => new TestObject() {
                    Product = s.DimProduct.ProductName,
                    Subcategory = s.DimProduct.DimProductSubcategory.ProductSubcategoryName,
                    Category = s.DimProduct.DimProductSubcategory.DimProductCategory.ProductCategoryName,
                    Date = s.DateKey,
                    Amount = s.SalesAmount,
                    Quantity = s.SalesQuantity
                })
                //.SelectMany(to => new[] { to, to, to, to, to, to, to })
                //.SelectMany(to => new[] { to, to })
                .ToArray();
            
            StartTest();
            

        }

        private void StartTest() {
            GC.Collect();

            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName.Contains("XtraPivotGrid"));
            sb.Append($"{assembly.FullName.Split(",".ToCharArray())[1]},{dataSource.Length}rows,");
            
            timer1.Enabled = true;
        }

        private void WriteResults() {
            string result = sb.ToString();
            Debug.WriteLine(result);
            this.Text = result;
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@"..\..\Results.csv", true)) {
                file.WriteLine(sb.ToString());
            }
        }
        private void Restart() {
            WriteResults();
            Process.Start(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
            Application.Exit();
        }

    }

    public class TestAction {
        public bool ReassignDataSource { get; set; }
        public bool ClearFilter { get; set; }
        public string Description { get; set; }
        public Action Action { get; set; }
    }
}
