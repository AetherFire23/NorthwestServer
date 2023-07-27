using Shared_Resources.Constants.Mapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared_Resources.Constants.Endpoints;

[ControllerPathMapper(GameController)]
public class GameEndpoints
{
    public static string GetFullPath(string endpointName) => EndpointPathsMapper.GetFullEndpoint(typeof(GameEndpoints), endpointName);
    public const string GameController = nameof(GameController);
    public const string GameState = nameof(GameState);
    public const string ExecuteGameTask = nameof(ExecuteGameTask);
    public const string TransferItem = nameof(TransferItem);
    public const string ChangeRoom = nameof(ChangeRoom);
    public const string UpdatePlayerPosition = nameof(UpdatePlayerPosition);
}
