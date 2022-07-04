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

    public class TicketHistoryConcrete : ITicketHistory
    {
       //private readonly DatabaseContext _context;

        GoldmedalTicketEntities _context = new GoldmedalTicketEntities();
        //public TicketHistoryConcrete(DatabaseContext context)
        //{
        //    _context = context;
        //}

        public void TicketHistory(TicketHistory ticketHistory)
        {
            try
            {
                //using (var dbContextTransaction = _context.Database.BeginTransaction())
                //{

                    ticketHistory.SLNO = 0;
                    _context.TicketHistories.Add(ticketHistory);
                    _context.SaveChanges();
                //}
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<TicketHistoryResponse> ListofTicketHistorybyTicket(long? ticketId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DatabaseConnection"].ConnectionString))
                {
                    var param = new DynamicParameters();
                    param.Add("@TicketId", ticketId);
                    return con.Query<TicketHistoryResponse>("Usp_GetTicketHistorybyTicketId", param, null, false, 0, CommandType.StoredProcedure).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
