using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using NetParts.Libraries.Login;
using NetParts.Libraries.Text;
using NetParts.Models;
using NetParts.Models.ProductAggregator;
using NetParts.Repositories.Contracts;
using PagarMe;

namespace NetParts.Libraries.Manager.Pagamento.PagarMe
{
    public class GerenciarPagarMe
    {
        private IConfiguration _configuration;
        private IMapper _mapper;
        private LoginCollaborator _loginCollaborator;

        public GerenciarPagarMe(IConfiguration configuration, IMapper mapper, LoginCollaborator loginCollaborator)
        {
            _configuration = configuration;
            _mapper = mapper;
            _loginCollaborator = loginCollaborator;
        }
        public Transaction GerarBoleto(decimal valor, List<ProductItem> products, Models.Address enderecoEntrega, ValorPrazoFrete valorPrazoFrete)
        {
            TechnicalAssistance technicalAssistancebuy = _loginCollaborator.GetCollaborator().TechnicalAssistance;

            PagarMeService.DefaultApiKey = _configuration.GetValue<String>("Pagamento:PagarMe:ApiKey");
            PagarMeService.DefaultEncryptionKey = _configuration.GetValue<String>("Pagamento:PagarMe:EncryptionKey");
            int DaysExpire = _configuration.GetValue<int>("Pagamento:PagarMe:BoletoDiaExpiracao");

            Transaction transaction = new Transaction();

            transaction.Amount = Mascara.ConverterValorPagarMe(Convert.ToInt32(valor));
            transaction.PaymentMethod = PaymentMethod.Boleto;
            transaction.BoletoExpirationDate = DateTime.Now.AddDays(DaysExpire);

            transaction.Customer = new Customer
            {
                ExternalId = technicalAssistancebuy.IdTecAssistance.ToString(),
                Name = technicalAssistancebuy.SocialReason,
                Type = CustomerType.Corporation,
                Country = "br",
                Email = technicalAssistancebuy.EmailAta,
                Documents = new[]
                {
                        new Document
                        {
                            Type = DocumentType.Cnpj,
                            Number = Mascara.Remover(technicalAssistancebuy.Cnpj)
                        }
                    },
                PhoneNumbers = new string[]
                {
                        "+55" + Mascara.Remover(technicalAssistancebuy.Phone),
                }
            };

            var Today = DateTime.Now;
            var fee = Convert.ToDecimal(valorPrazoFrete.Valor);

            transaction.Shipping = new Shipping //Dados da Ata/Cliente - Endereço Entrega
            {
                Name = technicalAssistancebuy.SocialReason,
                Fee = Mascara.ConverterValorPagarMe(fee),
                DeliveryDate = Today.AddDays(_configuration.GetValue<int>("Frete:DiasNaEmpresa"))
                    .AddDays(valorPrazoFrete.Prazo).ToString("yyyy-MM-dd"),
                Expedited = false,
                Address = new global::PagarMe.Address()
                {
                    Country = "br",
                    State = enderecoEntrega.State1,
                    City = enderecoEntrega.City,
                    Neighborhood = enderecoEntrega.District,
                    Street = enderecoEntrega.Address1 + " " + enderecoEntrega.Complement,
                    StreetNumber = enderecoEntrega.NumberAta,
                    Zipcode = Mascara.Remover(enderecoEntrega.ZipCode)
                }
            };
            Item[] itens = new Item[products.Count];

            for (var i = 0; i < products.Count; i++)
            {
                var item = products[i];

                var itemA = new Item()
                {
                    Id = item.IdAdvert.ToString(),
                    Title = item.Product.Description,
                    Quantity = item.QuantityProduct,
                    Tangible = true,
                    UnitPrice = Mascara.ConverterValorPagarMe(Convert.ToDecimal(item.Price))
                };
                itens[i] = itemA;
            }
            transaction.Item = itens;

            transaction.Save();

            transaction.Customer.Gender = Gender.Female;
            return transaction;
        }

        public Transaction GerarPagCartaoCredito(CartaoCredito cartao, Parcelamento parcelamento, Models.Address enderecoFatura,
            Models.Address enderecoEntrega, ValorPrazoFrete valorPrazoFrete, List<ProductItem> advertisement)
        {
            TechnicalAssistance technicalAssistancebuy = _loginCollaborator.GetCollaborator().TechnicalAssistance;

            PagarMeService.DefaultApiKey = _configuration.GetValue<String>("Pagamento:PagarMe:ApiKey");
            PagarMeService.DefaultEncryptionKey = _configuration.GetValue<String>("Pagamento:PagarMe:EncryptionKey");

            Card card = new Card();
            card.Number = cartao.NumberCard;
            card.HolderName = cartao.NameOnCard;
            card.ExpirationDate = cartao.ExpirationMM + cartao.ExpirationYY;
            card.Cvv = cartao.SecurityCode;

            card.Save();

            Transaction transaction = new Transaction();
            transaction.PaymentMethod = PaymentMethod.CreditCard;

            transaction.Card = new Card
            {
                Id = card.Id
            };

            transaction.Customer = new Customer
            {
                ExternalId = technicalAssistancebuy.IdTecAssistance.ToString(),
                Name = technicalAssistancebuy.SocialReason,
                Type = CustomerType.Corporation,
                Country = "br",
                Email = technicalAssistancebuy.EmailAta,
                Documents = new[]
                {
                    new Document
                    {
                        Type = DocumentType.Cnpj,
                        Number = Mascara.Remover(technicalAssistancebuy.Cnpj)
                    }
                },
                PhoneNumbers = new string[]
                {
                    "+55" + Mascara.Remover(technicalAssistancebuy.Phone),
                }

            };

            transaction.Billing = new Billing //Dados da ATA Cliente - Endereço Fatura
            {
                Name = technicalAssistancebuy.SocialReason,
                Address = new global::PagarMe.Address()
                {
                    Country = "br",
                    State = enderecoFatura.State1,
                    City = enderecoFatura.City,
                    Neighborhood = enderecoFatura.District,
                    Street = enderecoFatura.Address1 + " " + enderecoFatura.Complement,
                    StreetNumber = enderecoFatura.NumberAta,
                    Zipcode = Mascara.Remover(enderecoFatura.ZipCode)
                }
            };

            var Today = DateTime.Now;
            var fee = Convert.ToDecimal(valorPrazoFrete.Valor);

            transaction.Shipping = new Shipping //Dados da Ata/Cliente - Endereço Entrega
            {
                Name = technicalAssistancebuy.SocialReason,
                Fee = Mascara.ConverterValorPagarMe(fee),
                DeliveryDate = Today.AddDays(_configuration.GetValue<int>("Frete:DiasNaEmpresa"))
                    .AddDays(valorPrazoFrete.Prazo).ToString("yyyy-MM-dd"),
                Expedited = false,
                Address = new global::PagarMe.Address()
                {
                    Country = "br",
                    State = enderecoEntrega.State1,
                    City = enderecoEntrega.City,
                    Neighborhood = enderecoEntrega.District,
                    Street = enderecoEntrega.Address1 + " " + enderecoEntrega.Complement,
                    StreetNumber = enderecoEntrega.NumberAta,
                    Zipcode = Mascara.Remover(enderecoEntrega.ZipCode)
                }
            };
            Item[] itens = new Item[advertisement.Count];

            for (var i = 0; i < advertisement.Count; i++)
            {
                var item = advertisement[i];

                var itemA = new Item()
                {
                    Id = item.IdAdvert.ToString(),
                    Title = item.Product.Description,
                    Quantity = item.QuantityProduct,
                    Tangible = true,
                    UnitPrice = Mascara.ConverterValorPagarMe(Convert.ToDecimal(item.Price))
                };
                itens[i] = itemA;
            }

            transaction.Item = itens;
            transaction.Amount = Mascara.ConverterValorPagarMe(parcelamento.Valor);
            transaction.Installments = parcelamento.Numero;

            transaction.Save();

            transaction.Customer.Gender = Gender.Female;
            return transaction;
        }

        public List<Parcelamento> CalcularPagamentoParcelado(decimal valor)
        {
            List<Parcelamento> list = new List<Parcelamento>();

            int maxParcelamento = _configuration.GetValue<int>("Pagamento:PagarMe:MaxParcelas");
            int parcelaPagaVendedor = _configuration.GetValue<int>("Pagamento:PagarMe:ParcelaPagaVendedor");
            decimal juros = _configuration.GetValue<decimal>("Pagamento:PagarMe:Juros");

            for (int i = 1; i <= maxParcelamento; i++)
            {
                Parcelamento parcelamento = new Parcelamento();
                parcelamento.Numero = i;

                if (i > parcelaPagaVendedor)
                {
                    //Juros - i = (4-3 - parcelaPagaVendedor) + 5%
                    int quantidadeParcelasComJuros = i - parcelaPagaVendedor;
                    decimal valorDoJuros = valor * juros / 100;

                    parcelamento.Valor = quantidadeParcelasComJuros * valorDoJuros + valor;
                    parcelamento.ValorPorParcela = parcelamento.Valor / parcelamento.Numero;
                    parcelamento.Juros = true;
                }
                else
                {
                    parcelamento.Valor = valor;
                    parcelamento.ValorPorParcela = parcelamento.Valor / parcelamento.Numero;
                    parcelamento.Juros = false;
                }
                list.Add(parcelamento);
            }
            return list;
        }
        public Transaction GetTransaction(string transactionId)
        {
            PagarMeService.DefaultApiKey = _configuration.GetValue<String>("Pagamento:PagarMe:ApiKey");

            return PagarMeService.GetDefaultService().Transactions.Find(transactionId);
        }
        public Transaction ReversalCardCredit(string transactionId)
        {
            PagarMeService.DefaultApiKey = _configuration.GetValue<String>("Pagamento:PagarMe:ApiKey");

            var transaction = PagarMeService.GetDefaultService().Transactions.Find(transactionId);

            transaction.Refund();

            return transaction;
        }
        public Transaction ReversalBillet(string transactionId, ReverseOrderBillet billet)
        {
            PagarMeService.DefaultApiKey = _configuration.GetValue<String>("Pagamento:PagarMe:ApiKey");

            var transaction = PagarMeService.GetDefaultService().Transactions.Find(transactionId);

            var bankAccount = _mapper.Map<ReverseOrderBillet, BankAccount>(billet);

            transaction.Refund(bankAccount);

            return transaction;
        }
    }
}
