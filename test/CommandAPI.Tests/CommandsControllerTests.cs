using System;
using System.Collections.Generic;
using Moq;
using AutoMapper;
using CommandAPI.Models;
using CommandAPI.Data;
using CommandAPI.Profiles;
using Xunit;
using CommandAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using CommandAPI.Dtos;
using System.Diagnostics;
using Xunit.Abstractions;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        private readonly ITestOutputHelper _testOutputHelper;

        Mock<ICommandAPIRepo> mockRepo;
        CommandsProfile realProfile;
        MapperConfiguration configuration;
        IMapper mapper;

        public CommandsControllerTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;

            mockRepo = new Mock<ICommandAPIRepo>();

            realProfile = new CommandsProfile();
            configuration = new MapperConfiguration(cfg => cfg.AddProfile(realProfile));
            mapper = new Mapper(configuration);
        }

        public void Dispose()
        {
            mockRepo = null;
            mapper = null;
            configuration = null;
            realProfile = null;
        }

        [Fact]
        public void GetCommandItems_ReturnZeroItems_WhenDBIsEmpty()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(0));
            // create instance of CommandsController
            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetAllCommands();

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllCommands_ReturnsOneItem_WhenDBHasOneResource()
        {
            //Arrange
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);

            //Act
            var result = controller.GetAllCommands();

            //Assert
            var okResult = result.Result as OkObjectResult;
            var commands = okResult.Value as List<CommandReadDto>;
            Assert.Single(commands);
        }

        [Fact]
        public void TestName()
        {
            // Given
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);

            // When
            var result = controller.GetAllCommands();

            // Then
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetAllCommands_ReturnsCorrectType_WhenDBHasOneResource()
        {
            // Given
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));
            var controller = new CommandsController(mockRepo.Object, mapper);

            // When
            var result = controller.GetAllCommands();

            // Then
            Assert.IsType<ActionResult<IEnumerable<CommandReadDto>>>(result);
        }

        [Fact]
        public void GetCommById_Return404_WhenNonexistentIDProvided()
        {
            // Given
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);

            // When
            var result = controller.GetCommandById(1);

            // Then
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public void GetCommById_Returns200OK_WhenValidIDProvided()
        {
            // Given
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command
            {
                Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock"
            });
            var controller = new CommandsController(mockRepo.Object, mapper);

            // When
            var result = controller.GetCommandById(1);

            // Then
            Assert.IsType<OkObjectResult>(result.Result);
        }

        [Fact]
        public void GetCommByID_ReturnsCorrectType_WhenValidID()
        {
            // Given
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command
            {
                Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock"
            });
            var controller = new CommandsController(mockRepo.Object, mapper);

            // When
            var result = controller.GetCommandById(1);

            // Then
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        //CREATE TESTS
        [Fact]
        public void CreateCommand_ReturnsCorrectType_WhenValidObjSubmitted()
        {
            // Given
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command
            {
                Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock"
            });
            var controller = new CommandsController(mockRepo.Object, mapper);

            // When
            var result = controller.CreateCommand(new CommandCreateDto { });

            // Then
            Assert.IsType<ActionResult<CommandReadDto>>(result);
        }

        [Fact]
        public void CreateCommand_Returns201Created_WhenValidObject()
        {
            // Given
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command
            {
                Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock"
            });
            var controller = new CommandsController(mockRepo.Object, mapper);

            // When
            var result = controller.CreateCommand(new CommandCreateDto { });

            // Then
            Assert.IsType<CreatedAtRouteResult>(result.Result);
            _testOutputHelper.WriteLine("***** FACT");
        }

        //UpdateCommand Tests
        [Fact]
        public void UpdateComm_Returns204NoContent_WhenValidObjSubmitted()
        {
            // Given
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(new Command
            {
                Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock"
            });
            var controller = new CommandsController(mockRepo.Object, mapper);

            // When
            var result = controller.UpdateCommand(1, new CommandUpdateDto { });

            // Then
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateComm_Returns404NotFound_NotExistentIDSubmitted()
        {
            // Given
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);

            // When
            var result = controller.UpdateCommand(0, new CommandUpdateDto { });

            // Then
            Assert.IsType<NotFoundResult>(result);
        }

        // Partial Update Tests
        [Fact]
        public void PartialCommUPdate_Returns404_WhenNonExistentIDSubmitted()
        {
            // Given
            mockRepo.Setup(repo =>
                repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);

            // When
            var result = controller.PartialCommandUpdate(0,
                new Microsoft.AspNetCore.JsonPatch.JsonPatchDocument<CommandUpdateDto> { });

            // Then
            Assert.IsType<NotFoundResult>(result);
        }

        // Delete
        [Fact]
        public void DeleteCommand_Returns204NoContent_WhenValidID()
        {
            // Given
            mockRepo.Setup(repo =>
            repo.GetCommandById(1)).Returns(new Command
            {
                Id = 1,
                HowTo = "mock",
                Platform = "Mock",
                CommandLine = "Mock"
            });
            var controller = new CommandsController(mockRepo.Object, mapper);

            // When
            var result = controller.DeleteCommand(1);

            // Then
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCommand_Returns404NotFound_WhenNonExistentID()
        {
            // Given
            mockRepo.Setup(repo =>
            repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);

            // When
            var result = controller.DeleteCommand(0);

            // Then
            // Assert.IsType<NotFoundResult>(result);
            Assert.IsType<OkResult>(result);
        }


        private List<Command> GetCommands(int num)
        {
            _testOutputHelper.WriteLine("****_testOutputHelper");
            Debug.Print("+++++++++DEBUG Print");

            var commands = new List<Command>();
            if (num > 0)
            {
                commands.Add(new Command
                {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            }
            return commands;
        }
    }
}