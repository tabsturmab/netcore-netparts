$(function () {
    $('#codigo').focus();//documento pronto da o foco ao campo codigo   
    //chama a classe pai no click acha o classe info-link onde tem o link que vai acionar o evento click..mostra
    $('.form-group-pai').on('click', 'a.info-link', function (event) {
        event.preventDefault();
        //show() hide()
        //$(this).closest('.form-group-pai').find('.more-info').
        $(this).closest('.form-group-pai').find('.more-info').

            //toggle(1000, function () { desliza para baixo devido tempo
            //fadeToggle(1000, function () {//aparece devido tempo
            slideToggle('slow', function () {//desvencia para baixo devido tempo que pode ser slow fast ou milisecundos                 
                $('#codigo').focus();
            });
    });


});