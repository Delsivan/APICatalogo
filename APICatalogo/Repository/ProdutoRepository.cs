﻿using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using System.Collections.Generic;
using System.Linq;

namespace APICatalogo.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(AppDbContext contexto) : base(contexto)
        {

        }
        public PageList<Produto> GetProdutos(ProdutosParameters produtosParameters)
        {
            //return Get()
            //    .OrderBy(on => on.Nome)
            //   .Skip((produtosParameters.PageNumber - 1) * produtosParameters.PageSize)
            //   .Take(produtosParameters.PageSize)
            //   .ToList();

            return PageList<Produto>.ToPageList(Get().OrderBy(on => on.Nome),
                produtosParameters.PageNumber, produtosParameters.PageSize);
        }

        public IEnumerable<Produto> GetProdutosPorPreco()
        {
            return Get().OrderBy(c => c.Preco).ToList();
        }
    }
}