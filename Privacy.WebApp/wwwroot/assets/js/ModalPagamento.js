function div_Clicked(itemName) {

    alert(itemName);

    //var produto = $(itemName).val();

    //console.log(produto);

    var url = "/MyProfile?handler=ObterDadosUsuarioLoado";
    $.ajax({
        url: url,
        //data: { IdComentario: idComentario },
        cache: false,
        async: true,
        type: "GET",
        success: function (data) {

            console.log(data.Usuario);

            var valorProdutoMensal = $("#valorProdutoMensal").val().replace(",", "");
            var valorProdutoTrimestral = $("#valorProdutoTrimestral").val().replace(",", "");
            var valorProdutoSemestral = $("#valorProdutoSemestral").val().replace(",", "");



            var idProduto = $("#idProduto").val();

            var d = new Date();
            var currentDate = d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate();

            // inicia a instância do checkout
            var checkout = new PagarMeCheckout.Checkout({
                encryption_key: 'ek_test_YTEFhGJ5f814Q75R3xMKPmTmNKAtK7',
                success: function (data) {
                    console.log(data);

                    var url = "/AddCreditCard";
                    $.ajax({
                        url: url,
                        data: { TokenTransacao: data.token, IdPerfil: idProduto },
                        cache: false,
                        async: true,
                        type: "GET",
                        success: function (data) {

                            if (data.OK) {
                                var url = "ProfilePhotos?Id=" + idProduto + "";
                                window.location.href = url;
                            }
                        },
                        error: function (reponse) {
                            //alert("error : " + reponse);
                        }
                    });
                },
                error: function (err) {
                    console.log(err);
                },
                close: function () {
                    console.log('The modal has been closed.');
                }
            });

            // Obs.: é necessário passar os valores boolean como string
            checkout.open({
                //amount: valorProduto,
                buttonText: 'Pagar',
                buttonClass: 'botao-pagamento',
                customerData: 'false',
                createToken: 'true',
                paymentMethods: 'credit_card,boleto',
                customer: {
                    external_id: data.Usuario.IdUsuario,
                    name: data.Usuario.Nome,
                    type: 'individual',
                    country: 'br',
                    email: data.Usuario.Email,
                    documents: [
                        {
                            type: 'cpf',
                            number: data.Usuario.CPF
                        },
                    ],
                    phone_numbers: ["+55" + data.Usuario.Celular.replace("(", "").replace(")", "").replace("-", "") + ""], //TRATAR
                    birthday: data.Usuario.DataNascimento.substr(0, 10)
                },
                billing: {
                    name: data.Usuario.Nome,
                    address: {
                        country: 'br',
                        state: data.Usuario.Estado,
                        city: data.Usuario.Cidade,
                        //neighborhood: 'Fulanos bairro',
                        street: 'Rua Landolfo de Andrade', //OBRIGATÓRIO
                        //street_number: '123',
                        zipcode: '05855290'
                    }
                },
                shipping: {
                    name: data.Usuario.Nome,
                    fee: 0,
                    delivery_date: currentDate, //TRATAR
                    expedited: true,
                    address: {
                        country: 'br',
                        state: data.Usuario.Estado,
                        city: data.Usuario.Cidade,
                        //neighborhood: 'Fulanos bairro',
                        street: 'Rua Landolfo de Andrade', //OBRIGATÓRIO
                        //street_number: '123',
                        zipcode: '05855290'
                    }
                },
                items: [
                    {
                        id: idProduto,
                        title: 'Assinatura Privacy',
                        //unit_price: valorProduto,
                        quantity: 1,
                        tangible: true
                    }
                ]
            });

        },
        error: function (reponse) {
            //alert("error : " + reponse);
        }
    });

}