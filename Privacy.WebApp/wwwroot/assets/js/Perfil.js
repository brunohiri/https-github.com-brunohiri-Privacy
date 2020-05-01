
function mascara(o, f) {
    v_obj = o
    v_fun = f
    setTimeout("execmascara()", 1)
}

function execmascara() {
    v_obj.value = v_fun(v_obj.value)
}

function mreais(v) {
    v = v.replace(/\D/g, "") //Remove tudo o que não é dígito
    v = v.replace(/(\d{2})$/, ",$1") //Coloca a virgula
    v = v.replace(/(\d+)(\d{3},\d{2})$/g, "$1.$2") //Coloca o primeiro ponto

    if (v.length >= 5) {
        var maximo = v.replace(/\./g, '').replace(',', '.') > 1000;
        var minimo = v.replace(/\./g, '').replace(',', '.') < 0;

        if (maximo) {
            return '1.000,00';
        } else if (minimo) {
            return '1,00';
        } else {
            return v;
        }
    } else {
        return v;
    }




}