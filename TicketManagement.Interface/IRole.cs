using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TicketManagement.Models;
using TicketManagement.ViewModels;
using TicketManagement.Reposistory;

namespace TicketManagement.Interface
{
    public interface IRole
    {
        IEnumerable<RoleMaster> GetAllRoleMaster();
        RoleMaster GetRoleMasterById(int? roleId);
        long? AddRoleMaster(RoleMaster roleMaster);
        long? UpdateRoleMaster(RoleMaster roleMaster);
        void DeleteRoleMaster(int? roleId);
        bool CheckRoleMasterNameExists(string roleName);
        IQueryable<RoleMaster> ShowAllRoleMaster(string sortColumn, string sortColumnDir, string search);
        List<SelectListItem> GetAllActiveRoles();
        long? UpdateRoleStatus(ViewMenuRoleStatusUpdateModel vmrolemodel);
        long? UpdateSubMenuRoleStatus(ViewSubMenuRoleStatusUpdateModel vmrolemodel);
        List<SelectListItem> GetAllActiveRolesNotAgent();
        List<SelectListItem> GetAllActiveRolesAgent();
    }
}
