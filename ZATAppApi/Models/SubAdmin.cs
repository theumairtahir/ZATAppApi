using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using ZATAppApi.Models.Exceptions;

namespace ZATAppApi.Models
{
    /// <summary>
    /// A user of the system who performs sub-admin tasks
    /// </summary>
    public class SubAdmin : User
    {
        /// <summary>
        /// constructor which adds a new sub-admin to the database
        /// </summary>
        /// <param name="name">Full Name of the sub-admin</param>
        /// <param name="contactNumber">Contact Number of the sub-admin</param>
        public SubAdmin(NameFormat name, ContactNumberFormat contactNumber) : base(name, contactNumber)
        {
            dbCommand = new SqlCommand("INSERT INTO [SUB_ADMINS] ([UId]) VALUES (" + id + ")", dbConnection);
            dbConnection.Open();
            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("SubAdmin->Constructor(NameFormat, ContactNumberFormat)", ex);
            }
            dbConnection.Close();
        }
        /// <summary>
        /// Constructor initializes the values from the database
        /// </summary>
        /// <param name="id">Primary key</param>
        public SubAdmin(long id) : base(id)
        {
            if (Role != ApplicationRoles.SubAdmin)
            {
                throw new PrimaryKeyNotForEntityException(Role.ToString(), "Sub-Admin");
            }
        }
        /// <summary>
        /// Method to add area for the Sub-Admin
        /// </summary>
        /// <param name="areaName">Name of the area to be alloted</param>
        /// <returns></returns>
        public Area AddArea(string areaName)
        {
            Area area;
            dbCommand = new SqlCommand("AddNewAreaSubAdmin", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = id;
            dbCommand.Parameters.Add(new SqlParameter("@name", System.Data.SqlDbType.VarChar)).Value = areaName;
            dbConnection.Open();
            try
            {
                area = new Area { AreaId = (int)dbCommand.ExecuteScalar(), AreaName = areaName };
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("SubAdmin->AddArea", ex);
            }
            dbConnection.Close();
            return area;
        }
        /// <summary>
        /// Method to remove an alloted area from the sub-admin
        /// </summary>
        /// <param name="area">Area to be removed</param>
        public void RemoveArea(Area area)
        {
            dbCommand = new SqlCommand("RemoveAreaSubAdmin", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@aId", System.Data.SqlDbType.Int)).Value = area.AreaId;
            dbConnection.Open();
            try
            {
                dbCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("SubAdmin->RemoveArea", ex);
            }
            dbConnection.Close();
        }
        /// <summary>
        /// Method to get all areas alloted to the Sub-Admin
        /// </summary>
        /// <returns></returns>
        public List<Area> GetAllAreas()
        {
            List<Area> lstAreas = new List<Area>();
            dbCommand = new SqlCommand("GetAllAreasSubAdmin", dbConnection);
            dbCommand.CommandType = System.Data.CommandType.StoredProcedure;
            dbCommand.Parameters.Add(new SqlParameter("@uId", System.Data.SqlDbType.BigInt)).Value = id;
            dbConnection.Open();
            try
            {
                using (dbReader=dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        lstAreas.Add(new Area
                        {
                            AreaId = (int)dbReader["AreaId"],
                            AreaName = (string)dbReader["AreaName"]
                        });
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("SubAdmin->GetAllAreas", ex);
            }
            dbConnection.Close();
            return lstAreas;
        }
        /// <summary>
        /// Method to get all sub-admins present in the database
        /// </summary>
        /// <returns></returns>
        public static List<SubAdmin> GetAllSubAdmins()
        {
            List<SubAdmin> lstSubAdmins = new List<SubAdmin>();
            SqlConnection dbConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);
            SqlCommand dbCommand = new SqlCommand("SELECT * FROM SUB_ADMINS ORDER BY [UId] DESC ", dbConnection);
            dbConnection.Open();
            try
            {
                using (SqlDataReader dbReader= dbCommand.ExecuteReader())
                {
                    while (dbReader.Read())
                    {
                        lstSubAdmins.Add(new SubAdmin((long)dbReader[0]));
                    }
                }
            }
            catch (SqlException ex)
            {
                dbConnection.Close();
                throw new DbQueryProcessingFailedException("SubAdmin->GetAllSubAdmins", ex);
            }
            dbConnection.Close();
            return lstSubAdmins;
        }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override List<Ride> GetCompletedRides()
        {
            return new List<Ride>();
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        /// <summary>
        /// Area for which the SubAdmin Works in
        /// </summary>
        public class Area
        {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
            public string AreaName { get; set; }
            public int AreaId { get; set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        }
    }
}