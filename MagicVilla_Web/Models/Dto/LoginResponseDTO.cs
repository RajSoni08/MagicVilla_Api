﻿using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;

namespace MagicVilla_VillaAPI.Models.Dto
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string Token { get; set; }

    }
}
