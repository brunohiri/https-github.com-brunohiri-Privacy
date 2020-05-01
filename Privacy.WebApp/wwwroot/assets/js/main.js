"use strict";

/*! main.js | Friendkit | © Css Ninja. 2019-2020 */

/* ==========================================================================
Main js file
========================================================================== */
$(document).ready(function () {
    "use strict"; //Code highlight init

    $('.highlight-block code').each(function (i, block) {
        hljs.highlightBlock(block);
    }); //Pageloader

    initPageloader(); //Init navbar

    initNavbar(); //Mobile menu toggle

    initResponsiveMenu(); //Navbar dropdown

    initNavDropdowns(); //Navbar Cart

    initNavbarCart(); //Common Dropdown

    initDropdowns(); //Sidebars

    initSidebar(); //Tabs

    initTabs(); //Modals

    initModals(); //Subnavbar search

    initSubSearch(); //Attribute background images

    initBgImages(); //Feather icons initialization

    feather.replace(); //Emojis

    initEmojis(); //Load More

    initLoadMore(); //Init tooltips

    initTooltips(); //Init Like Button

    initLikeButton(); //Init Simple Popover

    initSimplePopover(); //Share modal demo

    initShareModal(); //Init Plus Menu

    initPlusMenu(); //Init Tipuedrop

    initMask();

    //$('#tipue_drop_input').tipuedrop();

    if (window.location.href.toString().indexOf('Search') >= 0) {
        $('input[name="TextoPesquisa"]').val(window.location.href.split('=')[1]);
        $('input[name="TextoPesquisa"]').focus();
    }

    $('input[name="TextoPesquisa"]').keyup(function () {
        var url = "/Search";
        var retorno = '';
        var textoPesquisa = $(this).val();

        if ($(this).val().length == 0) {
            retorno += '<div class="add-friend-block transition-block">';
            retorno += '    <div class="page-meta">';
            retorno += '        <span>Sem sugestões</span>';
            retorno += '        <span></span>';
            retorno += '    </div>';
            retorno += '    <div class="add-friend add-transition">';
            retorno += '        <i data-feather="user-plus"></i>';
            retorno += '    </div>';
            retorno += '</div>';
            $('#divListPesquisa').html(retorno);
        }

        if ($(this).val().length == 1) {
            if (window.location.href.toString().indexOf('Search') == -1)
                window.location.href = url + '?TextoPesquisa=' + textoPesquisa;
        }

        if ($(this).val().length > 0) {
            $('#divListPesquisa').html('');
            $.ajax({
                url: url,
                data: JSON.stringify(textoPesquisa),
                headers: { "RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() },
                dataType: 'json',
                cache: false,
                async: true,
                type: "POST",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                    if (data.OK) {
                        if ((data.ListPesquisa == null) || (data.ListPesquisa.length == 0)) {
                            retorno += '<div class="add-friend-block transition-block">';
                            retorno += '    <div class="page-meta">';
                            retorno += '        <span>Sem sugestões</span>';
                            retorno += '        <span></span>';
                            retorno += '    </div>';
                            retorno += '    <div class="add-friend add-transition">';
                            retorno += '        <i data-feather="user-plus"></i>';
                            retorno += '    </div>';
                            retorno += '</div>';
                        }
                        else {
                            var j = 0;
                            for (var i = 0; i < data.ListPesquisa.length; i++) {
                                if (j == 0) {
                                    retorno += '<div class="columns">';
                                }
                                retorno += '<div class="column is-4">';
                                retorno += '    <div class="add-friend-block transition-block spacing-search">';
                                retorno += '        <a href="/ProfileAbout?Id=' + data.ListPesquisa[i].IdUsuario + '">';
                                retorno += '            <img src="' + data.ListPesquisa[i].FotoPerfil + '" data-demo-src="' + data.ListPesquisa[i].FotoPerfil + '" data-user-popover="9" alt="" class="img-search">';
                                retorno += '            <span class="perfilPublico"><img src="/assets/img/screens/perfilPublicoSignUp.png" alt="Perfil Público"></span>';
                                retorno += '        </a>';
                                retorno += '        <div class="page-meta" style="">';
                                retorno += '            <a href="/ProfileAbout?Id=' + data.ListPesquisa[i].IdUsuario + '">';
                                retorno += '                <span>' + data.ListPesquisa[i].Nome + '</span>';
                                retorno += '            </a>';
                                retorno += '            <span style="padding-top:5px;"><strong>Local:</strong> ' + data.ListPesquisa[i].Cidade + ', ' + data.ListPesquisa[i].Pais + '</span>';
                                retorno += '            <span class="span-search"><strong>Gênero:</strong> ' + data.ListPesquisa[i].Genero + '</span>';
                                retorno += '            <span class="span-search"><strong>Etnia:</strong> ' + data.ListPesquisa[i].Etnia + '</span>';
                                retorno += '        </div>';
                                retorno += '        <div class="add-friend add-transition">';
                                retorno += '            <i data-feather="user-plus"></i>';
                                retorno += '        </div>';
                                retorno += '    </div>';
                                retorno += '</div>';
                                j++;
                                if (j == 3) {
                                    retorno += '</div>';
                                    j = 0;
                                }
                            };
                        }
                        $('#divListPesquisa').html(retorno);
                    }
                },
                error: function (reponse) {
                    alert("error : " + reponse);
                    console.log(reponse);
                }
            });
        }
    });

}); //Toast Service

var toasts = {};
toasts.service = {
    info: function info(title, icon, message, position, t) {
        iziToast.show({
            class: 'toast',
            icon: icon,
            title: title,
            message: message,
            titleColor: '#fff',
            messageColor: '#fff',
            iconColor: "#fff",
            backgroundColor: '#ff4e55',
            progressBarColor: '#bc7aff',
            position: position,
            transitionIn: 'fadeInDown',
            close: false,
            timeout: t,
            zindex: 99999
        });
    },
    success: function success(title, icon, message, position, t) {
        iziToast.show({
            class: 'toast',
            icon: icon,
            title: title,
            message: message,
            titleColor: '#fff',
            messageColor: '#fff',
            iconColor: "#fff",
            backgroundColor: '#ff4e55',
            progressBarColor: '#fafafa',
            position: position,
            transitionIn: 'fadeInDown',
            close: false,
            timeout: t,
            zindex: 99999
        });
    },
    error: function error(title, icon, message, position, t) {
        iziToast.show({
            class: 'toast',
            icon: icon,
            title: title,
            message: message,
            titleColor: '#fff',
            messageColor: '#fff',
            iconColor: "#fff",
            backgroundColor: '#ff533d',
            progressBarColor: '#fff',
            position: position,
            transitionIn: 'fadeInDown',
            close: false,
            timeout: t,
            zindex: 99999
        });
    }
};

function initMask() {
    $(".cpf").mask("999.999.999-99");
    $(".celular").mask("(99) 99999-9999");
    //$(".valorProduto").mask('#.##0,00', { reverse: true });
}