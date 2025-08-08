﻿using Swashbuckle.AspNetCore.Filters;
using FGC.Application.Common;
using FGC.Domain.Core;



public class GenericErrorBadRequestExample : IExamplesProvider<BaseResponse<object>>
{
    public BaseResponse<object> GetExamples()
    {
        var notification = new NotificationModel
        {
            NotificationType = NotificationModel.ENotificationType.BadRequestError
        };

        notification.AddMessage("Field", "Field Required");

        notification.AddMessage("Error", "Generic validation error");

        return BaseResponse<object>.Fail(notification);
    }
}
