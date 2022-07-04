//using TicketManagement.Models;
using TicketManagement.ViewModels;
using TicketManagement.Reposistory;

namespace TicketManagement.Interface
{
    public interface ISavedAssignedRoles
    {
        long? AddAssignedRoles(SavedAssignedRole savedAssignedRoles);
        bool CheckAssignedRoles(long? userId);
        SavedAssignedRolesViewModel GetAssignedRolesbyUserId(long? userId);
    }
}