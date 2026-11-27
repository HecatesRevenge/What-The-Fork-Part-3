using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.Threading;
using WhatTheForkMVC.Classes;
using WhatTheForkMVC.Classes.Chatbot;
using WhatTheForkMVC.Classes.Login;

namespace WhatTheForkMVC.Controllers
{
    public class ChatController : Controller
    {
        private ChatService chatService;

        public ChatController(IChatClient chatClient)
        {
            chatService = new ChatService(chatClient);
        }

        [HttpPost]
        public async Task<IActionResult> Send(String message, String filters)
        {
            if (!SessionUser.IsLoggedIn(HttpContext.Session))
            {
                return Json(new { reply = "Please log in to use recommendor chat." });
            }
            if (String.IsNullOrWhiteSpace(message))
            {
                return Json(new { reply = "" });
            }

            if (String.IsNullOrEmpty(filters))
            {
                filters = "no search yet";
            }

            UserAccounts user = SessionUser.GetUser(HttpContext.Session);
            List<ChatTurn> turns = ChatHistory.Load(HttpContext.Session);
            turns.Add(new ChatTurn { Role = "user", Text = message });

            String reply;
            try
            {
                reply = await chatService.GetReplyAsync(turns, user.UserID, user.FirstName, filters);
            }
            catch (Exception ex)
            {
                return Json(new { reply = "Sorry, chat services aren't available right now. Please try again later" });
            }

            turns.Add(new ChatTurn { Role = "assistant", Text = reply });
            ChatHistory.Save(HttpContext.Session, turns);
            return Json(new { reply = reply });
        }

        [HttpPost]
        public IActionResult Clear()
        {
            ChatHistory.Clear(HttpContext.Session);
            return Json(new { cleared = true });
        }
    }

}

