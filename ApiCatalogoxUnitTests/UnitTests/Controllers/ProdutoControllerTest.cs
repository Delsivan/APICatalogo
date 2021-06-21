using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ApiCatalogo.Tests.UnitTests.Controllers
{
    public class ProdutoControllerTest
    {
        private Mock<IMapper> _mapperMock;
        private Mock<IUnitOfWork> _repositoryMock;

        public ProdutoControllerTest()
        {
            _repositoryMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        [Trait("Produto", "Unit")]
        public async void GetProdutoById_Return_OkResult()
        {
            //Arrange  
            var controller = new ProdutosController(_repositoryMock.Object, _mapperMock.Object);
            var prodId = 10;

            var produtoEsperado = new Produto { ProdutoId = 10, Nome = "Teste" };
            var produtoEsperadoDTO = new ProdutoDTO { ProdutoId = 10, Nome = "Teste" };


            _repositoryMock.Setup(x => x.ProdutoRepository.GetById(p => p.ProdutoId == prodId)).ReturnsAsync(produtoEsperado);
            _mapperMock.Setup(x => x.Map<ProdutoDTO>(produtoEsperado))
            .Returns(produtoEsperadoDTO);

            //Act  
            var data = await controller.Get(prodId);


            //Assert  
            Assert.IsType<ProdutoDTO>(data.Value);
            Assert.Equal(prodId, data.Value.ProdutoId);
            Assert.Equal("Teste", data.Value.Nome);

        }

        //get por id -> notfoud
        [Fact]
        [Trait("Produto", "Unit")]

        public async void GetProdutoById_Return_NotFoundResult()
        {
            //Arrange  
            var controller = new ProdutosController(_repositoryMock.Object, _mapperMock.Object);

            _repositoryMock.Setup(x => x.ProdutoRepository.GetById(c => c.ProdutoId == It.IsAny<int>())).ReturnsAsync(() => null);


            //Act  
            var data = await controller.Get(It.IsAny<int>());

            //Assert  
            Assert.IsType<NotFoundResult>(data.Result);
            Assert.Null(data.Value);
        }
        //====================================Post=====================================

        [Fact]
        [Trait("Produto", "Unit")]
        public async void Post_Produto_AddValidData_Return_CreatedResult()
        {
            //Arrange  
            var controller = new ProdutosController(_repositoryMock.Object, _mapperMock.Object);

            var cat = new Produto() { Nome = "Teste2", Descricao = "Refrigerante de Coca 350 ml", Preco = 5, Estoque = 20};
            var catDTO = new ProdutoDTO() { Nome = "Teste2", Descricao = "Refrigerante de Coca 350 ml", Preco = 5}; 

            _repositoryMock.Setup(x => x.ProdutoRepository.Add(cat));
            _repositoryMock.Setup(x => x.Commit());

            _mapperMock.Setup(x => x.Map<Produto>(catDTO))
            .Returns(cat);

            //Act  
            var data = await controller.Post(catDTO);

            //Assert  
            Assert.IsType<CreatedAtRouteResult>(data);
            _repositoryMock.Verify(x => x.ProdutoRepository.Add(cat), Times.Once);
            _repositoryMock.Verify(x => x.Commit(), Times.Once);
        }
    }
}
