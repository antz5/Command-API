using System.Collections.Generic;
using CommandAPI.Models;

namespace CommandAPI.Data
{
    public class MockCommandRepository : ICommandRepository
    {
        public Command GetCommandById(int id)
        {
            return new Command
            {
                Id = 0,
                HowTo = "Make coffee",
                Line = "Boil milk",
                Platform= "Stove and milk boiler"
            };
        }

        public IEnumerable<Command> GetAllCommands()
        {
            var commands = new List<Command>()
            {
                new Command { Id = 1, HowTo="Chop onions",Line="knife",Platform="Chopping board and knife"},
                new Command {Id = 0, HowTo = "Make coffee", Line = "Boil milk", Platform= "Stove and milk boiler" },
                new Command {Id=2, HowTo= "Make tea", Line="boil water with a tea bag", Platform="kettle"}
            };

            return commands;
        }

        public bool SaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public void CreateCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }

        public void DeleteCommand(Command cmd)
        {
            throw new System.NotImplementedException();
        }
    }
}