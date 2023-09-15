# Plugin Pagar.Me para NopCommerce
Este plugin usa o [SDK oficial da Pagar.Me pada .NET](https://github.com/pagarme/pagarme-net-standard-sdk/tree/main)
 - O nome da Pasta do projeto deve ser Nop.Plugin.Payments.PagarMe
## Configurações na NopCommerce
Para utilizar este plugin deve-se ajustar na NopCommerce o atributo customizado para o CPF, que deve ser obrigatório
Além disso deve ser obrigatório o cadastro de um endereço ao criar-se uma conta, e o cadastro de um contato por telefone ou celular
Assim como o email

Nas configurações do Plugin deve-se preencher as chaves Secretas e Públicas tanto de Produção como de Testes, além do login e senha da conta

## Configurações no código fonte
Para o método de pagamento pix é obrigatório no Pagar.Me um campo adicional, preenchido neste plugin de maneira estática

No endereço do cliente foi deixado estático o País do cliente como BR (Brasil), a maneira de preencher o campo é pelo codigo ISO 3166-1 alpha-2 do País
