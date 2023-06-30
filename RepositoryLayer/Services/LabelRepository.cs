using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using CommonLayer;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Reflection.Emit;

namespace RepositoryLayer.Services
{
    public class LabelRepository : ILabelRepository
    {
        private readonly IConfiguration config;
        public readonly string connectionString;

        public LabelRepository(IConfiguration config)
        {
            this.connectionString = config.GetConnectionString("FundooDB");
            this.config = config;
        }

        public string AddLabel(string LabelName, int UserID, int NoteID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this.connectionString))
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_Add_Label", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@LabelName", LabelName);
                    cmd.Parameters.AddWithValue("@UserID", Convert.ToInt32(UserID));
                    cmd.Parameters.AddWithValue("@NoteID", Convert.ToInt32(NoteID));

                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return "Label Added Successfully";
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<LabelModel> GetLabelByUserID(int UserID)
        {

            List<LabelModel> labelmodellist = new List<LabelModel>();

            using (SqlConnection con = new SqlConnection(this.connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_ReadByUserID_Label", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", UserID);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        LabelModel labelModel = new LabelModel()
                        {
                            LabelName = reader["LabelName"].ToString(),
                            NoteID = Convert.ToInt32(reader["NoteID"])

                        };

                        labelmodellist.Add(labelModel);
                    }
                }
                con.Close();
            }
            return labelmodellist;
        }


        public IEnumerable<LabelModel> GetLabelByLabelName(string LabelName)
        {

            List<LabelModel> labelmodellist = new List<LabelModel>();

            using (SqlConnection con = new SqlConnection(this.connectionString))
            {
                SqlCommand cmd = new SqlCommand("dbo.usp_ReadByLabelName_Label", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LabelName", LabelName);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        LabelModel labelModel = new LabelModel()
                        {
                            UserID = Convert.ToInt32(reader["UserID"]),
                            NoteID = Convert.ToInt32(reader["NoteID"])

                        };

                        labelmodellist.Add(labelModel);
                    }
                }
                con.Close();
            }
            return labelmodellist;
        }


        public string UpdateLabel(string fromLabelName,string changeToLabelName, int UserID, int NoteID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this.connectionString))
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_Update_Label", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NoteID", NoteID);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@toLabelName", changeToLabelName);
                    cmd.Parameters.AddWithValue("@fromLabelName", fromLabelName);


                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    LabelModel label = new LabelModel();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            label.LabelName = reader.GetString(1);
                            label.UserID = reader.GetInt32(2);
                            label.NoteID  = reader.GetInt32(3); 

                        }
                    }
                    con.Close();
                }
                return "Label Updated Successfully";
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string DeleteLabel(string LabelName, int UserID, int NoteID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(this.connectionString))
                {
                    SqlCommand cmd = new SqlCommand("dbo.usp_Delete_Label", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@NoteID", NoteID);
                    cmd.Parameters.AddWithValue("@UserID", UserID);
                    cmd.Parameters.AddWithValue("@LabelName", LabelName);


                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return "Label Deleted Successfully";
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
