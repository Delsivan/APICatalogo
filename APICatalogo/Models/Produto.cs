using APICatalogo.Validations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APICatalogo.Models
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        public int ProdutoId { get; set; }
        [Required(ErrorMessage ="O nome é obrigatório")]
        [StringLength(20, ErrorMessage ="O nome deve tem entre 2 e 20 caracteres", MinimumLength = 2)]
        [PrimeiraLetraMaiuscula]
        public string Nome { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "A descrição deve ter no maximo{1} caracteres")]
        public string Descricao { get; set; }
        [Required]
        [Range(1, 10000, ErrorMessage = "O preço estar ente {1} e {2}")]
        public decimal Preco { get; set; }
        [Required]
        [StringLength(300, MinimumLength = 10)]
        public string ImagemUrl { get; set; }
        public float Estoque { get; set; }
        public DateTime DataCadastro { get; set; }

        public Categoria Categoria { get; set; }
        public int CategoriaId { get; set; }
    }
}
