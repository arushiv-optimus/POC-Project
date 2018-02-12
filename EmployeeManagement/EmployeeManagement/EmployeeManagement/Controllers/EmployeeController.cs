using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using EmployeeManagement.Models;
using System.Web.Security;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        //
        // GET: /localhost://Employee/
      ///<summary>
      ///<Created By>Arushi.S
      ///<Created on>10-02-2018
        ///<Desp>This is used to process employee data

        #region variable declaration

        public string connction = ConfigurationManager.ConnectionStrings["Employee"].ConnectionString;
        public static int flag = 1;

        #endregion


        #region login 

        /// <summary>
        /// This Method is used for Login
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        
        // POST: /Employee/Login
        [HttpPost]
        public ActionResult Login(User user)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connction))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Userslogin", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@UserName", user.username);
                        cmd.Parameters.AddWithValue("@Password", user.password);
                        cmd.Parameters.Add("@status", SqlDbType.Int);
                        cmd.Parameters["@status"].Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        int i = Convert.ToInt32(cmd.Parameters["@status"].Value);
                        if (i == 1)
                        {
                            Session["User"] = Convert.ToString(user.username);
                            return Json("valid");
                        }
                        else
                        {

                            return Json("error");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return View();
            }
                
        }

        /// <summary>
        /// This Method is used for User LogOff
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Employee");
        }

        #endregion


        #region select employee


      /// <summary>
        /// This Method is used to select Employee list
      /// </summary>
      /// <returns></returns>
        [HttpGet]
        public ActionResult SelectEmployee()
        {
            List<EmpInfo> empInfo = new List<EmpInfo>();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connction))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("EMP_CURD", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Flag", 4);
                        //cmd.Parameters.AddWithValue("@Emp_id", emp.EMP_ID);
                        cmd.Parameters.Add("@Result", SqlDbType.NVarChar, 200);
                        cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dt);
                        DataSet ds = new DataSet();
                        ds.Tables.Add(dt);
                        ds.WriteXml(Server.MapPath("~/XML/" + "Employee.xml"));
                        foreach (DataRow row in dt.Rows)
                        {
                            empInfo.Add(new EmpInfo()
                            {
                                EMP_ID = Convert.ToInt32(row["Emp_Id"]),
                                Emp_Code = Convert.ToString(row["Emp_Code"]),
                                Emp_Name = Convert.ToString(row["Emp_Name"]),
                                Emp_Phone = Convert.ToString(row["Emp_Phone"]),
                                Emp_Gender = Convert.ToString(row["Emp_Gender"]),
                                Emp_Email = Convert.ToString(row["Emp_Email"]),
                                Emp_salary = Convert.ToDouble(row["Emp_salary"]),
                                Emp_Address = Convert.ToString(row["Emp_Address"]),
                                Emp_Department = Convert.ToString(row["Emp_Department"]),
                                Emp_Designation = Convert.ToString(row["Emp_Designation"]),
                                Emp_Age = Convert.ToInt32(row["Emp_Age"])

                            });
                        }
                    }
                }

                return View(empInfo);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        // POST: /Employee/SelectEmployee
        [HttpPost]
        public ActionResult SelectEmployee(EmpInfo emp)
        {
            List<EmpInfo> empInfo = new List<EmpInfo>();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(connction))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("EMP_CURD", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Flag", 4);
                        cmd.Parameters.AddWithValue("@Emp_id", emp.EMP_ID);
                        cmd.Parameters.Add("@Result", SqlDbType.NVarChar, 200);
                        cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
                        dataAdapter.Fill(dt);
                        DataSet ds = new DataSet();
                        ds.Tables.Add(dt);
                        ds.WriteXml(Server.MapPath("~/XML/" + "Employee.xml"));
                        foreach (DataRow row in dt.Rows)
                        {
                            empInfo.Add(new EmpInfo()
                            {
                                EMP_ID = Convert.ToInt32(row["Emp_Id"]),
                                Emp_Code = Convert.ToString(row["Emp_Code"]),
                                Emp_Name = Convert.ToString(row["Emp_Name"]),
                                Emp_Phone = Convert.ToString(row["Emp_Phone"]),
                                Emp_Gender = Convert.ToString(row["Emp_Gender"]),
                                Emp_Email = Convert.ToString(row["Emp_Email"]),
                                Emp_salary = Convert.ToDouble(row["Emp_salary"]),
                                Emp_Address = Convert.ToString(row["Emp_Address"]),
                                Emp_Department = Convert.ToString(row["Emp_Department"]),
                                Emp_Designation = Convert.ToString(row["Emp_Designation"]),
                                Emp_Age = Convert.ToInt32(row["Emp_Age"])

                            });
                        }
                    }
                }

                return Json(empInfo);
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        #endregion


        #region insert employee

        /// <summary>
        /// This Method is used to Insert Employee 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ManipulateEmployee()
        {
            return View();
        }

        // POST: /Employee/ManipulateEmployee
        [HttpPost]
        public ActionResult ManipulateEmployee(EmpInfo emp)
        {
            string msg = "";
            if (emp.EMP_ID == 0)
            {
                flag = 1;
            }
            using (SqlConnection con = new SqlConnection(connction))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("EMP_CURD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", flag);
                    cmd.Parameters.AddWithValue("@Emp_id", emp.EMP_ID);
                    cmd.Parameters.AddWithValue("@Emp_Code", emp.Emp_Code);
                    cmd.Parameters.AddWithValue("@Emp_Name", emp.Emp_Name);
                    cmd.Parameters.AddWithValue("@Emp_Email", emp.Emp_Email);
                    cmd.Parameters.AddWithValue("@Emp_Phone", emp.Emp_Phone);
                    cmd.Parameters.AddWithValue("@Emp_Address", emp.Emp_Address);
                    cmd.Parameters.AddWithValue("@Emp_Age", emp.Emp_Age);
                    cmd.Parameters.AddWithValue("@Emp_Gender", emp.Emp_Gender);
                    cmd.Parameters.AddWithValue("@Emp_salary", emp.Emp_salary);
                    cmd.Parameters.AddWithValue("@Emp_Department", emp.Emp_Department);
                    cmd.Parameters.AddWithValue("@Emp_Designation", emp.Emp_Designation);
                    cmd.Parameters.Add("@Result", SqlDbType.NVarChar, 200);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    cmd.ExecuteNonQuery();
                    msg = cmd.Parameters["@Result"].Value.ToString();
                }
            }

            return Json(msg);
        }
        #endregion


        #region update employee

        /// <summary>
        /// This Method is used to Update Employee 
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult EditEmployee(EmpInfo emp)
        {
            EmpInfo empInfo = new EmpInfo();
            using (SqlConnection con = new SqlConnection(connction))
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("EMP_CURD", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", 4);
                    cmd.Parameters.AddWithValue("@Emp_id", emp.EMP_ID);
                    cmd.Parameters.Add("@Result", SqlDbType.NVarChar, 200);
                    cmd.Parameters["@Result"].Direction = ParameterDirection.Output;
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        empInfo.EMP_ID = dr.GetInt32(0);
                        empInfo.Emp_Code = dr.GetString(1);
                        empInfo.Emp_Name = dr.GetString(2);
                        empInfo.Emp_Email = dr.GetString(3);
                        empInfo.Emp_Phone = dr.GetString(4);
                        empInfo.Emp_Address = dr.GetString(5);
                        empInfo.Emp_Department = dr.GetString(6);
                        empInfo.Emp_Designation = dr.GetString(7);
                        empInfo.Emp_Age = dr.GetInt32(8);
                        empInfo.Emp_salary = dr.GetDouble(9);
                        empInfo.Emp_Gender = dr.GetString(10);
                    }
                }
            }
            flag = 2;
            return View(empInfo);
        }

        #endregion


        #region Delete employee

        /// <summary>
        /// This Method is used to Delete Employee
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteEmployee(EmpInfo emp)
        {
            flag = 3;
            List<EmpInfo> empInfo = new List<EmpInfo>();
            ManipulateEmployee(emp);
            return RedirectToAction("SelectEmployee", "Employee");
        }


        #endregion

    }
}
