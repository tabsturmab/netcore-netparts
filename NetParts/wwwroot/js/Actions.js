$(document).ready(function () {
    $(".btn-danger").click(function (e) {
        var resultado = confirm("Tem certeza que deseja realizar esta operação?");

        if (!resultado) {
            e.preventDefault();
        }
    });

    $('.dinheiro').mask('000.000.000.000.000,00', { reverse: true });

    $(".cnpj").mask("99.999.999/9999-99");

    $(".phone").mask("(99) 99999-9999");

    $(".cpf").mask("999.999.999-99");

    $(".cep").mask("99999-999");

    CategorySlug();
});

$('.fileTec').on("change", function () {
    const img = $(this).parent().find(".img-upload");
    console.log(img);
    img.html('<b>PDF:</b>' + $(this).val());
});

function CategorySlug() {
    if ($("#form-category").length > 0) {
        $("input[name=NameCategory]").keyup(function () {
            $("input[name=Slug]").val(convertToSlug($(this).val()));
        });
    }
}

$('.imgFile').on("change", function () {
    const file = $(this)[0].files[0];
    const fileReader = new FileReader();
    const img = $(this).parent().find(".img-upload2");
    const btnDelete = $(this).parent().find(".btn-ocultar");
    fileReader.onloadend = function() {
        img.attr('src', fileReader.result);
        btnDelete.show();
    }
    fileReader.readAsDataURL(file);
});

$('.btn-ocultar').on("click", function () {

    const file = $(this).parent().find('.imgFile');
    const img = $(this).parent().find(".img-upload2");
    const btnDelete = $(this).parent().find(".btn-ocultar");

    file.val("");
    img.attr("src", "/img/image-padrao.png");
    btnDelete.hide();
});

$(".img-upload").click(function () {
    $(this).parent().find(".input-file").click();
});

$(".img-upload2").click(function () {
    $(this).parent().parent().find(".input-file").click();
});

function convertToSlug(Text) {
    return Text
            .toLowerCase()
            .replace(/ /g, '-')
            .replace(/[^\w-]+/g, '');
}