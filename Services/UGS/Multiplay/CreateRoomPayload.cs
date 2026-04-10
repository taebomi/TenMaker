namespace TenMaker.Services.UGS.Multiplay
{
    public struct CreateRoomPayload
    {
        public string JoinCode;
        
        public CreateRoomPayload(string joinCode)
        {
            JoinCode = joinCode;
        }
    }
}