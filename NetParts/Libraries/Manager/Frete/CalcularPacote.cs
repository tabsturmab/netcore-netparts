using System;
using System.Collections.Generic;
using NetParts.Models;
using NetParts.Models.ProductAggregator;

namespace NetParts.Libraries.Manager.Frete
{
    public class CalcularPacote
    {
        private int comprimentoMinimo = 15;
        private int comprimentoMaximo = 100;
        private int larguraMinima = 10;
        private int larguraMaxima = 100;
        private int alturaMinima = 1;
        private int alturaMaxima = 100;
        private int pesoMaximo = 30;

        public List<Pacote> CalcularPacotesDeProdutos(List<ProductItem> productItem)
        {
            List<Pacote> pacotes = new List<Pacote>();
            Pacote pacote = new Pacote();

            int alturaProduto = 0;
            int larguraProduto = 0;
            int comprimentoProduto = 0;
            double pesoProdutos = 0;

            foreach (var prod in productItem)
            {
                if (!tamanhoValido(prod.Product.Height, prod.Product.Width1, prod.Product.Length, prod.Product.Weight))
                {
                    throw new Exception("Tamanho fora dos padrões dos Correios!");
                }

                pesoProdutos += prod.Product.Weight * prod.QuantityProduct;

                if (pesoProdutos > pesoMaximo)
                {

                    pacote = retornaTamanhoPacote(alturaProduto, larguraProduto, comprimentoProduto);
                    pacote.Peso = pesoProdutos - (prod.Product.Weight * prod.QuantityProduct);
                    pacotes.Add(pacote);

                    alturaProduto = 0;
                    larguraProduto = 0;
                    comprimentoProduto = 0;
                    pesoProdutos = 0;
                }

                if (prod.Product.Height > alturaProduto)
                {
                    alturaProduto = prod.Product.Height;
                }
                if (prod.Product.Width1 > larguraProduto)
                {
                    larguraProduto = prod.Product.Width1;
                }
                if (prod.Product.Length > comprimentoProduto)
                {
                    comprimentoProduto = prod.Product.Length;
                }
            }
            pacote = retornaTamanhoPacote(alturaProduto, larguraProduto, comprimentoProduto);
            pacote.Peso = pesoProdutos;
            pacotes.Add(pacote);

            return pacotes;
        }

        public bool tamanhoValido(int alturaProduto, int larguraProduto, int comprimentoProduto, double peso)
        {
            if (alturaProduto > alturaMaxima || larguraProduto > larguraMaxima ||
                comprimentoProduto > comprimentoMaximo || peso > pesoMaximo)
            {
                return false;
            }

            return true;
        }

        public Pacote retornaTamanhoPacote(int alturaProduto, int larguraProduto, int comprimentoProduto)
        {
            Pacote pacote = new Pacote();
            
            if (alturaProduto <= this.alturaMinima)
            {
                pacote.Altura = this.alturaMinima;
            }
            else
            {
                pacote.Altura = alturaProduto;
            }

            if (larguraProduto <= this.larguraMinima)
            {
                pacote.Largura = this.larguraMinima;
            }
            else
            {
                pacote.Largura = larguraProduto;
            }
            if (comprimentoProduto <= this.comprimentoMinimo)
            {
                pacote.Comprimento = this.comprimentoMinimo;
            }
            else
            {
                pacote.Comprimento = comprimentoProduto;
            }
            return pacote;
        }
    }
}
