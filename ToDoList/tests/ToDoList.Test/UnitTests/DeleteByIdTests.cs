namespace ToDoList.Test.UnitTests; //upravil jsem namespace aby vsechny testy byly v jednom

using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Domain.DTOs;
using ToDoList.WebApi.Controllers;
using ToDoList.Persistence.Repositories;
using ToDoList.Domain.Models;
using Microsoft.AspNetCore.Http;

public class DeleteByIdUnitTests
{
    [Fact]
    public void Delete_ValidItemId_ReturnsNoContent()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);

        var toDoItem = new ToDoItem
        {
            Name = "testItem",
            Description = "testDescription",
            IsCompleted = false,
            ToDoItemId = 1
        };

        repositoryMock.ReadById(Arg.Any<int>()).Returns(toDoItem); //mohli bychom specifikovat ze toDoItem navrati pouze pro ReadById(toDoItem.ToDoItemId)

        // Act
        var result = controller.DeleteById(toDoItem.ToDoItemId);

        // Assert
        Assert.IsType<NoContentResult>(result);
        //chtelo by to kontrolu kolikrat se zavolalo DeleteById
    }

    [Fact]
    public void Delete_DeleteByIdInvalidItemId_ReturnsNotFound()
    {
        // Arrange
        var repositoryMock = Substitute.For<IRepository<ToDoItem>>();
        var controller = new ToDoItemsController(repositoryMock);
        repositoryMock.ReadById(Arg.Any<int>()).Returns((ToDoItem)null);

        // Act
        var result = controller.DeleteById(1);
        var resultResult = result as NotFoundResult;

        // Assert
        Assert.IsType<NotFoundResult>(resultResult);
        repositoryMock.Received(1).ReadById(1);
        repositoryMock.DidNotReceive().Delete(Arg.Any<int>()); //krasa :)
    }
}
