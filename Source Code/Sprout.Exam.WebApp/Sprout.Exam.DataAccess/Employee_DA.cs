using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Common;
using Sprout.Exam.DataAccess.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sprout.Exam.DataAccess
{
    public class Employee_DA : IBaseDA<EmployeeDto, int>
    {
        public EmployeeDto Get(EmployeeDto e)
        {
            EmployeeDto employeeDto = null;
            using (SqlConnection conn = new SqlConnection(Util.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = Util.GetConnectionTimeOut(),
                    CommandText = "spEmployeeGet",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int);
                    cmd.Parameters.Add("@TIN", SqlDbType.VarChar);

                    cmd.Parameters["@ID"].Value = e.Id;
                    cmd.Parameters["@TIN"].Value = e.Tin;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employeeDto = new EmployeeDto();
                            employeeDto.Id = Convert.ToInt32(reader["Id"]);
                            employeeDto.FullName = reader["FullName"].ToString();
                            employeeDto.Tin = reader["TIN"].ToString();
                            employeeDto.Birthdate = reader["BirthDate"].ToString();
                            employeeDto.TypeId = Convert.ToInt32(reader["EmployeeTypeId"]);
                            employeeDto.isDeleted = Convert.ToBoolean(reader["isDeleted"]);
                        }

                    }
                }
                return employeeDto;
            }
        }

        public IEnumerable<EmployeeDto> GetList()
        {
            using (SqlConnection conn = new SqlConnection(Util.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = Util.GetConnectionTimeOut(),
                    CommandText = "spEmployeeGet",
                    CommandType = CommandType.StoredProcedure
                })
                {

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return new EmployeeDto()
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                FullName = reader["FullName"].ToString(),
                                Tin = reader["TIN"].ToString(),
                                Birthdate = reader["BirthDate"].ToString(),
                                TypeId = Convert.ToInt32(reader["EmployeeTypeId"]),
                                isDeleted = Convert.ToBoolean(reader["isDeleted"])
                            };
                        }
                    }
                }
            }
        }

        public bool Delete(EmployeeDto e)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(Util.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = Util.GetConnectionTimeOut(),
                    CommandText = "spEmployeeUpdate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int);
                    cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);

                    cmd.Parameters["@ID"].Value = e.Id;
                    cmd.Parameters["@IsDeleted"].Value = e.isDeleted;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    result = true;
                }
            }
            return result;
        }

        public EmployeeDto Insert(EmployeeDto e)
        {
            using (SqlConnection conn = new SqlConnection(Util.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = Util.GetConnectionTimeOut(),
                    CommandText = "spEmployeeInsert",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@FullName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@BirthDate", SqlDbType.Date);
                    cmd.Parameters.Add("@TIN", SqlDbType.VarChar);
                    cmd.Parameters.Add("@EmployeeTypeId", SqlDbType.Int);

                    cmd.Parameters["@FullName"].Value = e.FullName;
                    cmd.Parameters["@BirthDate"].Value = e.Birthdate;
                    cmd.Parameters["@TIN"].Value = e.Tin;
                    cmd.Parameters["@EmployeeTypeId"].Value = e.TypeId;

                    conn.Open();

                    e.Id = Convert.ToInt32(cmd.ExecuteScalar());
                    if (e.Id > 0)
                    {
                        e.isSuccess = true;
                    }
                }
            }

            return e;
        }

        public bool Update(EmployeeDto e)
        {
            bool result = false;
            using (SqlConnection conn = new SqlConnection(Util.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand()
                {
                    Connection = conn,
                    CommandTimeout = Util.GetConnectionTimeOut(),
                    CommandText = "spEmployeeUpdate",
                    CommandType = CommandType.StoredProcedure
                })
                {
                    cmd.Parameters.Add("@ID", SqlDbType.Int);
                    cmd.Parameters.Add("@FullName", SqlDbType.VarChar);
                    cmd.Parameters.Add("@BirthDate", SqlDbType.Date);
                    cmd.Parameters.Add("@TIN", SqlDbType.VarChar);
                    cmd.Parameters.Add("@EmployeeTypeId", SqlDbType.Int);
                    cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit);

                    cmd.Parameters["@ID"].Value = e.Id;
                    cmd.Parameters["@FullName"].Value = e.FullName;
                    cmd.Parameters["@BirthDate"].Value = e.Birthdate;
                    cmd.Parameters["@TIN"].Value = e.Tin;
                    cmd.Parameters["@EmployeeTypeId"].Value = e.TypeId;
                    cmd.Parameters["@IsDeleted"].Value = e.isDeleted;
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    result = true;
                }
            }

            return result;
        }
    }
}
