$('#input_apiKey').change(function () {
    var key = $('#input_apiKey')[0].value;
    var credentials = key.split(':');
    $.ajax({
        url: "http://localhost:65371/token",
        type: "post",
        contenttype: 'x-www-form-urlencoded',
        data: "grant_type=password&username=" + credentials[0] + "&password=" + credentials[1],
        success: function (response) { 
            var bearerToken = "Bearer " + response.access_token;

            window.swaggerUi.api.clientAuthorizations.remove('api_key');

            var apiKeyAuth = new SwaggerClient.ApiKeyAuthorization("Authorization", bearerToken, "header");

            window.swaggerUi.api.clientAuthorizations.add('oauth2', apiKeyAuth);

            alert("Login Succesfull!");

        },
        error: function (xhr, ajaxoptions, thrownerror) {
            alert("Login failed!");
        }
    });
});