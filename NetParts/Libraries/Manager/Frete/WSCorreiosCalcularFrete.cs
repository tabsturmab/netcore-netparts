﻿using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetParts.Database;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Models.ProductAggregator;
using WSCorreios;

namespace NetParts.Libraries.Manager.Frete
{
    public class WSCorreiosCalcularFrete
    {
        private IConfiguration _configuration;
        private CalcPrecoPrazoWSSoap _servico;

        public WSCorreiosCalcularFrete(NetPartsContext banco, IConfiguration configuration, CalcPrecoPrazoWSSoap servico, Cookie.Cookie cookie)
        {
            _configuration = configuration;
            _servico = servico;
        }

        public async Task<ValorPrazoFrete> CalcularFrete(String cepDestino, String tipoFrete, List<Pacote> pacotes, List<ProductItem> productItems)
        {
            List<ValorPrazoFrete> ValorDosPacotesPorFrete = new List<ValorPrazoFrete>();

            foreach (var pacote in pacotes)
            {
                String cepOrigem = productItems.First().TechnicalAssistance.Address.First().ZipCode;
                var resultado = await CalcularValorPrazoFrete(cepOrigem, cepDestino, tipoFrete, pacote);
                if (resultado != null)
                    ValorDosPacotesPorFrete.Add(resultado);
            }

            if (ValorDosPacotesPorFrete.Count > 0)
            {
                ValorPrazoFrete ValorDosFretes = ValorDosPacotesPorFrete
                    .GroupBy(a => a.TipoFrete)
                    .Select(list => new ValorPrazoFrete
                    {
                        TipoFrete = list.First().TipoFrete,
                        CodTipoFrete = list.First().CodTipoFrete,
                        Prazo = list.Max(c => c.Prazo),
                        Valor = list.Sum(c => c.Valor)
                    }).ToList().First();

                return ValorDosFretes;
            }
            return null;
        }
        private async Task<ValorPrazoFrete> CalcularValorPrazoFrete(String cepOrigem, String cepDestino, String tipoFrete, Pacote pacote)
        {
            var maoPropria = _configuration.GetValue<String>("Frete:MaoPropria");
            var avisoRecebimento = _configuration.GetValue<String>("Frete:AvisoRecebimento");
            var diametro = Math.Max(Math.Max(pacote.Comprimento, pacote.Largura), pacote.Altura);

            cResultado resultado = await _servico.CalcPrecoPrazoAsync("", "", tipoFrete, cepOrigem, cepDestino, pacote.Peso.ToString(), 1, pacote.Comprimento, pacote.Altura, pacote.Largura, diametro, maoPropria, 0, avisoRecebimento);

            if (resultado.Servicos[0].Erro == "0")
            {
                var valorLimpo = resultado.Servicos[0].Valor.Replace(".", "");
                var valorFinal = double.Parse(valorLimpo);

                return new ValorPrazoFrete()
                {
                    TipoFrete = TipoFreteConstant.GetNames(tipoFrete),
                    CodTipoFrete = tipoFrete,
                    Prazo = int.Parse(resultado.Servicos[0].PrazoEntrega),
                    Valor = valorFinal
                };
            }
            else if (resultado.Servicos[0].Erro == "008" || resultado.Servicos[0].Erro == "-888")
            {
                //Ex.: SEDEX10 - não entrega naquela região
                return null;
            }
            else
            {
                throw new Exception("Erro: " + resultado.Servicos[0].MsgErro);
            }
        }
    }
}
