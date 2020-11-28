using System.Collections.Generic;
using CommandAPI.Models;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Data;
using AutoMapper;
using CommandAPI.Dtos;
using Microsoft.AspNetCore.JsonPatch;

namespace CommandAPI.Controllers
{
    [Route("api/commands")]
    [ApiController]
    public class CommandsControllers : ControllerBase
    {
        private readonly ICommandRepository _commandRepository;

        public IMapper _mapper { get; }

        public CommandsControllers(ICommandRepository commandRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommands()
        {
            var commands = _commandRepository.GetAllCommands();
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
        }

        [HttpGet("{id}",Name="GetCommandById")]
        public ActionResult<CommandReadDto> GetCommandById(int id)
        {
            var command =_mapper.Map<CommandReadDto>(_commandRepository.GetCommandById(id));
            if(command == null)
            {
                return NotFound();
            }
            return Ok(command);
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommand([FromBody] CommandCreateDto cmd)
        {
            var commandModel = _mapper.Map<Command>(cmd);
            _commandRepository.CreateCommand(commandModel);
            _commandRepository.SaveChanges();

            var commandReadDto = _mapper.Map<CommandReadDto>(commandModel);
            return CreatedAtRoute(nameof(GetCommandById), new {Id = commandReadDto.Id},commandReadDto);            
        }

        [HttpPut("{id}")]
        public ActionResult UpdateCommand(int id, [FromBody] CommandUpdateDto cmd)
        {
            var commandExists = _commandRepository.GetCommandById(id);
            if(commandExists == null)
            {
                return NotFound(cmd);
            }

            _mapper.Map(cmd,commandExists);
            _commandRepository.UpdateCommand(commandExists);
            _commandRepository.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id}")]
        public ActionResult PartialCommandUpdate(int id, JsonPatchDocument<CommandUpdateDto> patchCmd)
        {
            var commandModelFromRepo = _commandRepository.GetCommandById(id);
            if(commandModelFromRepo == null)
            {
                return NotFound();
            }
            var commandToPatch = _mapper.Map<CommandUpdateDto>(commandModelFromRepo);
            patchCmd.ApplyTo(commandToPatch, ModelState);

            if(!TryValidateModel(commandToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(commandToPatch,commandModelFromRepo);

            _commandRepository.UpdateCommand(commandModelFromRepo);
            _commandRepository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteCommand(int id)
        {
            var commandExists = _commandRepository.GetCommandById(id);

            if(commandExists == null)
            {
                return NotFound();
            }

            _commandRepository.DeleteCommand(commandExists);
            _commandRepository.SaveChanges();

            return NoContent();
        }
    }
}
