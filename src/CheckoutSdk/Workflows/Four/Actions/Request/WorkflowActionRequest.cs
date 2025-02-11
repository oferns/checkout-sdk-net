﻿namespace Checkout.Workflows.Four.Actions.Request
{
    public abstract class WorkflowActionRequest
    {
        public WorkflowActionType Type { get; set; }

        protected WorkflowActionRequest(WorkflowActionType type)
        {
            Type = type;
        }
    }
}