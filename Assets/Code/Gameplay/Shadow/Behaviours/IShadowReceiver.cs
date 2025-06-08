namespace Code.Gameplay.Shadow.Behaviours
{
    public interface IShadowReceiver
    {
        void BeginReceive();
        void Receive();
        void EndReceive();
    }
}
