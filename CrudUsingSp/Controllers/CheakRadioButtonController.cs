using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CrudUsingSp.Controllers
{
    public class CheakRadioButtonController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            List<SelectListItem> items = PopulateFruits();
            return View(items);
        }

        [HttpPost]
        public ActionResult Index(string fruit)
        {
            List<SelectListItem> items = PopulateFruits();
            var selectedItem = items.Find(p => p.Value == fruit);
            if (selectedItem != null)
            {
                selectedItem.Selected = true;
                ViewBag.Message = "Selected Fruit: " + selectedItem.Text;
            }

            return View(items);
        }

        private static List<SelectListItem> PopulateFruits()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            string constr = ConfigurationManager.ConnectionStrings["ecs"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = " SELECT FruiName, FruitId FROM Fruit";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            items.Add(new SelectListItem
                            {
                                Text = sdr["FruiName"].ToString(),
                                Value = sdr["FruitId"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }

            return items;
        }
    }
}