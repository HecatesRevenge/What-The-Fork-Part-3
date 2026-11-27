using System;
using System.Collections.Generic;
using System.Text.Json;
namespace WhatTheForkMVC.Classes.Chatbot
{

    public class ChatTurn
    {
        public String Role { get; set; }
        public String Text { get; set; }
    }
    public class ChatHistory
    {
        private const String SessionKey = "ChatHistory";
        private const int MaxTurns = 20;

        public static List<ChatTurn> Load(ISession session)
        {
            String json = session.GetString(SessionKey);
            if (json==null)
            {
                return new List<ChatTurn>();
            }

            return JsonSerializer.Deserialize<List<ChatTurn>>(json);
        }

        public static void Save(ISession session, List<ChatTurn> turns)
        {
            while (turns.Count > MaxTurns)
            {
                turns.RemoveAt(0);
            }
            session.SetString(SessionKey, JsonSerializer.Serialize(turns));
        }

        public static void Clear(ISession session)
        {
            session.Remove(SessionKey);
        }
    }
}
