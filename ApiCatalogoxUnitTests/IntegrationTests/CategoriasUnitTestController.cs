using APICatalogo.Context;
using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Models.Mappings;
using APICatalogo.Repository;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace ApiCatalogoxUnitTests
{
    public class CategoriasUnitTestController
    {

        private IMapper mapper;
        private IUnitOfWork repository;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString =
           "Server=localhost;DataBase=CatalogoBD;Uid=your_user;Pwd=your_password";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
               .UseMySql(connectionString,ServerVersion.AutoDetect(connectionString))
               .Options;
        }

        public CategoriasUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            //DBUnitTestsMockInitializer db = new DBUnitTestsMockInitializer();
            //db.Seed(context);

            repository = new UnitOfWork(context);
        }

        //==========testes unitários===========================================================

        //====================================Get(int id) =====================================
        [Fact]
        [Trait("Categoria", "Integration")]
        public async void GetCategoriaById_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 13;

            //Act  
            var data = await controller.Get(catId);
            //data.Value.CategoriaId = 20;
            

            //Assert  
            Assert.IsType<CategoriaDTO>(data.Value);
            Assert.Equal(catId, data.Value.CategoriaId);
            //Assert.Equal("xpto", data.Value.Nome);
        }

        //get por id -> notfoud
        [Fact]
        [Trait("Categoria", "Integration")]

        public async void GetCategoriaById_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 9999;

            //Act  
            var data = await controller.Get(catId);

            //Assert  
            Assert.IsType<NotFoundResult>(data.Result);
        }
        //====================================Post=====================================

        [Fact]
        [Trait("Categoria", "Integration")]
        public async void Post_Categoria_AddValidData_Return_CreatedResult()
        {
            //Arrange   
            var controller = new CategoriasController(repository, mapper);

            var cat = new CategoriaDTO() { Nome = "Teste2", ImagemUrl = "http://www.delsivan.net/Imagens/2.jpg" };

            //Act  
            var data = await controller.Post(cat);
            
            //Assert  
            Assert.IsType<CreatedAtRouteResult>(data);
        }

        //===========================================Put =====================================

        [Fact]
        [Trait("Categoria", "Integration")]
        public async void Put_Categoria_Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(repository, mapper);
            var catId = 18; 

            //Act  
            var existingPost = await controller.Get(catId);
            var result = existingPost.Value.Should().BeAssignableTo<CategoriaDTO>().Subject;


            var catDto = new CategoriaDTO
            {
                CategoriaId = catId,
                Nome = "Categoria Atualizada",
                ImagemUrl = result.ImagemUrl
            };

            var updatedData = await controller.Put(catId, catDto);

            //Assert  
            Assert.IsType<OkObjectResult>(updatedData);
        }
    }
}
