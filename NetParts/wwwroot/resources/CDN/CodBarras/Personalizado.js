$('.more-info').slideToggle();//esconde slade 
var count = new Number();
    count = 11;
setTimeout('validaCodigo();', 35);
function validaCodigo() { 

    var barras = $('#codigo');
    if (count - 1 >= 0) {
        count = count - 1;
        if (count === 0 ) {

            if (barras.val().trim().length >= 15) {
                //pega o id da tags que e uma div > inseri o span com mensageme em cores.       
                document.getElementById("erroCodigo").innerHTML = "<span style='color: #00ff00'>PartNumber Leitor.</span>";
                document.getElementById("Description").focus();
                count = 0;
            }
            else {
                //document.getElementById("erroCodigo").innerHTML = "<span style='color: #ff0000'>Use o leitor de código</span>";
                count = 11;
                apaga();
            }
            //count = 11;
        }
        else if (count < 11) {
            count = "0" + count;
        }
        demo.innerText = count + " " + $('#codigo').val().trim().length;
    }
    setTimeout('validaCodigo();', 35);
}

function stop() {
    var barras = $('#codigo');
    if (barras.val().trim().length < 14) {
        apaga(); barras.focus();
        document.getElementById('codigo').readOnly = false;
    } else {
        document.getElementById('codigo').readOnly = true;
    }
}
function apaga() {
    codigo.value = "";
}

$('.cod-manual').on('click', function () {

    if ($('#codigo').val().trim().length >= 15) {
        alert("Codigo já informado!!!");
        $('.more-info').slideToggle();//esconde slade 
    }
    else {
        $('#erroCodigo').html(
            "<span style='color: #00ff00'>PartNumber Manual</span>"); //.fadeOut(10000)some aou poucos
        count = 0;
        stop();

        $('.more-info').slideToggle();//esconde slade 
    }
});






