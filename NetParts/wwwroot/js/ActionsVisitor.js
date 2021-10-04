$(document).ready(function () {
    
    MoveScrollOrdination();
    ChangeOrder();
    ChangeImageMasterProduct();
    ChangeQuantityProductCart();

    MascaraCEP();
    AJAXBuscarCEP();
    AcaoCalcularFreteBTN();
    AJAXCalcularFrete(false);

    AJAXEnderecoEntregaCalcularFrete();
    PrintOutBtnOrder();

    if ($.cookie !== undefined) {
        atribuirCepRadioButton($.cookie('Cart.CEP'));
    }
});

function AJAXEnderecoEntregaCalcularFrete() {

    $("input[name=endereco]").change(function () {
        var id =$(this).parent().find("input[name=cep]").val();
        $.cookie("Cart.EnderecoCookie", id, { path: "/" });
        var cep = RemoverMascara($(this).parent().find("input[name=endereco]").val());
        

        EnderecoEntregaCardsLimpar();
        LimparValores();
        EnderecoEntregaCardsLoading();
        $(".btn-continuar").addClass("disabled");;

        $.ajax({
            type: "GET",
            url: "/ShoppingCart/CalcularFrete?cepDestino=" + cep,
            error: function (data) {
                ChangeMessageError("Erro ao obter o Frete..." + data.Message);

                EnderecoEntregaCardsLimpar();
            },
            success: function (data) {
                EnderecoEntregaCardsLimpar();

                for (var i = 0; i < data.listValues.length; i++) {
                    var tipoFrete = data.listValues[i].tipoFrete;
                    var valor = data.listValues[i].valor;
                    var prazo = data.listValues[i].prazo;

                    $(".card-title")[i].innerHTML = "<label for='" + tipoFrete + "'>" + tipoFrete + "</label>";
                    $(".card-text")[i].innerHTML = "<label for='" + tipoFrete + "'>Prazo de " + prazo + " dias.</label>";
                    $(".card-footer .text-muted")[i].innerHTML = "<input type=\"radio\" name=\"frete\" value=\"" + tipoFrete + "\" id='" + tipoFrete + "' /> <strong><label for='" + tipoFrete + "'>" + numberToReal(valor) + "</label></strong>";

                    console.info($.cookie("Cart.TipoFrete") + " - " + tipoFrete)
                    console.info($.cookie("Cart.TipoFrete") === tipoFrete);

                    if ($.cookie("Cart.TipoFrete") !== undefined && $.cookie("Cart.TipoFrete") === tipoFrete) {
                        $(".card-footer .text-muted input[name=frete]").eq(i).attr("checked", "checked");
                        SelecionarTipoFreteStyle($(".card-footer .text-muted input[name=frete]").eq(i));

                        $(".btn-continuar").removeClass("disabled");
                    }
                }

                $(".card-footer .text-muted").find("input[name=frete]").change(function () {
                    $.cookie("Cart.TipoFrete", tipoFrete, { path: '/' });
                    $(".btn-continuar").removeClass("disabled");

                    SelecionarTipoFreteStyle($(this));
                });
            }
        });
    });
}

function SelecionarTipoFreteStyle(obj) {
    $(".card-body").css("background-color", "white");
    $(".card-footer").css("background-color", "rgba(0,0,0,.03)");

    obj.parent().parent().parent().find(".card-body").css("background-color", "#D7EAFF");
    obj.parent().parent().parent().find(".card-footer").css("background-color", "#D7EAFF");

    AtualizarValores();
}

function AtualizarValores() {
    var produto = parseFloat($(".texto-produto").text().replace("R$", "").replace(".", "").replace(",", "."));
    var frete = parseFloat($(".card-footer input[name=frete]:checked").parent().find("label").text().replace("R$", "").replace(".", "").replace(",", "."));

    var total = produto + frete;

    $(".texto-frete").text(numberToReal(frete));
    $(".texto-total").text(numberToReal(total));
}

function LimparValores() {
    $(".texto-frete").text("-");
    $(".texto-total").text("-");
}

function EnderecoEntregaCardsLoading() {
    for (var i = 0; i < 3; i++) {
        $(".card-text")[i].innerHTML = "<br /><br /><img src='\\img\\loading.gif' />";
    }
}

function EnderecoEntregaCardsLimpar() {
    for (var i = 0; i < 3; i++) {
        $(".card-title")[i].innerHTML = "-";
        $(".card-text")[i].innerHTML = "-";
        $(".card-footer .text-muted")[i].innerHTML = "-";
    }
}

function AJAXBuscarCEP() {
    $("#ZipCode").keyup(function () {
        HideMessageError();
        if ($(this).val().length === 9) {

            var cep = RemoverMascara($(this).val());
            $.ajax({
                type: "GET",
                url: "https://viacep.com.br/ws/" + cep + "/json/?callback=callback_name",
                dataType: "jsonp",
                error: function (data) {
                    console.info("Erro na busca pelo CEP! Servidores estão offline!");
                },
                success: function (data) {
                    if (data.erro === undefined) {
                        $("#Address1").val(data.logradouro);
                        $("#District").val(data.bairro);
                        $("#City").val(data.localidade);
                        $("#State1").val(data.uf);
                    }
                    else {
                        ChangeMessageError("O CEP informado não existe!");
                    }
                }
            });
        }
    });
}

function MascaraCEP() {
    $(".cep").mask("00000-000");
}

function AcaoCalcularFreteBTN() {
    $(".btn-calcular-frete").click(function (e) {
        AJAXCalcularFrete(true);
        e.preventDefault();
    });
}

function AJAXCalcularFrete(callByButton) {
    let cepCookie;
    if ($.cookie !== undefined) {
        cepCookie = $.cookie('Cart.CEP');
    }

    let cepTextBox = $(".cep").val();
    $(".btn-continuar").addClass("disabled");

    if (callByButton === false) {

        if (cepCookie !== undefined) {

            if ($(".remove-cep").length <= 0) {
                $(".cep").val(cepCookie);
                cepTextBox = cepCookie;
                $("#ZipCode").keyup();
            }

        }
    }

    if (cepTextBox !== undefined && cepTextBox.length > 0) {

        const cep = RemoverMascara(cepTextBox);

        if (cep.length === 8) {
            $.cookie('Cart.CEP', cepTextBox);
            $(".container-frete").html("<br /><br /><img src='\\img\\loading.gif' />");
     

            $.ajax({
                type: "GET",
                url: "/ShoppingCart/CalcularFrete?cepDestino=" + cep,
                error: function (data) {
                    ChangeMessageError("Opps! Tivemos um erro ao obter o Frete..." + data.Message);
                    console.error(data);
                },
                success: function (data) {
                    html = "";

                    for (var i = 0; i < data.listValues.length; i++) {
                        var tipoFrete = data.listValues[i].tipoFrete;
                        var valor = data.listValues[i].valor;
                        var prazo = data.listValues[i].prazo;

                        html += "<dl class=\"dlist-align\"><dt><input type=\"radio\" name=\"frete\" value=\"" +
                            tipoFrete +
                            "\" /><input type=\"hidden\" name=\"valor\" value=\"" +
                            valor +
                            "\" /></dt><dd>" +
                            tipoFrete +
                            " - " +
                            numberToReal(valor) +
                            " (" +
                            prazo +
                            " dias últeis)</dd></dl>";
                    }
                    $(".container-frete").html(html);
                    $(".container-frete").find("input[type=radio]").change(function () {

                        $.cookie("Cart.TipoFrete", $(this).val());
                        $(".btn-continuar").removeClass("disabled");

                        var valorFrete = parseFloat($(this).parent().find("input[type=hidden]").val());

                        $(".frete").text(numberToReal(valorFrete));

                        var subtotal = parseFloat($(".subtotal").text().replace("R$", "").replace(".", "")
                            .replace(",", "."));
                        console.info("Subtotal: " + subtotal);

                        var total = valorFrete + subtotal;

                        $(".total").text(numberToReal(total));
                    });
                }
            });
        } else {
            if (callByButton === true) {
                $(".container-frete").html("");
                ChangeMessageError("Digite o CEP para calcular o frete!");
            }
        }
    }
}

function atribuirCepRadioButton(cep) {

    if (cep !== undefined && cep !== null) {
        cep = RemoverMascara(cep);
        const radio = $("input:radio[value='" + cep + "']");
        radio.prop('checked', true);
        radio.change();
    }
}

function numberToReal(numero) {
    var numero = numero.toFixed(2).split('.');
    numero[0] = "R$ " + numero[0].split(/(?=(?:...)*$)/).join('.');
    return numero.join(',');
}

function ChangeQuantityProductCart() {
    $("#order .btn-primary").click(function () {
        if ($(this).hasClass("less")) {
            ControllerActionProduct("less", $(this));
        }
        if ($(this).hasClass("more")) {
            ControllerActionProduct("more", $(this));
        }
    });
}

function ControllerActionProduct(operation, button) {
    HideMessageError();

    var pai = button.parent().parent();

    var idAdvert = pai.find(".inputIdAdvert").val();
    var quantityStock = parseInt(pai.find(".inputQuantityStock").val());
    var priceUnitary = parseFloat(pai.find(".inputPriceUnitary").val().replace(",", "."));

    var fieldQuantityProductCart = pai.find(".inputQuantityProductCart");
    var quantityProductCartOld = parseInt(fieldQuantityProductCart.val());

    var fieldPrice = button.parent().parent().parent().parent().parent().find(".price");

    var advertisement = new ProductQuantityEPrice(idAdvert, quantityStock, priceUnitary, quantityProductCartOld, 0, fieldQuantityProductCart, fieldPrice);

    ChangesVisualProductCart(advertisement, operation);
}

function ChangesVisualProductCart(advertisement, operation) {
    if (operation === "more") {
        /*if (advertisement.quantityProductCartOld == advertisement.quantityStock) {
            alert("Não possuimos estoque suficiente para a quantidade que você deseja comprar!");
        } else*/ {
            advertisement.quantityProductCartNew = advertisement.quantityProductCartOld + 1;

            UpdateQuantityEPrice(advertisement);

            AJAXCommunicateChangeQuantityProduct(advertisement);

        }
    } else if (operation === "less") {
        /*if (advertisement.quantityProductCartOld == 1) {
            alert("Caso não deseje este produto, clique no botão Remover");
        } else*/ {
            advertisement.quantityProductCartNew = advertisement.quantityProductCartOld - 1;

            UpdateQuantityEPrice(advertisement);

            AJAXCommunicateChangeQuantityProduct(advertisement);
        }
    }
}

function AJAXCommunicateChangeQuantityProduct(advertisement) {
    $.ajax({
        type: "GET",
        url: "/ShoppingCart/ChangeQuantity?id=" + advertisement.idAdvert + "&quantity=" + advertisement.quantityProductCartNew,
        error: function (data) {
            ChangeMessageError(data.responseJSON.message);

            //Rollback
            advertisement.quantityProductCartNew = advertisement.quantityProductCartOld;
            UpdateQuantityEPrice(advertisement);
        },
        success: function () {
            AJAXCalcularFrete();

        }
    });
}

function ChangeMessageError(message) {
    $(".alert-danger").css("display", "block");
    $(".alert-danger").text(message);
}

function HideMessageError() {
    $(".alert-danger").css("display", "none");
}

function UpdateQuantityEPrice(advertisement) {
    advertisement.fieldQuantityProductCart.val(advertisement.quantityProductCartNew);

    var result = advertisement.priceUnitary * advertisement.quantityProductCartNew;
    advertisement.fieldPrice.text(numberToReal(result));

    UpdateSubtotal();
}

function UpdateSubtotal() {
    var Subtotal = 0;
    var TagsComPrice = $(".price");
    TagsComPrice.each(function () {
        var priceReais = parseFloat($(this).text().replace("R$", "").replace(".", "").replace(" ", "").replace(",", "."));
        Subtotal += priceReais;
    })
    $(".subtotal").text(numberToReal(Subtotal));
}

function ChangeImageMasterProduct() {
    $(".img-small-wrap img").click(function () {
        var Way = $(this).attr("src");
        $(".img-big-wrap img").attr("src", Way);
        $(".img-big-wrap a").attr("href", Way);
    });
}

function MoveScrollOrdination() {
    if (window.location.hash.length > 0) {
        var hash = window.location.hash;
        if (hash === "#position-product") {
            window.scrollBy(0, 773);
        }
    }
}

function ChangeOrder() {
    $("#ordination").change(function () {
        var Page = 1;
        var Search = "";
        var Ordination = $(this).val();
        var Fragment = "#position-product";

        var QueryString = new URLSearchParams(window.location.search);
        if (QueryString.has("page")) {
            Page = QueryString.get("page");
        }
        if (QueryString.has("search")) {
            Search = QueryString.get("search");
        }
        if ($("#breadcrumb").length > 0) {
            Fragment = "";
        }

        var URL = window.location.protocol + "//" + window.location.host + window.location.pathname;

        var URLWithParameters = URL + "?page=" + Page + "&search=" + Search + "&ordination=" + Ordination + Fragment;
        window.location.href = URLWithParameters;

    });
}

function RemoverMascara(valor) {

    if (valor !== undefined && valor !== null)
        return valor.replace("-", "");

    return null;
}

function PrintOutBtnOrder() {
    $(".btn-printout").click(function () {
        window.print();
    });
}

class ProductQuantityEPrice {
    constructor(idAdvert, quantityStock, priceUnitary, quantityProductCartOld, quantityProductCartNew, fieldQuantityProductCart, fieldPrice) {
        this.idAdvert = idAdvert;
        this.quantityStock = quantityStock;
        this.priceUnitary = priceUnitary;

        this.quantityProductCartOld = quantityProductCartOld;
        this.quantityProductCartNew = quantityProductCartNew;

        this.fieldQuantityProductCart = fieldQuantityProductCart;
        this.fieldPrice = fieldPrice;
    }
}
