using MagicVilla_VillaAPI.Data;
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
using Microsoft.AspNetCore.Authorization;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IVillaRepository _dbVilla;
        private readonly IMapper _mapper;

        public VillaAPIController(IVillaRepository dbVilla, IMapper mapper)
        {
            _dbVilla = dbVilla;
            _mapper = mapper;
            this._response = new();

        }
       // private readonly ILogger<VillaAPIController> _logger;

       /* public VillaAPIController(ILogger<VillaAPIController> logger)
        {
            _logger = logger;
        }*/


        [HttpGet]
        [Authorize]

        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
                _response.Result = _mapper.Map<List<VillaDTO>>(villaList);
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

        [HttpGet("{id:int}",Name= "GetVilla")]
        [Authorize (Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id) 
        {
            try { 
            if (id == 0)
            {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                 
                }
            var villa = await _dbVilla.GetAsync(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            _response.Result = _mapper.Map<VillaDTO>(villa);
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
        public async Task<ActionResult<APIResponse>> CreateVilla([FromBody]VillaCreateDTO createDTO) 
        {
            try { 
           /* if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }*/
           if(await _dbVilla.GetAsync(u=>u.Name.ToLower()== createDTO.Name.ToLower())!=null)
            {
                ModelState.AddModelError("Errormessages", "Villa Already Exist");
                return BadRequest(ModelState);
            }
            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
           /* if (villaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }*/
           Villa villa = _mapper.Map<Villa>(createDTO); 
           /* Villa model = new ()
           {
                Amenity= createDTO.Amenity,
                Details= createDTO.Details,
                //Id= villaDTO.Id,
                ImageUrl= createDTO.ImageUrl,
                Name= createDTO.Name,
                Occupancy= createDTO.Occupancy,
                Rate= createDTO.Rate,
                Sqft= createDTO.Sqft
            };*/
            await _dbVilla.CreateAsync(villa);
            _response.Result = _mapper.Map<VillaDTO>(villa);
            _response.StatusCode = HttpStatusCode.Created;
            

            return CreatedAtRoute("GetVilla", new { id = villa.Id }, _response);
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
        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [Authorize(Roles = "custom")]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try { 
            if (id == 0)
            {
                return BadRequest();
            }
            var villa = await _dbVilla.GetAsync(u => u.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            
            await _dbVilla.RemoveAsync(villa);
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
        [HttpPut("{id:int}", Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> UpdateVillas(int id, [FromBody] VillaUpdateDTO updateDTO)
        {
            try { 
            if (updateDTO == null || id != updateDTO.Id)
            {
                return BadRequest(updateDTO);

            }
            // var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            // villa.Name = villaDTO.Name;
            //  villa.Occupancy = villaDTO.Occupancy;

            Villa model = _mapper.Map<Villa>(updateDTO);

            //Villa model = new()
            //{
            //    Amenity = updateDTO.Amenity,
            //    Details = updateDTO.Details,
            //    Id = updateDTO.Id,
            //    ImageUrl = updateDTO.ImageUrl,
            //    Name = updateDTO.Name,
            //    Occupancy = updateDTO.Occupancy,
            //    Rate = updateDTO.Rate,
            //    Sqft = updateDTO.Sqft
            //};
            await _dbVilla.UpdateAsync(model);
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
        [HttpPatch("{id:int}",Name ="UpdatePartialVilla" )]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePatchVilla(int id, JsonPatchDocument<VillaUpdateDTO> patchDTO)

        {
            
            if (patchDTO == null || id == 0)
            {
                return BadRequest();

            }
            var villa = await _dbVilla.GetAsync(u => u.Id == id,tracked:false);

            //  villa.Name = "new name";
            //  _db.SaveChanges();

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(villa);

            //VillaUpdateDTO villaDTO = new()
            //{
            //    Amenity = villa.Amenity,
            //    Details = villa.Details,
            //    Id = villa.Id,
            //    ImageUrl = villa.ImageUrl,
            //    Name = villa.Name,
            //    Occupancy = villa.Occupancy,
            //    Rate = villa.Rate,
            //    Sqft = villa.Sqft
            //};

            if (villa == null)
            {
                return BadRequest();

            }
            Villa model = _mapper.Map<Villa>(villaDTO);
            patchDTO.ApplyTo(villaDTO, ModelState);

            //Villa model = new Villa()
            //{
            //    Amenity = villaDTO.Amenity,
            //    Details = villaDTO.Details,
            //    Id = villaDTO.Id,
            //    ImageUrl = villaDTO.ImageUrl,
            //    Name = villaDTO.Name,
            //    Occupancy = villaDTO.Occupancy,
            //    Rate = villaDTO.Rate,
            //    Sqft = villaDTO.Sqft
            //};

            await _dbVilla.UpdateAsync(model);
           
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return NoContent();
        }
    }
}
