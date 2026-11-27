
using System;
using System.Collections.Generic;
using Microsoft.Extensions.AI;
using WhatTheForkMVC.Views.Chatbot;
using WhatTheForkMVC.Classes.Chatbot;
using System.Threading.Tasks;

namespace WhatTheForkMVC.Classes.Chatbot
{
    public class ChatService
    {
        private IChatClient chatClient;

        public ChatService(IChatClient theChatClient)
        {
            chatClient = theChatClient;
        }

        public List<ChatMessage> BuildMessages(List<ChatTurn> turns)
        {
            List<ChatMessage> messages = new List<ChatMessage>();
            foreach (ChatTurn turn in turns)
            {
                ChatRole role = ChatRole.Assistant;
                if (turn.Role == "user")
                {
                    role = ChatRole.User;
                }
                messages.Add(new ChatMessage(role, turn.Text));
            }
            return messages;
        }

        public ChatOptions BuildOptions(int userID, String firstName, String currentFilters)
        {
            RecommandationTools tools = new RecommandationTools(userID);

            ChatOptions options = new ChatOptions();
            //This is the text that claude reads to understand what the user wants and how to use the tools
            //This will be tweaked as output is massaged to get better results. 

            options.Tools = new List<AITool>
            {
                //Turns the methods in RecommandationTools into tools that claude can call
                AIFunctionFactory.Create(tools.GetUserReviews),
                AIFunctionFactory.Create(tools.GetUserSearchHistory),
                AIFunctionFactory.Create(tools.GetSearchOptions),
                AIFunctionFactory.Create(tools.SearchRestaurants)
            };
            return options;
        }

        public async Task<String> GetReplyAsync(List<ChatTurn> turns, int userID, String firstName, String currentFilters)
        {
            //runs whole chat loop. Claude asks for a tool, method runs, returns result to claude, claude responds, repeat until done
            ChatResponse response = await chatClient.GetResponseAsync(
                BuildMessages(turns), BuildOptions(userID, firstName, currentFilters));
            return response.Text;
        }

    }
}
