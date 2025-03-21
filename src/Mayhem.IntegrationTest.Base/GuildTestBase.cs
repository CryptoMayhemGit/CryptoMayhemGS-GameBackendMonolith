using Mayhem.Helper;
using Mayhem.Util.Classes;
using Mayhem.WebApi.ActionNames;
using Mayhen.Bl.Commands.AcceptInvitationByOwner;
using Mayhen.Bl.Commands.AcceptInvitationByUser;
using Mayhen.Bl.Commands.AsksToJoinGuildByUser;
using Mayhen.Bl.Commands.ChangeGuildOwner;
using Mayhen.Bl.Commands.CloseGuild;
using Mayhen.Bl.Commands.CreateGuild;
using Mayhen.Bl.Commands.DeclineInvitationByOwner;
using Mayhen.Bl.Commands.DeclineInvitationByUser;
using Mayhen.Bl.Commands.GetGuildById;
using Mayhen.Bl.Commands.GetGuilds;
using Mayhen.Bl.Commands.GetInvitationsByGuildId;
using Mayhen.Bl.Commands.GetInvitationsByUserId;
using Mayhen.Bl.Commands.InviteUserByGuildOwner;
using Mayhen.Bl.Commands.LeaveGuild;
using Mayhen.Bl.Commands.RemoveUserFromGuildByOwner;
using System.Threading.Tasks;

namespace Mayhem.IntegrationTest.Base
{
    public class GuildTestBase<T> : ControllerTestBase<T>
    {
        protected async Task<ActionDataResult<CreateGuildCommandResponse>> CreateGuildAsync(string name, string token) =>
                await httpClientService.HttpPostAsJsonAsync<CreateGuildCommandRequest, CreateGuildCommandResponse>(
                    $"api/{ControllerNames.Guild}/Create",
                     new()
                     {
                         Name = name,
                     }, token);

        protected async Task<ActionDataResult<AskToJoinGuildByUserCommandResponse>> AskToJoinGuildByUserAsync(int guildId, string token) =>
            await httpClientService.HttpPostAsJsonAsync<AskToJoinGuildByUserCommandRequest, AskToJoinGuildByUserCommandResponse>(
                $"api/{ControllerNames.Guild}/Invitation/User/Send",
                new()
                {
                    GuildId = guildId,
                }, token);

        protected async Task<ActionDataResult<CloseGuildCommandResponse>> CloseGuildAsync(string token) =>
            await httpClientService.HttpDeleteAsJsonAsync<CloseGuildCommandResponse>($"api/{ControllerNames.Guild}/Close", token);

        protected async Task<ActionDataResult<InviteUserByGuildOwnerCommandResponse>> InviteUserByGuildOwnerAsync(int invitedUserId, string token) =>
            await httpClientService.HttpPostAsJsonAsync<InviteUserByGuildOwnerCommandRequest, InviteUserByGuildOwnerCommandResponse>(
                $"api/{ControllerNames.Guild}/Invitation/Owner/Send",
                new()
                {
                    InvitedUserId = invitedUserId,
                }, token);

        protected async Task<ActionDataResult<AcceptInvitationByOwnerCommandResponse>> AcceptInvitationByOwnerAsync(int invitationId, string token) =>
            await httpClientService.HttpPutAsJsonAsync<AcceptInvitationByOwnerCommandRequest, AcceptInvitationByOwnerCommandResponse>(
                $"api/{ControllerNames.Guild}/Invitation/Owner/Accept", new()
                {
                    InvitationId = invitationId
                }, token);

        protected async Task<ActionDataResult<AcceptInvitationByUserCommandResponse>> AcceptInvitationByUserAsync(int invitationId, string token) =>
            await httpClientService.HttpPutAsJsonAsync<AcceptInvitationByUserCommandRequest, AcceptInvitationByUserCommandResponse>(
                $"api/{ControllerNames.Guild}/Invitation/User/Accept",
                new()
                {
                    InvitationId = invitationId,
                }, token);

        protected async Task<ActionDataResult<DeclineInvitationByOwnerCommandResponse>> DeclineInvitationByOwnerAsync(int invitationId, string token) =>
            await httpClientService.HttpDeleteAsJsonAsync<DeclineInvitationByOwnerCommandRequest, DeclineInvitationByOwnerCommandResponse>(
                $"api/{ControllerNames.Guild}/Invitation/Owner/Decline",
                new()
                {
                    InvitationId = invitationId,
                }, token);

        protected async Task<ActionDataResult<DeclineInvitationByUserCommandResponse>> DeclineInvitationByUserAsync(int invitationId, string token) =>
            await httpClientService.HttpDeleteAsJsonAsync<DeclineInvitationByUserCommandRequest, DeclineInvitationByUserCommandResponse>(
                $"api/{ControllerNames.Guild}/Invitation/User/Decline",
                new()
                {
                    InvitationId = invitationId,
                }, token);

        protected async Task<ActionDataResult<RemoveUserFromGuildByOwnerCommandResponse>> RemoveUserFromGuildByOwnerAsync(int removedUserId, string token) =>
            await httpClientService.HttpDeleteAsJsonAsync<RemoveUserFromGuildByOwnerCommandRequest, RemoveUserFromGuildByOwnerCommandResponse>(
                $"api/{ControllerNames.Guild}/User/Remove",
                new()
                {
                    RemovedUserId = removedUserId,
                }, token);

        protected async Task<ActionDataResult<LeaveGuildCommandResponse>> LeaveGuildAsync(string token) =>
            await httpClientService.HttpDeleteAsJsonAsync<LeaveGuildCommandResponse>($"api/{ControllerNames.Guild}/User/Leave", token);

        protected async Task<ActionDataResult<ChangeGuildOwnerCommandResponse>> ChangeGuildOwnerAsync(int newOwnerId, string token) =>
            await httpClientService.HttpPutAsJsonAsync<ChangeGuildOwnerCommandRequest, ChangeGuildOwnerCommandResponse>(
                $"api/{ControllerNames.Guild}/Owner/Change",
                new()
                {
                    NewOwnerId = newOwnerId,
                }, token);

        protected async Task<ActionDataResult<GetGuildByIdCommandResponse>> GetGuildByIdAsync(int guildId, string token) =>
            await httpClientService.HttpGetAsJsonAsync<GetGuildByIdCommandResponse>($"api/{ControllerNames.Guild}/{guildId}", token);

        protected async Task<ActionDataResult<GetGuildsCommandResponse>> GetGuildsAsync(int? skip, int? limit, string name, string token) =>
            await httpClientService.HttpGetAsJsonAsync<GetGuildsCommandResponse>(
                $"api/{ControllerNames.Guild}/?{new GetGuildsCommandRequest() { Skip = skip, Limit = limit, Name = name, }.ToQueryString()}",
                token);

        protected async Task<ActionDataResult<GetInvitationsByGuildIdCommandResponse>> GetInvitationsByGuildIdAsync(int guildId, string token) =>
            await httpClientService.HttpGetAsJsonAsync<GetInvitationsByGuildIdCommandResponse>($"api/{ControllerNames.Guild}/{guildId}/Invitation", token);

        protected async Task<ActionDataResult<GetInvitationsByUserIdCommandResponse>> GetInvitationsByUserIdAsync(string token) =>
            await httpClientService.HttpGetAsJsonAsync<GetInvitationsByUserIdCommandResponse>($"api/{ControllerNames.Guild}/Invitation/User", token);
    }
}
