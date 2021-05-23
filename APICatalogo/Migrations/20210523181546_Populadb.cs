using Microsoft.EntityFrameworkCore.Migrations;

namespace APICatalogo.Migrations
{
    public partial class Populadb : Migration
    {
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("Insert into Categorias(Nome,ImagemUrl) values('Bebidas'," + "'https://images.unsplash.com/photo-1563556812239-013bf57d6aeb?ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&ixlib=rb-1.2.1&auto=format&fit=crop&w=634&q=80')");
            mb.Sql("Insert into Categorias(Nome,ImagemUrl) values('Lanches'," + "'https://images.unsplash.com/photo-1586816001966-79b736744398?ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&ixlib=rb-1.2.1&auto=format&fit=crop&w=1350&q=80')");

            mb.Sql("Insert into Categorias(Nome,ImagemUrl) values('Sobremesas'," + "'https://images.unsplash.com/photo-1546384879-9b267876f0bd')");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) values('Coca-Cola Diet','Refrigerante Coca-Cola 350 ml',5.45,'https://images.unsplash.com/photo-1613390188333-7f927aefc7c9',50,now(),(Select CategoriaId from Categorias where Nome='Bebidas'))");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque,DataCadastro,CategoriaId) values('Lanche de Atum','Lanche de Atum com maionese',8.50,'https://media.istockphoto.com/photos/sashimi-de-atum-salmo-e-peixe-branco-picture-id477562426?s=612x612',50,now(),(Select CategoriaId from Categorias where Nome='Lanches'))");

            mb.Sql("Insert into Produtos(Nome,Descricao,Preco,ImagemUrl,Estoque, DataCadastro,CategoriaId) values('Pudim 100 g','Pudim de leite condensado 100g',6.75,'https://www.receitasmais.com.br/wp-content/uploads/2016/05/Pudim-de-leite-condensado.png',50,now(),(Select CategoriaId from Categorias where Nome='Sobremesas'))");
        }

        protected override void Down(MigrationBuilder mb)
        {
            mb.Sql("Delete from Categorias");
            mb.Sql("Delete from Produtos");
        }
    }
}
