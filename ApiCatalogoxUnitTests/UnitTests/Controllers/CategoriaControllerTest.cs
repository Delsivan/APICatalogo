using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ApiCatalogo.Tests.UnitTests.Controllers
{
    public class CategoriaControllerTest
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IUnitOfWork> _repositoryMock;

        public CategoriaControllerTest()
        {
            _repositoryMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        [Trait("Categoria", "Unit")]
        public async void GetCategoriaById_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(_repositoryMock.Object, _mapperMock.Object);
            var catId = 10;

            var categoriaEsperada = new Categoria {CategoriaId = 10, Nome="Teste"};
            var categoriaEsperadaDTO = new CategoriaDTO { CategoriaId = 10, Nome = "Teste" };


            _repositoryMock.Setup(x => x.CategoriaRepository.GetById(c => c.CategoriaId == catId)).ReturnsAsync(categoriaEsperada);
            _mapperMock.Setup(x => x.Map<CategoriaDTO>(categoriaEsperada))
            .Returns(categoriaEsperadaDTO);

            //Act  
            var data = await controller.Get(catId);
            

            //Assert  
            Assert.IsType<CategoriaDTO>(data.Value);
            Assert.Equal(catId, data.Value.CategoriaId);
            Assert.Equal("Teste", data.Value.Nome);

        }

        //get por id -> notfoud
        [Fact]
        [Trait("Categoria", "Unit")]

        public async void GetCategoriaById_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new CategoriasController(_repositoryMock.Object, _mapperMock.Object);

            _repositoryMock.Setup(x => x.CategoriaRepository.GetById(c => c.CategoriaId == It.IsAny<int>())).ReturnsAsync(() => null);
            

            //Act  
            var data = await controller.Get(It.IsAny<int>());

            //Assert  
            Assert.IsType<NotFoundResult>(data.Result);
            Assert.Null(data.Value);
        }
        //====================================Post=====================================

        [Fact]
        [Trait("Categoria", "Unit")]
        public async void Post_Categoria_AddValidData_Return_CreatedResult()
        {
            //Arrange  
            var controller = new CategoriasController(_repositoryMock.Object, _mapperMock.Object);

            var cat = new Categoria() { Nome = "Teste2", ImagemUrl = "http://www.delsivan.net/Imagens/2.jpg" };
            var catDTO = new CategoriaDTO() { Nome = "Teste2", ImagemUrl = "http://www.delsivan.net/Imagens/2.jpg" };

            _repositoryMock.Setup(x => x.CategoriaRepository.Add(cat));
            _repositoryMock.Setup(x => x.Commit());

            _mapperMock.Setup(x => x.Map<Categoria>(catDTO))
            .Returns(cat);

            //Act  
            var data = await controller.Post(catDTO);

            //Assert  
            Assert.IsType<CreatedAtRouteResult>(data);
            _repositoryMock.Verify(x => x.CategoriaRepository.Add(cat), Times.Once);
            _repositoryMock.Verify(x => x.Commit(), Times.Once);
        }

        //===========================================Put =====================================

        [Fact]
        [Trait("Categoria", "Unit")]
        public async void Put_Categoria_Update_ValidData_Return_OkResult()
        {
            //Arrange  
            var controller = new CategoriasController(_repositoryMock.Object, _mapperMock.Object);
            var catId = 18;

            var cat = new Categoria() { CategoriaId = catId, Nome = "Teste3", ImagemUrl = "http://www.delsivan.net/Imagens/2.jpg" };
            var catDTO = new CategoriaDTO() { CategoriaId = catId, Nome = "Teste3", ImagemUrl = "http://www.delsivan.net/Imagens/3.jpg" };

            _repositoryMock.Setup(x => x.CategoriaRepository.Update(cat));
            _repositoryMock.Setup(x => x.Commit());

            _mapperMock.Setup(x => x.Map<Categoria>(catDTO))
            .Returns(cat);

            //Act  
            var data = await controller.Put(catId, catDTO);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        [Trait("Categoria", "Unit")]

        public async void Put_Categoria_Update_BadRequest()
        {
            //Arrange  
            var controller = new CategoriasController(_repositoryMock.Object, _mapperMock.Object);

            var catId = 18;

            var cat = new Categoria() { CategoriaId = 20, Nome = "Teste3", ImagemUrl = "http://www.delsivan.net/Imagens/2.jpg" };
            var catDTO = new CategoriaDTO() { CategoriaId = 20, Nome = "Teste3", ImagemUrl = "http://www.delsivan.net/Imagens/3.jpg" };

            _repositoryMock.Setup(x => x.CategoriaRepository.Update(cat));
            _repositoryMock.Setup(x => x.Commit());


            //Act  
            var data = await controller.Put(catId, catDTO);


            //Assert  
            var returnObject = Assert.IsType<BadRequestObjectResult>(data);
            Assert.Equal(400, returnObject.StatusCode);
            Assert.Equal($"Não foi possível atualizar a categoria com id={catId}", returnObject.Value);
        }


        //=======================================Delete ===================================
        [Fact]
        [Trait("Categoria", "Unit")]
        public async void Delete_Categoria_Return_OkResult()
        {

            var categoriaEsperada = new Categoria { CategoriaId = 10, Nome = "Cat", ImagemUrl = "" };
            var catId = 9;

            //Arrange  
            var controller = new CategoriasController(_repositoryMock.Object, _mapperMock.Object);

            _repositoryMock.Setup(x => x.CategoriaRepository.GetById(c => c.CategoriaId == catId)).ReturnsAsync(categoriaEsperada);

            //Act  
            var data = await controller.Delete(catId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
            _repositoryMock.Verify(x => x.CategoriaRepository.Delete(categoriaEsperada), Times.Once);
        }

        [Fact]
        [Trait("Categoria", "Unit")]
        public async void Delete_Categoria_Return_NotFound()
        {
            var catId = 9;

            //Arrange  
            var controller = new CategoriasController(_repositoryMock.Object, _mapperMock.Object);

            _repositoryMock.Setup(x => x.CategoriaRepository.GetById(c => c.CategoriaId == catId)).ReturnsAsync(() => null);

            //Act  
            var data = await controller.Delete(catId);

            //Assert  
            var returnObject = Assert.IsType<NotFoundObjectResult>(data);
            Assert.Equal(404, returnObject.StatusCode);
            Assert.Equal($"A categoria com id={catId} não foi encontrada", returnObject.Value);

            _repositoryMock.Verify(x => x.CategoriaRepository.Delete(It.IsAny<Categoria>()), Times.Never);
        }
    }
}
