global using FraudSys.Application.Command.AtualizarLimite;
global using FraudSys.Application.Command.CadastrarLimite;
global using FraudSys.Application.Command.EfetuarTransacao;
global using FraudSys.Application.Command.RemoverLimite;
global using FraudSys.Application.Query.BuscarLimite;
global using FraudSys.Application.Query.BuscarTodasTransacoes;
global using FraudSys.Application.Query.BuscarTodosLimites;
global using FraudSys.Application.Repository;
global using FraudSys.Domain.Exception;
global using FraudSys.Domain.LimiteCliente.Validator;
global using FraudSys.Domain.Transacao.Validator;
global using FraudSys.Infra.AWS.DynamoDB.Exception;
global using FraudSys.Infra.AWS.DynamoDB.IoC;
global using FraudSys.Infra.Logger.IoC;
global using FraudSys.Service.API.Configuration;
global using FraudSys.Service.API.Filter;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;