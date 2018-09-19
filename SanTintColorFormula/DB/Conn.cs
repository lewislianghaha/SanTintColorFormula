using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SanTintColorFormula.DB
{
    public class Conn
    {
        #region 获取产品系列信息

        private string _productList = @"
                                         SELECT a.ProductId,a.ProductName
                                         FROM dbo.Product a
                                         INNER JOIN dbo.BrandProduct b ON a.ProductId=b.ProductId
                                         WHERE b.BrandId={0}
                                       ";

        #endregion

        #region 获取品牌信息

        private string _GetBrand = @"
                                       SELECT a.BrandId,a.BrandName
                                       FROM dbo.Brand a
                                    ";

        #endregion

        //获取配置文件信息并进行数据库连接
        public SqlConnection GetConnection()
        {
            var pubs = ConfigurationManager.ConnectionStrings["Connstring"];//读取配置文件信息

            var strcon = pubs.ConnectionString;

            var conn=new SqlConnection(strcon);
            return conn;
        }

        //获取品牌列表
        public DataSet GetBrandList()
        {
            var sqlDataAdapter=new SqlDataAdapter();
            var ds=new DataSet();

            var sqlcon = GetConnection();

            try
            {
                sqlDataAdapter.SelectCommand=new SqlCommand(_GetBrand,sqlcon);
                sqlDataAdapter.Fill(ds);
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }

            return ds;
        }

        public DataSet GetProductList(int brandId)
        {
            var sqlDataAdapter=new SqlDataAdapter();
            var ds=new DataSet();

            var sqlcon = GetConnection();

            try
            {
                sqlDataAdapter.SelectCommand=new SqlCommand(string.Format(_productList,brandId),sqlcon);
                sqlDataAdapter.Fill(ds);
            }
            catch (Exception ex)
            {
                throw (new Exception(ex.Message));
            }

            return ds;
        }
    }
}
