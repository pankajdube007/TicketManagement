using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TicketManagement.Interface;
//using TicketManagement.Models;
using TicketManagement.ViewModels;
using TicketManagement.Reposistory;

namespace TicketManagement.Concrete
{
    public class SavedAssignedRolesConcrete : ISavedAssignedRoles
    {
        GoldmedalTicketEntities _context = new GoldmedalTicketEntities();
       // private readonly DatabaseContext _context;
        //public SavedAssignedRolesConcrete(DatabaseContext context)
        //{
        //    _context = context;
        //}

        public long? AddAssignedRoles(SavedAssignedRole savedAssignedRoles)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    con.Open();
                    SqlTransaction transaction = con.BeginTransaction();
                    var param = new DynamicParameters();
                    param.Add("@RoleId", savedAssignedRoles.RoleId);
                    param.Add("@Status", savedAssignedRoles.IsActive);
                    param.Add("@UserId", savedAssignedRoles.UserId);
                    var result = con.Execute("Usp_InsertSavedAssignedRoles", param, transaction, 0, CommandType.StoredProcedure);
                    if (result > 0)
                    {
                        transaction.Commit();
                        return result;
                    }
                    else
                    {
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool CheckAssignedRoles(long? userId)
        {
            try
            {
                var checkIsRoleAlreadyAssigned = (from sar in _context.SavedAssignedRoles
                                                  where sar.UserId == userId
                                                  select sar).Any();

                return checkIsRoleAlreadyAssigned;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public SavedAssignedRolesViewModel GetAssignedRolesbyUserId(long? userId)
        {
            try
            {
            
                    var checkIsRoleAlreadyAssigned = (from sar in _context.SavedAssignedRoles
                                                      join roles in _context.RoleMasters on sar.RoleId equals roles.SLNO
                                                      where sar.UserId == userId
                                                      select new SavedAssignedRolesViewModel
                                                      {
                                                          Status = sar.IsActive,
                                                          RoleId = sar.RoleId,
                                                          RoleName = roles.RoleName,
                                                          UserId = sar.UserId
                                                      }).FirstOrDefault();

                    return checkIsRoleAlreadyAssigned;
               
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
