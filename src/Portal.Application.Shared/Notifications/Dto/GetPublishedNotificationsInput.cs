﻿using System;

namespace Portal.Notifications.Dto
{
    public class GetPublishedNotificationsInput
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}