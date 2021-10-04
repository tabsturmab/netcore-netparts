using System;
using System.Collections.Generic;
using AutoMapper;
using NetParts.Libraries.Json.Resolver;
using NetParts.Libraries.Text;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Models.ProductAggregator;
using Newtonsoft.Json;
using PagarMe;

namespace NetParts.Libraries.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Advertisement, ProductItem>();

            CreateMap<Transaction, TransacaoPagarMe>();

            CreateMap<TransacaoPagarMe, Order>()
                .ForMember(dest => dest.IdOrder, opt => opt.MapFrom(orig => 0))
                .ForMember(dest => dest.IdTecAssistance, opt => opt.MapFrom(orig => int.Parse(orig.Customer.ExternalId)))
                .ForMember(dest => dest.TransactionId, opt => opt.MapFrom(orig => orig.Id))
                .ForMember(dest => dest.FreightCompany, opt => opt.MapFrom(orig => "ECT - Correios"))
                .ForMember(dest => dest.FormPayment, opt => opt.MapFrom(orig => (orig.PaymentMethod == 0) ? MethodPaymentConstant.CartaoCredito : MethodPaymentConstant.Boleto))
                .ForMember(dest => dest.DataTransaction, opt => opt.MapFrom(orig => JsonConvert.SerializeObject(orig)))
                .ForMember(dest => dest.DateRegisterOrder, opt => opt.MapFrom(orig => DateTime.Now))
                .ForMember(dest => dest.ValueTotal, opt => opt.MapFrom(orig => Mascara.ConverterPagarMeIntToDecimal(orig.Amount)));

            CreateMap<Order, OrderSituation>()
                .ForMember(dest => dest.IdOrderSituation, opt => opt.MapFrom(orig => 0))
                .ForMember(dest => dest.IdOrder, opt => opt.MapFrom(orig => orig.IdOrder))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(orig => DateTime.Now));

            CreateMap<TransactionAdvertisement, OrderSituation>()
                .ForMember(dest => dest.Data, opt => opt.MapFrom(orig => JsonConvert.SerializeObject(orig, new JsonSerializerSettings() { ContractResolver = new ProductItemResolver<List<ProductItem>>() })));

            CreateMap<ReverseOrderBillet, BankAccount>()
                .ForMember(dest => dest.BankCode, opt => opt.MapFrom(orig => orig.BankCode))
                .ForMember(dest => dest.Agencia, opt => opt.MapFrom(orig => orig.Agency))
                .ForMember(dest => dest.AgenciaDv, opt => opt.MapFrom(orig => orig.AgencyDv))
                .ForMember(dest => dest.Conta, opt => opt.MapFrom(orig => orig.Account))
                .ForMember(dest => dest.ContaDv, opt => opt.MapFrom(orig => orig.AccountDv))
                .ForMember(dest => dest.LegalName, opt => opt.MapFrom(orig => orig.LegalName))
                .ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(orig => orig.CNPJ));
        }
    }
}
