﻿global using System.Data.Common;
global using System.Data.SqlClient;
global using System.Runtime.Serialization;
global using MediatR;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using OrionEShopOnContainer.Services.Ordering.API;
global using OrionEShopOnContainer.Services.Ordering.API.Application.IntegrationEvents;
global using OrionEShopOnContainer.Services.Ordering.API.Application.IntegrationEvents.EventHandlers;
global using OrionEShopOnContainer.Services.Ordering.API.Application.IntegrationEvents.Events;
global using OrionEShopOnContainer.Services.Ordering.Domain.AggregatesModel.BuyerAggregate;
global using OrionEShopOnContainer.Services.Ordering.Domain.AggregatesModel.OrderAggregate;
global using OrionEShopOnContainer.Services.Ordering.Domain.Events;
global using OrionEShopOnContainer.Services.Ordering.Domain.Exceptions;
global using OrionEShopOnContainer.Services.Ordering.Domain.SeedWork;
global using Microsoft.Extensions.Options;
global using Swashbuckle.AspNetCore.SwaggerGen;
global using OrionEShopOnContainer.Services.Ordering.API.Extensions;
global using OrionEShopOnContainer.Services.Ordering.API.Application.Commands;
global using OrionEShopOnContainer.Services.Ordering.API.Models;
