﻿using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using MagicVilla_VillaAPI.Repository.iRepository;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaNumberRepository _dbVillaNumber;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;

        public VillaNumberAPIController(IVillaNumberRepository dbVillaNumber, IMapper mapper, IVillaRepository dbVilla)
        {
            _dbVillaNumber = dbVillaNumber;
            _mapper = mapper;
            this._response = new();
            _dbVilla = dbVilla;
        }
        [HttpGet]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                IEnumerable<VillaNumber> villaNumberList = await _dbVillaNumber.GetAllAsync(includeProperties : "Villa");
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(villaNumberList);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errormessages
                    =new List<string>() { ex.ToString()};
                return _response;
            }
        }
        [HttpGet("{id:int}",Name= "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id) 
        {
            try { 
            if (id == 0)
            {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                 
                }
            var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
            if (villaNumber == null)
            {
                return NotFound();
            }
            _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
            _response.StatusCode = HttpStatusCode.OK;
            return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errormessages
                    = new List<string>() { ex.ToString() };
                return _response;
            }

        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO createDTO) 
        {
            try { 
         
           if(await _dbVillaNumber.GetAsync(u=>u.VillaNo == createDTO.VillaNo)!=null)
            {
                ModelState.AddModelError("Errormessages", "Villa Number Already Exist");
                return BadRequest(ModelState);
            }
           if(await _dbVilla.GetAsync(u=>u.Id== createDTO.VillaID)== null)
                {
                    ModelState.AddModelError("Errormessages", "Villa ID is Invalid");
                    return BadRequest(ModelState);
                }
            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }

           VillaNumber villaNumber = _mapper.Map<VillaNumber>(createDTO); 
          
            await _dbVillaNumber.CreateAsync(villaNumber);
            _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
            _response.StatusCode = HttpStatusCode.Created;
            

            return CreatedAtRoute("GetVillaNumber", new { id = villaNumber.VillaNo }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errormessages
                    = new List<string>() { ex.ToString() };
                return _response;
            }
        }
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try { 
            if (id == 0)
            {
                return BadRequest();
            }
            var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id);
            if (villaNumber == null)
            {
                return NotFound();
            }
            
            await _dbVillaNumber.RemoveAsync(villaNumber);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errormessages
                    = new List<string>() { ex.ToString() };
                return _response;
            }
        }
        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillaNumber(int id, [FromBody] VillaNumberUpdateDTO updateDTO)
        {
            try { 
            if (updateDTO == null || id != updateDTO.VillaNo)
            {
                return BadRequest(updateDTO);

            }
                if (await _dbVilla.GetAsync(u => u.Id == updateDTO.VillaID) == null)
                {
                    ModelState.AddModelError("Errormessages", "Villa ID is Invalid");
                    return BadRequest(ModelState);
                }


                VillaNumber model = _mapper.Map<VillaNumber>(updateDTO);


            await _dbVillaNumber.UpdateAsync(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Errormessages
                    = new List<string>() { ex.ToString() };
                return _response;
            }
        }
        [HttpPatch("{id:int}",Name = "UpdatePartialVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePatchVillaNumber(int id, JsonPatchDocument<VillaNumberUpdateDTO> patchDTO)

        {
            
            if (patchDTO == null || id == 0)
            {
                return BadRequest();

            }
            var villaNumber = await _dbVillaNumber.GetAsync(u => u.VillaNo == id,tracked:false);


            VillaNumberUpdateDTO villaNumberDTO = _mapper.Map<VillaNumberUpdateDTO>(villaNumber);

            if (villaNumber == null)
            {
                return BadRequest();

            }
            VillaNumber model = _mapper.Map<VillaNumber>(villaNumberDTO);
            patchDTO.ApplyTo(villaNumberDTO, ModelState);

            await _dbVillaNumber.UpdateAsync(model);
           
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
