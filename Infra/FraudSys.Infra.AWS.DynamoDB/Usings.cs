global using Amazon.DynamoDBv2;
global using Amazon.DynamoDBv2.DataModel;
global using Amazon.DynamoDBv2.Model;
global using Amazon.Extensions.NETCore.Setup;

global using FraudSys.Application.Repository;
global using FraudSys.Domain.LimiteCliente;
global using FraudSys.Domain.LimiteCliente.Repository;
global using FraudSys.Domain.SeedWork;
global using FraudSys.Domain.Transacao;
global using FraudSys.Domain.Transacao.Enum;
global using FraudSys.Domain.Transacao.Repository;
global using FraudSys.Infra.AWS.DynamoDB.Configuration;
global using FraudSys.Infra.AWS.DynamoDB.Exception;
global using FraudSys.Infra.AWS.DynamoDB.Repository;

global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;

global using System.Globalization;