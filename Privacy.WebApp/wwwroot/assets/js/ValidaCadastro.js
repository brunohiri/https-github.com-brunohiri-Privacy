
$(document).ready(function () {
    var botaoIrPasso3 = $("#btnIrPasso3");
    botaoIrPasso3.attr("disabled", "disabled").attr("data-step", "").css("cursor", "not-allowed").css("background-color", "#dbdbdb");

    //ValidarPasso2();
    //ValidarPasso4();
    //$("#btnIrPasso3").attr("disabled", "disabled").attr("data-step", "").css("cursor", "not-allowed").css("background-color", "#dbdbdb");
    $('a[name="LinkTipoPessoa"]').click(function () {
        $('#HiddenTipoPessoa').val($(this).data('id'));
        if ($(this).data('id') == 1)
            $('#divQuantoQuer').show();
        else
            $('#divQuantoQuer').hide();
    });
});

function ValidarPasso2() {

    var botaoIrPasso3 = $("#btnIrPasso3");

    var nome = $("#Nome").val();
    var cpf = $("#CPF").val();
    var dataNascimento = $("#DataNascimento").val();
    var email = $("#Email").val();
    var perfilPublico = $('#HiddenTipoPessoa').val();
    var etnia = $("#IdEtnia").val();
    var genero = $("#IdGenero").val();
    var interesse = $("#IdInteresse").val();

    //if (email != "") {
    //    if (!ValidarEmail(email)) {
    //        botaoIrPasso3.attr("disabled", "disabled").attr("data-step", "").css("cursor", "not-allowed").css("background-color", "#dbdbdb");
    //        return false;
    //    }
    //}

    if (nome != "" /*&& cpf.length == 14*/ && dataNascimento != "" && email != "" && etnia > 0 && genero > 0 && interesse > 0) {
        PostDadosPasso2(nome, dataNascimento, email, cpf, perfilPublico, etnia, genero, interesse );
    }

    botaoIrPasso3.attr("disabled", "disabled").attr("data-step", "").css("cursor", "not-allowed").css("background-color", "#dbdbdb");
    return false;
    
}

function PostDadosPasso2(nome, dataNascimento, email, cpf, perfilPublico, etnia, genero, interesse) {

    var botaoIrPasso3 = $("#btnIrPasso3");

    var url = "/SignUp?handler=VerificarDados2";
    $.ajax({
        url: url,
        data: { Nome: nome, DataNascimento: dataNascimento, Email: email, PerfilPublico: perfilPublico, CPF: cpf, Etnia: etnia, Genero: genero, Interesse: interesse },
        cache: false,
        async: true,
        type: "GET",
        success: function (data) {

            if (!data.OK) {
                $("#MensagemDados").text(data.Mensagem);
                botaoIrPasso3.attr("disabled", "disabled").attr("data-step", "").css("cursor", "not-allowed").css("background-color", "#dbdbdb");
                return false;
            }
            else {
                $("#MensagemDados").text("");
                botaoIrPasso3.removeAttr("disabled").attr("data-step", "step-dot-3").css("cursor", "pointer").css("background-color", "#FFF");
                return true;
            }
        },
        error: function (reponse) {
            //alert("error : " + reponse);
        }
    });
}

function ValidarPasso3() {
    var botaoIrPasso4 = $("#btnIrPasso4");

    var fotoperfil = $("#upload-preview").attr("src");

    PostDadosPasso3(fotoperfil);

    botaoIrPasso4.attr("disabled", "disabled").attr("data-step", "").css("cursor", "not-allowed").css("background-color", "#dbdbdb");
    return false;
}

function PostDadosPasso3(fotoperfil) {

    var botaoIrPasso4 = $("#btnIrPasso4");

    var url = "/SignUp?handler=Passo3";

    $.ajax({
        url: url,
        data: { FotoPerfil: encodeURIComponent(fotoperfil) },
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        cache: false,
        async: true,
        type: "POST",
        success: function (data) {

            if (!data.OK) {
                $("#MensagemDados").text(data.Mensagem);
                botaoIrPasso4.attr("disabled", "disabled").attr("data-step", "").css("cursor", "not-allowed").css("background-color", "#dbdbdb");
                return false;
            }
            else {
                $("#MensagemDados").text("");
                botaoIrPasso4.removeAttr("disabled").attr("data-step", "step-dot-5").css("cursor", "pointer").css("background-color", "#FFF");
                return true;
            }


        },
        error: function (reponse) {
            //alert("error : " + reponse);
        }
    });
}

function ValidarPasso4() {

    var botaoIrPasso5 = $("#btnIrPasso5");

    var senha = $("#Senha").val();
    var confirmarSenha = $("#ConfirmarSenha").val();
    var celular = $("#Celular").val();
    var quantoquer = $("#QuantoQuer").val();


    if (senha == "" || confirmarSenha == "" || celular.length < 14) {
        botaoIrPasso5.attr("disabled", "disabled").attr("data-step", "").css("cursor", "not-allowed").css("background-color", "#dbdbdb");
        return false;
    }
    else {
        
        if (senha != confirmarSenha) {
            botaoIrPasso5.attr("disabled", "disabled").attr("data-step", "").css("cursor", "not-allowed").css("background-color", "#dbdbdb");
            $("#confirmaSenha").html("As senhas são diferentes. Por favor, verifique");
            return false;
        }
        if (senha == confirmarSenha) {
            $("#confirmaSenha").html("");
        }

        if (senha == confirmarSenha && celular.length >= 14) {
            PostDadosPasso4(senha, celular, quantoquer);
            botaoIrPasso5.removeAttr("disabled").attr("data-step", "step-dot-5").css("cursor", "pointer").css("background-color", "#FFF");
        }

       return false;
    }
}

function PostDadosPasso4(senha, celular, quantoquer) {

    var botaoIrPasso5 = $("#btnIrPasso5");

    var url = "/SignUp?handler=Passo4";

    $.ajax({
        url: url,
        data: { Senha: senha, Celular: celular, QuantoQuer: quantoquer },
        cache: false,
        async: true,
        type: "GET",
        success: function (data) {

            if (!data.OK) {
                $("#MensagemDados").text(data.Mensagem);
                btnIrPasso5.attr("disabled", "disabled").attr("data-step", "").css("cursor", "not-allowed").css("background-color", "#dbdbdb");
                return false;
            }
            else {
                $("#MensagemDados").text("");
                //btnIrPasso5.removeAttr("disabled").attr("data-step", "step-dot-5").css("cursor", "pointer").css("background-color", "#FFF");
                return true;
            }


        },
        error: function (reponse) {
            //alert("error : " + reponse);
        }
    });
}

function PostSalvarDados() {

    var url = "/SignUp?handler=SalvarDados";
    $.ajax({
        url: url,
        data: null,
        headers: {
            RequestVerificationToken:
                $('input:hidden[name="__RequestVerificationToken"]').val()
        },
        cache: false,
        async: true,
        type: "POST",
        success: function (data) {

            if (!data.OK) {
                $("#MensagemDados").text(data.Mensagem);
                return false;
            }
            else {
                $("#MensagemDados").text("");
                return true;
            }


        },
        error: function (reponse) {
            //alert("error : " + reponse);
        }
    });
}


function ValidarEmail(email) {
    var emailReg = /^[a-z0-9.]+@[a-z0-9]+\.[a-z]+\.([a-z]+)?$/i;///^([\w-\.]+@([\w-]+\.)+[\w-]{3,4})?$/;

    if (!emailReg.test(email)) {
        return false;
    }
    else {
        return true;
    }
    //return emailReg.test(email);

}