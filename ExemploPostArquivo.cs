//public async Task OnPostAsync()
//{

//	if (HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO") == null)
//		RedirectToPage("/Login");
//	else
//		UsuarioLogado = HttpContext.Session.GetObjectFromJson<Usuario>("USUARIO");

//	if (Upload == null)
//		Message = new MensagemModel(EnumeradorModel.TipoMensagem.Aviso, "Problema ao realizar upload do arquivo!");
//	else
//	{
//		var file = Path.Combine(_environment.WebRootPath, _configuration["CaminhoFotoPerfil"], Upload.FileName);
//		using (var fileStream = new FileStream(file, FileMode.Create))
//		{
//			await Upload.CopyToAsync(fileStream);
//		}
//	}
//}

///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
////html
//<form method="post" enctype="multipart/form-data">
//	<div class="col-6">
//		<label class="control-label">Selecione seu arquivo</label>
//		<input type="file" asp-for="Upload" class="form-control-input" name="Upload" id="Upload" accept=".txt,.csv,.xls,.xlsx,.zip" />
//		<label class="control-label"><small>Tipos de arquivos aceitos: .txt,.csv,.xls,.xlsx,.zip</small></label>
//	</div>
//</form>

////////////////////////////////////////////////////////////////////////////////////////////////////////
////appsettings.json

//{
//  "Logging": {
//    "LogLevel": {
//      "Default": "Warning"
//    }
//  },
//  "AllowedHosts": "*",
//  "CaminhoFotoPerfil": "FotosPerfil\\"
//}