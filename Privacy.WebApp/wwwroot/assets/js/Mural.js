//$(document).ready(function () {
//    $("#publish-button").attr("disabled", "disabled");
   
//});


//function VerificaTextoMural(texto) {

//    var botaoPublicar = $("#publish-button").attr("disabled", "disabled");

//    if (texto.length > 0) {
//        botaoPublicar.removeAttr("disabled");
//    }
    
//}

function Like(idUsuario, idPost) {

    var span = document.getElementById("countLikes(" + idPost + ")");

    var url = "/Index?handler=Like";
    $.ajax({
        url: url,
        data: { IdUsuario: idUsuario, IdPost: idPost },
        cache: false,
        async: true,
        type: "GET",
        success: function (data) {

            if (!data.OK) {
                span.innerHTML = data.Likes;
                return false;
            }
            else {
                span.innerHTML = data.Likes;
                return true;
            }
        },
        error: function (reponse) {
            //alert("error : " + reponse);
        }
    });
}

function Comentar(idPost, idUsuario, idComentraio) {

    var textoComentario = document.getElementById("comentario(" + idPost+")").value;
    
    var url = "/Index?handler=Comentar";
    $.ajax({
        url: url,
        data: { IdUsuario: idUsuario, IdPost: idPost, Texto: textoComentario, IdComentario: idComentraio },
        cache: false,
        async: true,
        type: "GET",
        success: function (data) {

            if (!data.OK) {
                alert("Insira algum comentário.");
                return false;
            }
            else {
                location.reload();
                return true;
            }
        },
        error: function (reponse) {
            //alert("error : " + reponse);
        }
    });
}

function VerificaConteudo(IdPost, IdUsuario) {

    var comentario = document.getElementsByClassName("emojionearea-editor").value;
    console.log(comentario);

    var botaoComentar = document.getElementById("btnComentar(" + IdPost + ", " + IdUsuario + ")");

    //if (comentario.length > 0) {
    //    botaoComentar.removeAttribute("disabled");
    //}
    
}

function DesabilitarBotaoComentar(IdPost, IdUsuario) {

    //var botaoComentar = document.getElementById("btnComentar(" + IdPost + ", " + IdUsuario + ")");
    //botaoComentar.setAttribute("disabled", "disabled");

    

    //return false;

}



function EditarComentario(idComentario, idPost) {

    var comentario = document.getElementById("comentarioPostado(" + idComentario + ")").innerHTML;

    //var teste = $('.emojionearea-editor').attr("id", "textAreaComentario(" + idComentario + ")");

    //teste.html(comentario);

    
}

function OcultarMostrarComentario(idComentario) {
    var url = "/Index?handler=OcultarMostrarComentario";
    $.ajax({
        url: url,
        data: { IdComentario: idComentario },
        cache: false,
        async: true,
        type: "GET",
        success: function (data) {

            if (!data.OK) {
                span.innerHTML = data.Likes;
                return false;
            }
            else {
                location.reload();
                return true;
            }
        },
        error: function (reponse) {
            //alert("error : " + reponse);
        }
    });
}