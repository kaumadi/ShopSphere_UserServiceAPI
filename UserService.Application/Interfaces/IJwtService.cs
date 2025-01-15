using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserService.Application.DTO;

namespace UserService.Application.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateJwtToken(UserDTO userDTO);
    }
}
