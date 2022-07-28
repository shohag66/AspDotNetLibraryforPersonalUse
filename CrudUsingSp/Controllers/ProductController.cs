using CrudUsingSp.Config;
using CrudUsingSp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrudUsingSp.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return View(new Product { Id = 0 });
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Products_SaveOrUpdate", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id",product.Id);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Supplier", product.Supplier);
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("GetAll");
        }

        public ActionResult Search(string search)
        {
            List<Product> products = GetProducts("Products_SearchProduct", search);
            return View("GetAll", products);
        }

        public List<Product> GetProducts(string storeprocedure,string search)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand(storeprocedure, con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (search != null)
                        cmd.Parameters.AddWithValue("@Filter", search);
                    if (con.State == System.Data.ConnectionState.Open) ;
                        con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dtProducts = new DataTable();
                    dtProducts.Load(sdr);

                    foreach (DataRow row in dtProducts.Rows)
                    {
                        products.Add
                            (
                                  new Product
                                  {
                                      Id = Convert.ToInt32(row["Id"]),
                                      Name = row["Name"].ToString(),
                                      Price = Convert.ToDecimal(row["Price"]),
                                      Supplier = row["Supplier"].ToString()
                                  }


                            );
                    }
                }
            }

            return products;
           
        }

        public ActionResult GetAll()
        {
            List<Product> products = GetProducts("Products_GetAllProduct", null);
            return View(products);

            //using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            //{
            //    using (SqlCommand cmd = new SqlCommand("Products_GetAllProduct", con))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        if (con.State != System.Data.ConnectionState.Open)
            //            con.Open();
            //        SqlDataReader sdr = cmd.ExecuteReader();
            //        DataTable dtProducts = new DataTable();
            //        dtProducts.Load(sdr);

            //        foreach (DataRow row in dtProducts.Rows)
            //        {
            //            products.Add(
            //                      new Product
            //                      {
            //                          Id = Convert.ToInt32(row["Id"]),
            //                          Name = row["Name"].ToString(),
            //                          Price = Convert.ToDecimal(row["Price"]),
            //                          Supplier = row["Supplier"].ToString()
            //                      }


            //                );
            //        }
            //    }
            //}

        }

        public ActionResult Delete(int id)
        {
            if (id < 0)
            {
                return HttpNotFound();
            }
            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {
                using (SqlCommand cmd = new SqlCommand("Product_DeleteById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("GetAll");
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return HttpNotFound();
            }

            var _product = new Product();

            using (SqlConnection con = new SqlConnection(StoreConnection.GetConnection()))
            {

                using (SqlCommand cmd = new SqlCommand("Product_GetById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);
                    if (con.State != System.Data.ConnectionState.Open)
                        con.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    DataTable dt = new DataTable();
                    if (sdr.HasRows)
                    {
                        dt.Load(sdr);

                        DataRow row = dt.Rows[0];

                        _product.Id = Convert.ToInt32(row["Id"]);
                        _product.Name = row["Name"].ToString();
                        _product.Price = Convert.ToDecimal(row["Price"]);
                        _product.Supplier = row["Supplier"].ToString();

                        return View("Create", _product);
                    }
                    else
                    {
                        return HttpNotFound();
                    }

                }
            }

        }
    }
}