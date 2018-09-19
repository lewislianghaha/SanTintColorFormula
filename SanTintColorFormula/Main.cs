using System;
using System.Data;
using System.Windows.Forms;
using SanTintColorFormula.DB;

namespace SanTintColorFormula
{
    public partial class Main : Form
    {
        public int LastBrandId = 0; //获取最后一次选中的品牌ID
        Conn conn=new Conn();

        public Main()
        {
            InitializeComponent();
            OnRegisterEvents();
        }

        public void OnRegisterEvents()
        {
            btnPrintout.Click += BtnPrintout_Click;
            btnSearch.Click += BtnSearch_Click;
            cmbBrand.Click += CmbBrand_Click;
            cmbProductList.Click += CmbProductList_Click;
        }

        /// <summary>
        /// //获取品牌记录作为列表记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbBrand_Click(object sender, EventArgs e)
        {
            try
            {
                //判断下拉列表是否有值(若有值就将产品系列表内的值清空)
                if (cmbBrand.Items.Count != 0)
                {
                    cmbProductList.Text = "";
                }
                //(若没有才与数据库连接获取数据)
                else
                {
                    var ds = conn.GetBrandList();
                    cmbBrand.DataSource = ds.Tables[0];
                    cmbBrand.DisplayMember = "BrandName";  //设置显示在列表的值(与数据表所定义的字段名一致)
                    cmbBrand.ValueMember = "BrandId";     //设置显示列表的值对应的内码ID
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// //根据品牌获取对应的产品系列信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CmbProductList_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbBrand.Items.Count == 0)
                {
                    MessageBox.Show("请选择品牌后继续", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //获取所选择的品牌信息(默认是获取对应的内码ID)
                    var dv = (DataRowView)cmbBrand.Items[cmbBrand.SelectedIndex];
                    var brandId = Convert.ToInt32(dv["BrandId"]);

                    //若为第一次获取产品系列列表就直接读取记录
                    if (cmbProductList.Items.Count==0)
                    {
                        GetProductToList(brandId);
                    }
                    //若产品系列列表内已有值就先判断再读取(作用:避免重复与数据库交互)
                    else if (LastBrandId!=brandId)
                    {
                        GetProductToList(brandId);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// //获取产品系列信息
        /// </summary>
        /// <param name="brandId"></param>
        private void GetProductToList(int brandId)
        {
            var ds = conn.GetProductList(brandId);
            cmbProductList.DataSource = ds.Tables[0];
            cmbProductList.DisplayMember = "ProductName";
            cmbProductList.ValueMember = "ProductId";
            LastBrandId = brandId;
        }

        /// <summary>
        /// //查询功能
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbBrand.Items.Count == 0 || cmbProductList.Items.Count == 0) throw new Exception("请选择品牌以及产品系列表再进行查询");
                //获取所选择的品牌列表信息
                var branddv = (DataRowView)cmbBrand.Items[cmbBrand.SelectedIndex];
                var brandId = Convert.ToInt32(branddv["BrandId"]);

                //获取所选的产品系列信息
                var productdv = (DataRowView)cmbProductList.Items[cmbProductList.SelectedIndex];
                var productId = Convert.ToInt32(productdv["ProductId"]);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// //导出记录至EXCEL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnPrintout_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
