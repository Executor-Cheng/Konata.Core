﻿using System;
using System.Threading.Tasks;
using Konata.Core.Events;

namespace Konata.Core.Components.Logics;

internal class BaseLogic : IBaseLogic
{
    internal BusinessComponent Context { get; }

    internal BusinessComponent BusinessComponent
        => Context.GetComponent<BusinessComponent>();

    internal ConfigComponent ConfigComponent
        => Context.GetComponent<ConfigComponent>();

    internal PacketComponent PacketComponent
        => Context.GetComponent<PacketComponent>();

    internal ScheduleComponent ScheduleComponent
        => Context.GetComponent<ScheduleComponent>();

    internal SocketComponent SocketComponent
        => Context.GetComponent<SocketComponent>();

    internal HighwayComponent HighwayComponent
        => Context.GetComponent<HighwayComponent>();

    internal BaseLogic(BusinessComponent context)
    {
        Context = context;
    }

    public virtual Task Incoming(ProtocolEvent e)
        => throw new NotImplementedException();
}
