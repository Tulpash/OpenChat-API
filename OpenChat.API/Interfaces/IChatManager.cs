﻿namespace OpenChat.API.Interfaces
{
    public interface IChatManager
    {
        public void AddConnection(string? userName, string connectionId);

        public void RemoveConnection(string connectionId);
    }
}