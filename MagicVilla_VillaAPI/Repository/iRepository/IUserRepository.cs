using MagicVilla_API.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI.Repository.iRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);

        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        Task<UserDTO> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}
